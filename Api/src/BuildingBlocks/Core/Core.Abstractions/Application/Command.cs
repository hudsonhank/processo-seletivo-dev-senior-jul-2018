using System;

namespace Core.Abstractions.Application
{
    public abstract class Command// : INotification
    {
        //[PropriedadeInfo(ComponenteTipoEnum.Hidden)]        
        public /*IMensagemLivroiner<IMensagem>*/ Type TypeMensagenCommand { get; private set; }

        protected Command(/*IMensagemLivroiner<IMensagem> */ Type mensagemTipo)
        {
            TypeMensagenCommand = mensagemTipo;
        }
    }
}
