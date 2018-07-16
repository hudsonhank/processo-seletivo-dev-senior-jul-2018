using Core.Abstractions.Types.Exception;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Abstractions.Infrastructure
{
    public abstract class BaseDapperQuery<T> where T : class
    {        
        protected string ConnectionString;
        protected ICacheRepository<T> CacheRepository;

        public string Alias { get; set; } = "qra";
        public string OrderBy { get; set; } = "Id";
        public string CampoChavePrimaria { get; private set; } = "Id";

        public string SqlBase { get; private set; }
    
        protected string SqlPaginacao()
        {
            return String.Format(@" order by {0}.{1} OFFSET(@currentPage - 1) * @pageSize ROWS FETCH NEXT @pageSize ROWS ONLY", Alias, OrderBy);
        }

        protected string QueryBase(string filtrarPor)
        {
            return String.Format("{0} select * from Resultado {3} where {3}.Ativo > 0 {2} {1} ", SqlBase, SqlPaginacao(), filtrarPor, Alias);
        }

        protected string QueryCount(string filtrarPor)
        {
            return String.Format("{0} select count(1) from Resultado {2} where {2}.Ativo>0 {1}", SqlBase, filtrarPor, Alias);
        }

        protected string QueryChavePrimaria(string valor)
        {
            //var query = String.Format("{0} select top 1 * from Resultado {1} where {1}.Ativo > 0 and {1}.{2} = @valorChave", SqlBase, Alias, CampoChavePrimaria, valor);
            return String.Format("{0} select top 1 * from Resultado {1} where {1}.Ativo > 0 and {1}.{2} = '{3}'", SqlBase, Alias, CampoChavePrimaria, valor);
        }

        protected BaseDapperQuery(ICacheRepository<T> cache, string connectionString, string sqlBase, string campoChavePrimaria="Id")
        {
            CampoChavePrimaria = campoChavePrimaria;
            CacheRepository = cache;
            SqlBase = sqlBase;
            ConnectionString = connectionString ?? throw new ArgumentException(nameof(connectionString));
        }

        public async Task<T> FirstOrDefault(string identificador = "")
        {
            T result = null;
            if (!string.IsNullOrEmpty(identificador) && CacheRepository != null)
            {
                //buscar no cache full-text
                var dados = await CacheRepository.GetAsync(identificador);
                if (dados != null)
                {
                    result = dados;
                }
            }

            using (var connection = new SqlConnection(ConnectionString))
            {
                result = await FirstOrDefault(identificador, connection);
            }

            if (result == null)
                throw new NaoEncontradoException($"Nenhum resultado para o identificador {identificador}.");

            return result;
        }

        protected async Task<T> FirstOrDefault(string identificador, SqlConnection connection = null)
        {
            //var filtros = new DynamicParameters();
            //filtros.Add("valorChave", identificador, DbType.String);
            return await connection.QueryFirstOrDefaultAsync<T>(QueryChavePrimaria(identificador));
        }

        protected async Task<QueryResult<T>> QueryPagination(DynamicParameters filtros = null, string filtrarPor ="", 
            string texto = "", int pageSize = 10, int currentPage = 1) 
        {
            var research = new QueryResult<T>();
            //buscar no cache full-text
            if (!string.IsNullOrEmpty(texto) && CacheRepository!=null)
            {
                var dados = await CacheRepository.GetAllAsync(texto, pageSize, currentPage);
                if (dados != null && dados.Total > 0)
                {
                    return dados;
                }
            }

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if(filtros==null)
                {
                    filtros = new DynamicParameters();
                }               
                //Count
                research.Total = await connection.QueryFirstAsync<long>(QueryCount(filtrarPor), filtros);

                //Query principal
                filtros.Add("pageSize", pageSize, DbType.Int32);
                filtros.Add("currentPage", currentPage, DbType.Int32);
                research.Resultado = await connection.QueryAsync<T>(QueryBase(filtrarPor), filtros);
            }

            if (research.Total == 0)
                throw new NaoEncontradoException($"Nenhum resultado da pesquisa encontrado.");

            return research;
        }
        
        protected async Task<List<T>> Query(DynamicParameters filtros, SqlConnection connection, string filtrarPor = "")
        {
            var queryResult = await connection.QueryAsync<T>(QueryBase(filtrarPor), filtros);

            if (queryResult.Any())
            {
                return queryResult.AsList();
            }
            return default(List<T>);
        }               
    }
}
