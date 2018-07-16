using AutoMapper;
using Core.Abstractions.Infrastructure;
using Core.Abstractions.Types.Exception;
using Dapper;
using Microsoft.Extensions.Configuration;
using SGL.Application.Livro.Commands;
using SGL.Core.Domain.Entities;
using SGL.Core.Domain.Entities.Queries;
using SGL.Domain.Repository;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SGL.Application.Livro.Queries
{
    public interface ILivroQueries
    {
        Task<QueryResult<LivroQuery>> BuscarLista(LivroQuery filtros = null, string texto = "", int pageSize = 10, int currentPage = 1);
        Task<EditarLivroCommand> Buscar(long id);
    }

    public class LivroQueries : BaseDapperQuery<LivroQuery>, ILivroQueries
    {
        private readonly IMapper Mapper;

        private const string SQL_BASE = @";WITH Resultado as (Select a.*, Year(a.DataPublicacao) PublicacaoAno, b.Nome TextoExtra From Livro a left JOIN Imagem b ON a.CapaId = b.Id)";
        
        public LivroQueries(ILivroCacheRepository cacheRepository, IMapper mapper, IConfiguration Configuration):
            base(cacheRepository, Configuration["Connection:Relacional"], SQL_BASE)
        {
            CacheRepository = cacheRepository ?? throw new ArgumentNullException(nameof(cacheRepository));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<EditarLivroCommand> Buscar(long id)
        {
            EditarLivroCommand result = null;
            using (var connection = new SqlConnection(ConnectionString))
            {
                result = await connection.QueryFirstOrDefaultAsync<EditarLivroCommand>(QueryChavePrimaria(id.ToString()));
            }

            if (result == null)
                throw new NaoEncontradoException($"Nenhum resultado para o identificador {id.ToString()}.");

            return result;
        }

        public async Task<QueryResult<LivroQuery>> BuscarLista(LivroQuery filtros = null, string texto = "", int pageSize = 10, int currentPage = 1)
        {
            var parametrosQuery = new DynamicParameters(new { filtros });

            var filtrarPor = "";

            texto = filtros.TextoExtra;

            if (!string.IsNullOrWhiteSpace(filtros.Titulo) || !string.IsNullOrEmpty(texto))
            {
                filtrarPor += " and qra.Titulo like @titulo";
                parametrosQuery.Add("titulo", $"%{filtros.Titulo}%", DbType.String);
            }

            if(string.IsNullOrEmpty(texto))
            {
                if (!string.IsNullOrWhiteSpace(filtros.Autor))
                {
                    filtrarPor += " and qra.Autor like @autor";
                    parametrosQuery.Add("autor", $"%{filtros.Autor}%", DbType.String);
                }

                if (!string.IsNullOrWhiteSpace(filtros.Genero))
                {
                    filtrarPor += " and qra.Genero like @genero";
                    parametrosQuery.Add("genero", $"%{filtros.Genero}%", DbType.String);
                }

                if (!string.IsNullOrWhiteSpace(filtros.Editora))
                {
                    filtrarPor += " and qra.Editora like @editora";
                    parametrosQuery.Add("editora", $"%{filtros.Editora}%", DbType.String);
                }

                if (filtros.PublicacaoAno>0)
                {
                    filtrarPor += " and qra.PublicacaoAno = @publicacaoAno";
                    parametrosQuery.Add("publicacaoAno", filtros.PublicacaoAno, DbType.Int32);
                }
            }

            var result = await QueryPagination(parametrosQuery, filtrarPor, texto, pageSize, currentPage);

            if (result == null || (result!=null && result.Total==0))
                throw new NaoEncontradoException($"Nenhum resultado da pesquisa encontrado.");

            return result;
        }
    }
}