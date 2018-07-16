using Core.Abstractions.Domain.Validation;
using Core.Abstractions.Domain.Validation.Interfaces;
using Core.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions.Domain
{
    public interface IBaseService<TEntity> where TEntity : Entity
    {
        Task<TEntity> Criar(TEntity model, CancellationToken cancellationToken = default(CancellationToken));
        Task<TEntity> Editar(TEntity model);
        Task Excluir(long id);
        Task Excluir(Expression<Func<TEntity, bool>> condition);
        Task<TEntity> BuscarPorId(long id, bool includes = true);
        Task<TEntity> Buscar(Expression<Func<TEntity, bool>> filter = null, bool includes = true);

        QueryResult<TEntity> BuscarLista(Expression<Func<TEntity, bool>> filter = null, int pageSize = 0, int currentPage = 0, string order = "", bool orderByDescending = false);
        QueryResult<TEntity> BuscarListaReport(Expression<Func<TEntity, bool>> filter = null, int pageSize = 0, int currentPage = 0, string order = "", bool orderByDescending = false);

        DateTime BuscarDatabaseDateTime();

        IList<Validacao> ValidacoesInvalidas { get; }
        IValidacao<TEntity> GetValidacao();
    }
}
