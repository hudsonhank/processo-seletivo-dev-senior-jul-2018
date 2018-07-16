using Core.Abstractions.Infrastructure;
using Core.Abstractions.Infrastructure.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using NRediSearch;
using SGL.Core.Domain.Entities;
using SGL.Core.Domain.Entities.Queries;
using SGL.Domain.Repository;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SGL.Infrastructure.Repository
{
    public class LivroRepository : BaseRepository<LivroEntity>, ILivroRepository
    {
        public LivroRepository(IDatabaseContext context) : base(context)
        {
        }
        public Task<LivroEntity> BuscarPorCodigoUnico(string codigoUnico, bool includes = true)
        {
            Expression<Func<LivroEntity, bool>> CodigoUnicoExpression = (c => c.CodigoUnico == codigoUnico);

            var membro = Buscar(CodigoUnicoExpression, includes);

            return membro;
        }
    }

    public class LivroCacheRepository : ICacheRepository<LivroQuery>, ILivroCacheRepository
    {
        private Client ClienteRedis = null;
        private string ConnectionString = string.Empty;
        public ElasticClient ClienteElastic;
        private readonly ILogger<LivroCacheRepository> Logger;
        private readonly StackExchange.Redis.IDatabase Database;
        private readonly ConnectionMultiplexer Redis;
        private readonly ILivroRepository Repository;
        private readonly string _index = "livro";

        public LivroCacheRepository(ILoggerFactory loggerFactory, IConfiguration Configuration, ILivroRepository repository, ConnectionMultiplexer redis)
        {
            Logger = loggerFactory.CreateLogger<LivroCacheRepository>();
            Repository = repository;
            Redis = redis;
            Database = redis.GetDatabase();

            /*var settings = new ConnectionSettings(new Uri(Configuration["Connection:Elastic"]))
            .DefaultMappingFor<LivroQuery>(m => m.IndexName("e_livro"));
            ClienteElastic = new ElasticClient(settings);*/

            var pass = Task.FromResult(CreateIndex());
        }

        private async Task<bool> CreateIndex()
        {
            // Defining a schema for an index and creating it:
            var idxLivro = new Schema().AddTextField("CodigoUnico", 6).AddTextField("Titulo", 5).AddTextField("Autor", 4).AddTextField("Genero", 3).AddTextField("Editora", 2).AddNumericField("PublicacaoAno");
            bool result = false;
            try
            {
                ClienteRedis = new Client("idx_livro_search", Database);

                if (ClienteRedis != null)
                {
                    try { ClienteRedis.DropIndex(); } catch { } // reset DB
                }

                result = ClienteRedis.CreateIndex(idxLivro, Client.IndexOptions.Default);

                await InsertRecordBulkBatch();

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

        public async Task<bool> SetAsync(LivroQuery entity)
        {
            try
            {
                var created = await Database.StringSetAsync(ValueIndexName(entity.Id), JsonConvert.SerializeObject(entity));
                if (!created)
                {
                    Logger.LogInformation("Problem occur persisting the item.");
                    return false;
                }

                var fields = new Dictionary<string, RedisValue>
                {
                    ["CodigoUnico"] = entity.CodigoUnico ?? "",
                    ["Titulo"] = entity.Titulo ?? "",
                    ["Autor"] = entity.Autor ?? "",
                    ["Genero"] = entity.Genero ?? "",
                    ["Editora"] = entity.Editora ?? "",
                    ["PublicacaoAno"] = entity.DataPublicacao.Year,
                    ["DataPublicacao"] = entity.DataPublicacao.ToShortDateString(),
                    ["TextoExtra"] = entity.TextoExtra ?? "",
                };

                var chaveIndex = IndexName(entity.Id);

                var result = false;

                if (ExistId(entity.Id))
                {
                    result = await Task.FromResult(ClienteRedis.DeleteDocument(chaveIndex));
                }

                result = await ClienteRedis.AddDocumentAsync(chaveIndex, fields);
                //result = await SetElasticAsync(entity);

                return result;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return false;
            }
        }

        public async Task<bool> SetElasticAsync(LivroQuery entity)
        {
            var response = await ClienteElastic.UpdateAsync(DocumentPath<LivroQuery>.Id(entity.Id),
                u => u.Index("e_livro").Type("livro").DocAsUpsert(true).Doc(entity));

            return true;
        }

        private async Task<bool> AddOnIndexAsync(LivroQuery entity)
        {
            try
            {
                var fields = new Dictionary<string, RedisValue>
                {
                    ["CodigoUnico"] = entity.CodigoUnico ?? "",
                    ["Titulo"] = entity.Titulo ?? "",
                    ["Autor"] = entity.Autor ?? "",
                    ["Genero"] = entity.Genero ?? "",
                    ["Editora"] = entity.Editora ?? "",
                    ["PublicacaoAno"] = entity.DataPublicacao.Year,
                    ["DataPublicacao"] = entity.DataPublicacao.ToShortDateString(),
                    ["TextoExtra"] = entity.TextoExtra ?? ""

                };

                return await ClienteRedis.AddDocumentAsync(IndexName(entity.Id), fields);

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

        public async Task<LivroQuery> GetAsync(long livroId)
        {
            return await GetAsync(livroId.ToString(), true);
        }

        public async Task<LivroQuery> GetAsync(string livroId, bool useIndexName = false)
        {
            var data = await Database.StringGetAsync(useIndexName ? ValueIndexName(livroId) : livroId);
            if (data.IsNullOrEmpty)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<LivroQuery>(data.ToString());
        }

        public async Task<QueryResult<LivroQuery>> GetAllAsync(string texto = "", int pageSize = 10, int currentPage = 1)
        {
            var retorno = new QueryResult<LivroQuery>();
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
                    q.SortBy = "Titulo";
                    var result = await ClienteRedis.SearchAsync(q);

                    if (result.TotalResults > 0)
                    {
                        retorno.Resultado = result.Documents.Select(doc => new LivroQuery(doc.Id.Replace($"idx_{_index}:", ""), doc["CodigoUnico"], doc["Titulo"],
                            doc["Autor"], doc["Genero"], doc["Editora"], Convert.ToDateTime(doc["DataPublicacao"]), doc["TextoExtra"])).ToList();
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

        public async Task<bool> InsertRecordBulkBatch()
        {
            var items = await BuscarTodos();
            //Batching
            var list = new List<Task<bool>>();
            IBatch batch = Database.CreateBatch();
            foreach (var item in items.ToList())
            {
                list.Add(batch.StringSetAsync(ValueIndexName(item.Id), JsonConvert.SerializeObject(item)));
            }

            batch.Execute();

            await Task.WhenAll(list.ToArray());

            return true;
        }

        private async Task<List<LivroQuery>> BuscarTodos()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var lista = await connection.QueryAsync<LivroQuery>(@"Select a.Ativo, a.Id, Year(a.DataPublicacao) PublicacaoAno, a.CodigoUnico, a.Titulo, a.Genero, a.Autor, a.Editora, a.DataPublicacao, b.Nome TextoExtra From Livro a left JOIN Imagem b ON a.CapaId = b.Id");
                return lista.ToList();
            }
        }
    }
}