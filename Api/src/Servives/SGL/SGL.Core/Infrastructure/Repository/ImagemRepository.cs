using Core.Abstractions.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using NRediSearch;
using SGL.Core.Domain.Entities;
using SGL.Domain.Repository;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SGL.Infrastructure.Repository
{

    public class ImagemCacheRepository : ICacheRepository<Imagem>, IImagemCacheRepository
    {
        private Client ClienteRedis = null;
        private string ConnectionString = string.Empty;
        public ElasticClient ClienteElastic;        
        private readonly StackExchange.Redis.IDatabase Database;
        private readonly ConnectionMultiplexer Redis;
        
        private readonly string _index = "imagem";

        public ImagemCacheRepository(ConnectionMultiplexer redis)
        {
            Redis = redis;
            Database = redis.GetDatabase();
            var pass = Task.FromResult(CreateIndex());
        }

        private async Task<bool> CreateIndex()
        {
            // Defining a schema for an index and creating it:
            var idximagem = new Schema().AddTextField("Nome", 6).AddTextField("ContentType", 5).AddTextField("BytesString64", 4);
            bool result = false;
            try
            {
                ClienteRedis = new Client("idx_imagem_search", Database);

                if (ClienteRedis != null)
                {
                    try { ClienteRedis.DropIndex(); } catch { } // reset DB
                }

                result = await ClienteRedis.CreateIndexAsync(idximagem, Client.IndexOptions.Default);

                result = true;
            }
            catch (RedisServerException ex)
            {
                // TODO: Convert to Skip
                if (ex.Message == "ERR unknown command 'FT.CREATE'")
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Module not installed, aborting");
                    result = false; // the module isn't installed
                }
            }
            return result;
        }

        public IEnumerable<string> GetIds()
        {
            var query = GetServer().Keys(pattern: $"{_index}:*").ToList().Select(x => x.ToString()).AsEnumerable();
            return query;
        }

        public string ValueIndexName(string id)
        {
            return $"{_index}:{id}";
        }

        public string IndexName(string id)
        {
            return $"idx_{_index}:{id}";
        }

        public bool ExistId(string id)
        {
            var query = GetServer().Keys(pattern: ValueIndexName(id)).Any();
            return query;
        }

        public async Task<bool> SetAsync(Imagem entity)
        {
            try
            {
                var created = await Database.StringSetAsync(ValueIndexName(entity.Id.ToString()), JsonConvert.SerializeObject(entity));
                if (!created)
                {
                    Console.WriteLine("Problem occur persisting the item.");
                    return false;
                }

                var fields = new Dictionary<string, RedisValue>
                {
                    ["Nome"] = entity.Nome ?? "",
                    ["ContentType"] = entity.ContentType ?? "",
                    ["BytesString64"] = Convert.ToBase64String( entity.Bytes)
                };

                var chaveIndex = IndexName(entity.Id.ToString());

                var result = false;

                if (ExistId(entity.Id.ToString()))
                {
                    result = await Task.FromResult(ClienteRedis.DeleteDocument(chaveIndex));
                }

                result = await ClienteRedis.AddDocumentAsync(chaveIndex, fields);

                return result;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return false;
            }
        }

        private async Task<bool> AddOnIndexAsync(Imagem entity)
        {
            try
            {
                var fields = new Dictionary<string, RedisValue>
                {
                    ["Nome"] = entity.Nome ?? "",
                    ["ContentType"] = entity.ContentType ?? "",
                    ["BytesString64"] = Convert.ToBase64String(entity.Bytes)
                };

                return await ClienteRedis.AddDocumentAsync(IndexName(entity.Id.ToString()), fields);

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var delete = await Database.KeyDeleteAsync(IndexName(id));
            return delete;
        }

        public async Task<Imagem> GetAsync(long imagemId)
        {
            return await GetAsync(imagemId.ToString(), true);
        }

        public async Task<Imagem> GetAsync(string imagemId, bool useIndexName = false)
        {
            var data = await Database.StringGetAsync(useIndexName ? ValueIndexName(imagemId) : imagemId);
            if (data.IsNullOrEmpty)
            {
                return null;
            }
            var imagem = JsonConvert.DeserializeObject<Imagem>(data.ToString());
            return imagem;
        }

        public async Task<QueryResult<Imagem>> GetAllAsync(string texto = "", int pageSize = 10, int currentPage = 1)
        {
            var retorno = new QueryResult<Imagem>();
            try
            {
                if (!string.IsNullOrEmpty(texto.Trim()))
                {
                    var match = Regex.Match(texto.Trim(), @"[-.]+");

                    if (!string.IsNullOrEmpty(match.Value))
                    {
                        texto = Regex.Replace(texto, @"[-.]", "");
                    }

                    var busca = Regex.Replace(texto.Trim(), @"[^\w\d Çç~^áàéèíìóòúùÁÀÉÈÍÌÓÒÚÙâãêîôõûÂÃÊÎÔÕÛäëïöüÄËÏÖÜñÿýÝ'!-@#$%¨&*()_+,.:?\\º¹²³£¢¬/¼½¾<>®±© ]", "").ToLower();
                    var q = new Query(busca).Limit((currentPage - 1) * pageSize, pageSize);
                    q.SortBy = "Nome";
                    var result = await ClienteRedis.SearchAsync(q);

                    if (result.TotalResults > 0)
                    {
                        retorno.Resultado = result.Documents.Select(doc => new Imagem(Convert.ToInt64( doc.Id.Replace($"idx_{_index}:", "")), doc["Nome"], doc["ContentType"], Convert.FromBase64String(doc["BytesString64"]))).ToList();
                        retorno.Total = Convert.ToInt32(result.TotalResults);
                    }
                }
                return retorno;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return null;
            }
        }

        private IServer GetServer()
        {
            var endpoint = Redis.GetEndPoints();
            return Redis.GetServer(endpoint.First());
        }
    }
}