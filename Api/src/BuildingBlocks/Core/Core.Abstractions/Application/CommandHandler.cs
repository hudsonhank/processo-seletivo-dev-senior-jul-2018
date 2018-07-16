using Core.Abstractions.Domain.Mensagens;
using Core.Abstractions.Infrastructure.Data;
using Core.Abstractions.Types.Exception;
using System;
using System.Threading.Tasks;

namespace Core.Abstractions.Application
{
    public class CommandHandler
    {
        
        public readonly IUnitOfWork UnitOfWork;
        
        public CommandHandler(/*IMediator mediator,*/ IUnitOfWork unitOfWork)
        {            
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        //public virtual IMensagem Mensagem(Exception exc)
        //{
        //    return new BaseMensagem { Tipo = TipoMensagemEnum.Erro, Texto = (exc.Message + "\n" + (exc.InnerException != null ? exc.InnerException.Message : string.Empty)) };
        //}

        //protected CommandResult Execute(Func<Task<object>> action)
        //{
        //    return ExecuteAsync(action).Result;
        //}

        //protected async Task<CommandResult> ExecuteAsync(Func<Task<object>> action)
        //{
        //    try
        //    {
        //        UnitOfWork.BeginTransaction();
        //        var result = await action();
        //        UnitOfWork.Commit();
        //        return result as CommandResult;
        //    }
        //    catch (RequisicaoInvalidaException exc)
        //    {
        //        UnitOfWork.Rollback();
        //        throw exc;
        //    }
        //    catch (System.Exception exc)
        //    {
        //        UnitOfWork.Rollback();
        //        Mensagem(exc);
        //        throw exc;
        //    }
        //}

    }
}
