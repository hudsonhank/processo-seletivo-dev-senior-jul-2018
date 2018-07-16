using Core.Abstractions.Domain;
using Core.Abstractions.Infrastructure.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions.Infrastructure
{
    public interface IBaseRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> Criar(TEntity model, CancellationToken cancellationToken = default(CancellationToken));
        Task<TEntity> Editar(TEntity model);
        Task Excluir(long id);
        Task Excluir(Expression<Func<TEntity, bool>> condition);
        Task<TEntity> BuscarPorId(long id, bool includes = true);
        Task<TEntity> Buscar(Expression<Func<TEntity, bool>> filter = null, bool includes = true);
        IQueryable<TEntity> Filtrar();
        QueryResult<TEntity> BuscarLista(Expression<Func<TEntity, bool>> filter = null, int pageSize = 0, int currentPage = 0, string order = "", bool orderByDescending = false, bool includes = true);
        QueryResult<TEntity> BuscarListaReport(Expression<Func<TEntity, bool>> filter = null, int pageSize = 0, int currentPage = 0, string order = "", bool orderByDescending = false);
        
        DateTime BuscarDatabaseDateTime();
        int Count(Expression<Func<TEntity, bool>> filter = null, int pageSize = 0, int currentPage = 0);
    }
}
