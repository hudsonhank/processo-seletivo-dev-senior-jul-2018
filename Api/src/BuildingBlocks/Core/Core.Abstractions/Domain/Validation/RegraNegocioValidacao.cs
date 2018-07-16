using Core.Abstractions.Domain.Enums;
using Core.Abstractions.Domain.Mensagens;

namespace Core.Abstractions.Domain.Validation
{
    public class RegraNegocioValidacao<T> : Validacao
    {
        public RegraNegocioValidacao(bool condicao, IMensagem mensagem) 
            : base(
                  TipoValidacao.RegraNegocio, 
                  null,
                  null,
                  condicao,
                  mensagem)
        {
        }
    }
}