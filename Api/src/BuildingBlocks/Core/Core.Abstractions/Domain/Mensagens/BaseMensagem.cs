using Core.Abstractions.Extension;

namespace Core.Abstractions.Domain.Mensagens
{
    public class BaseMensagem : IMensagem
    {
        public BaseMensagem(string mensagem, TipoMensagemEnum tipoMensagem = TipoMensagemEnum.Alerta)
        {
            Texto = mensagem;
        }

        public BaseMensagem(string chave, string mensagem, TipoMensagemEnum tipoMensagem = TipoMensagemEnum.Alerta)
        {
            Chave = chave;
            Texto = mensagem;
        }

        public BaseMensagem()
        {            
        }

        public BaseMensagem(string mensagem) : base()
        {
            Texto = mensagem;
        }

        public static BaseMensagem Personalizada(string campoValor, int tipo = 0, int quantidade = 0, TipoMensagemEnum tipoMensagem = TipoMensagemEnum.Alerta)
        {
            switch (tipo)
            {
                case 1:
                    return new BaseMensagem($"É obrigatório informar {campoValor}.");
                case 2:
                    return new BaseMensagem($"Informe no mínimo {quantidade} caracteres em {campoValor}.");
                case 3:
                    return new BaseMensagem($"Informe no máximo {quantidade} caracteres em {campoValor}.");
                default:
                    return new BaseMensagem(campoValor, tipoMensagem);
            }
        }

        public string Chave { get; set; }
        public string Texto { get; set; }
        public TipoMensagemEnum Tipo { get; set; }

        public string TipoTexto { get { return Tipo.GetDescription(); } }
    }
}