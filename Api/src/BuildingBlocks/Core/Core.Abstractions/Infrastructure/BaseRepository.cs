using Core.Abstractions.Domain;
using Core.Abstractions.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions.Infrastructure
{
    public class BaseRepository<TEntity> : IDisposable, IBaseRepository<TEntity> where TEntity : Entity
    {
        public IDatabaseContext Context;

        public BaseRepository()
        {
        }

        public BaseRepository(IDatabaseContext context)
        {
            Context = context;
        }

        public virtual void Dispose() { }

        public virtual async Task<TEntity> Criar(TEntity model, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Context.Criar(model, cancellationToken);            
        }

        public virtual async Task<TEntity> Editar(TEntity model)
        {            
            return await Context.Editar(model);
        }
        
        public virtual async Task Excluir(long id)
        {
            await Context.Excluir<TEntity>(id);
        }

        public virtual async Task Excluir(Expression<Func<TEntity, bool>> condition = null)
        {
            await Context.Excluir(condition);
        }

        public virtual async Task<TEntity> BuscarPorId(long id, bool includes = true)
        {
            return await Context.BuscarPorId<TEntity>(id, includes);
        }

        public virtual async Task<TEntity> Buscar(Expression<Func<TEntity, bool>> filter = null, bool includes = true)
        {            
            return await Context.Buscar(filter, includes);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> filter = null, int pageSize = 10, int currentPage = 1)
        {
            var query = Filtrar();
            var research = new QueryResult<TEntity> { CurrentPage = currentPage, PageSize = pageSize, Resultado = new List<TEntity>() };

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return (pageSize > 0 && currentPage > 0) ? query.Skip((currentPage - 1) * pageSize).Take(pageSize).Count() : query.Count();
        }

        public virtual QueryResult<TEntity> BuscarLista(Expression<Func<TEntity, bool>> filter = null, int pageSize = 0, int currentPage = 0, string order = "", bool orderByDescending = false, bool includes = true)
        {
            var query = Context.BuscarLista(filter, pageSize, currentPage, includes);

            var research = new QueryResult<TEntity> { CurrentPage = currentPage, PageSize = pageSize, Resultado = new List<TEntity>() };

            if (!string.IsNullOrEmpty(order))
            {
                if (orderByDescending)
                {
                    query = query.Select(x=>x).OrderByDescending(c => order);
                }
                else
                {
                    query = query.Select(x => x).OrderBy(c => order);
                    //query = query.OrderBy(c => order);
                }
            }

            research.Total = query.Count();
            research.Resultado = (pageSize > 0 && currentPage > 0) ? query
                .Skip((currentPage - 1) * pageSize).Take(pageSize).ToList() : query.ToList();

            return research;
        }

        public virtual QueryResult<TEntity> BuscarListaReport(Expression<Func<TEntity, bool>> filter = null, int pageSize = 0, int currentPage = 0, string order = "", bool orderByDescending = false)
        {
            var query = Filtrar();
            var research = new QueryResult<TEntity> { CurrentPage = currentPage, PageSize = pageSize, Resultado = new List<TEntity>() };

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(order))
            {
                if (orderByDescending)
                {
                    query = query.OrderByDescending(c => order);
                }
                else
                {
                    query = query.OrderBy(c => order);
                }
            }

            research.Total = query.Count();
            research.Resultado = (pageSize > 0 && currentPage > 0) ? query.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList() : query.ToList();

            return research;
        }

        public virtual DateTime BuscarDatabaseDateTime()
        {
            return Context.BuscarDatabaseDateTime();
        }    

        public IQueryable<TEntity> Filtrar()
        {
            return Context.Query<TEntity>();
        }


    }
}
