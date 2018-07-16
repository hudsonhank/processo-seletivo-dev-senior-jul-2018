using Core.Abstractions.Domain.Validation;
using System.Collections.Generic;

namespace Core.Abstractions.Types.Exception
{
    public class RequisicaoInvalidaException : System.Exception
    {

        public IList<Validacao> Mensagens { get; private set; }

        public RequisicaoInvalidaException()
        {

        }

        public RequisicaoInvalidaException(string message)
            : base(message)
        {
        }

        public RequisicaoInvalidaException(IList<Validacao> mensagems)
           : base()
        {
            Mensagens = mensagems;
        }
    }
}
