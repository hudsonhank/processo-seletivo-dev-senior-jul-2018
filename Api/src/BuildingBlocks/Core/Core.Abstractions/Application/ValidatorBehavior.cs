using Core.Abstractions.Domain.Mensagens;
using Core.Abstractions.Domain.Validation;
using Core.Abstractions.Types.Exception;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Abstractions.Application
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : Command
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validar = new BaseValidacao<TRequest>(request.TypeMensagenCommand as IMensagemLivroiner<IMensagem>);
            if (validar.ValidarAtributos(request))
            {
                throw new RequisicaoInvalidaException(validar.Invalidos);
            }

            var response = await next();
            return response;
        }
    }
}
