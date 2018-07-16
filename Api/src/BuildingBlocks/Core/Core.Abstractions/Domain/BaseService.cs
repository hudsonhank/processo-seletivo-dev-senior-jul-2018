using Core.Abstractions.Application;
using Core.Abstractions.Domain.Mensagens;
using Core.Abstractions.Domain.Validation;
using Core.Abstractions.Domain.Validation.Interfaces;
using Core.Abstractions.Infrastructure;
using Core.Abstractions.Infrastructure.Data;
using Core.Abstractions.Types.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions.Domain
{
    public class BaseService<TEntity> : IDisposable, IBaseService<TEntity>        
        where TEntity : Entity        
    {
        public IBaseRepository<TEntity> Repository;
        public IValidacao<TEntity> Validacao;
        public IList<Validacao> ValidacoesInvalidas
        {
            get
            {
                if (Validacao != null)
                {
                    return Validacao.Invalidos;
                }
                return null;
            }
        }

        public IUnitOfWork UnitOfWork { get; }

        public bool InMemory { get; }

        public IValidacao<TEntity> GetValidacao()
        {
            return Validacao;
        }

        public BaseService()
        {

        }

        public BaseService(IBaseRepository<TEntity> repository, IValidacao<TEntity> validacao, IUnitOfWork unitOfWork, bool inMemory = false)
        {
            Repository = repository;
            Validacao = validacao;
            UnitOfWork = unitOfWork;
            InMemory = inMemory;
        }
        public virtual IMensagem Mensagem(Exception exc)
        {
            return new BaseMensagem { Tipo = TipoMensagemEnum.Erro, Texto = (exc.Message + "\n" + (exc.InnerException != null ? exc.InnerException.Message : string.Empty)) };
        }

        protected object Execute(Func<Task<object>> action)
        {
            return ExecuteAsync(action).Result;
        }

        protected async Task<object> ExecuteAsync(Func<Task<object>> action)
        {
            try
            {
                object result;
                if (!InMemory)
                {
                    UnitOfWork.BeginTransaction();
                    result = await action();
                    UnitOfWork.Commit();                    
                }
                else
                {
                    result = await action();
                }                
                return result;
            }
            catch (RequisicaoInvalidaException exc)
            {
                if (!InMemory)
                {
                    UnitOfWork.Rollback();
                }
                throw exc;
            }
            catch (Exception exc)
            {
                UnitOfWork.Rollback();
                Mensagem(exc);
                throw exc;
            }
        }



        public virtual async Task<TEntity> Criar(TEntity model, CancellationToken cancellationToken = default(CancellationToken))
        {
            Validacao.ValidarCriar(model);
            Validacao.EncerrarSeInvalido();            
            return await Repository.Criar(model);
        }

        public virtual async Task<TEntity> Editar(TEntity model)
        {
            Validacao.ValidarEditar(model);
            Validacao.EncerrarSeInvalido();
            return await Repository.Editar(model);
        }

        public virtual async Task Excluir(long id )
        {
            await Repository.Excluir(id);
        }

        public virtual async Task Excluir(Expression<Func<TEntity, bool>> condition)
        {
            if (condition == null)
            {
                throw new System.Exception("Não é possivel Excluir sem condição.");
            }
            var itens = Repository.Filtrar().Where(condition).ToList();
            if (itens != null)
            {
                foreach (var item in itens)
                {
                    if (item != null)
                    {
                        await Repository.Excluir(item.Id);
                    }
                }
            }
        }

        public virtual async Task<TEntity> BuscarPorId(long id, bool includes = true)
        {
            return await Repository.BuscarPorId(id, includes);
        }

        public virtual async Task<TEntity> Buscar(Expression<Func<TEntity, bool>> filter = null, bool includes = true)
        {
            return await Repository.Buscar(filter, includes);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> filter = null, int pageSize = 0, int currentPage = 0)
        {
            return Repository.Count(filter, pageSize, currentPage);
        }

        public virtual QueryResult<TEntity> BuscarLista(Expression<Func<TEntity, bool>> filter = null, int pageSize = 0, int currentPage = 0, string order = "", bool orderByDescending = false)
        {
            Validacao.ValidarListar(filter);
            Validacao.EncerrarSeInvalido();
            return Repository.BuscarLista(filter, pageSize, currentPage, order, orderByDescending);
        }

        public virtual QueryResult<TEntity> BuscarListaReport(Expression<Func<TEntity, bool>> filter = null, int pageSize = 0, int currentPage = 0, string order = "", bool orderByDescending = false)
        {
            return Repository.BuscarListaReport(filter, pageSize, currentPage, order, orderByDescending);
        }
        
        public virtual void Dispose()
        {
        }

        public virtual DateTime BuscarDatabaseDateTime()
        {
            return Repository.BuscarDatabaseDateTime();
        }
    }
    
}
