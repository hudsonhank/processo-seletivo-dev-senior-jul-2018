using Core.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions.Infrastructure.Data
{
    public interface IDatabaseContext
    {
        IDatabase Database { get; }
        void BeginTransaction();
        Task Commit(CancellationToken cancellationToken = default(CancellationToken));
        void Rollback();        
        //
        Task<T> Criar<T>(T entity, CancellationToken cancellationToken = default(CancellationToken)) where T : Entity;
        Task<T> Editar<T>(T entity) where T : Entity;
        Task Excluir<T>(long id) where T : Entity;
        Task Excluir<T>(Expression<Func<T, bool>> condition = null) where T : Entity;
        //
        Task<T> BuscarPorId<T>(long id, bool includes = true) where T : Entity;
        Task<T> Buscar<T>(Expression<Func<T, bool>> filter = null, bool includes = true) where T : Entity;

        IEnumerable<T> BuscarLista<T>(Expression<Func<T, bool>> filter = null, int pageSize = 0, int currentPage = 0, bool includes = true) where T : Entity;
        //
        IQueryable<T> Query<T>() where T : Entity;
        //
        DateTime BuscarDatabaseDateTime();

        void Dispose();
    }    
}