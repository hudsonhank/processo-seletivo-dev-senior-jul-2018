using Core.Abstractions.Domain.Enums;
using Core.Abstractions.Domain.Mensagens;

namespace Core.Abstractions.Types.Formulario
{
    public class FormularioValidacao
    {
        public IMensagem Mensagem { get; set; }
        public string NomePropriedade { get; set; }
        public TipoValidacao Tipo { get; set; }
        public TipoCampo TipoCampo { get; set; }
        public string Formato { get; set; }

        public FormularioValidacao()
        {
        }

        public FormularioValidacao(TipoValidacao tipoValidacao, string nomePropriedade, IMensagem mensagem)
        {
            Tipo = tipoValidacao;
            NomePropriedade = nomePropriedade;
            Mensagem = mensagem;
        }

        public FormularioValidacao(TipoValidacao tipoValidacao, TipoCampo tipoCampo, IMensagem mensagem = null, string nomePropriedade = "", string formato = "")
        {
            Tipo = tipoValidacao;
            TipoCampo = tipoCampo;
            NomePropriedade = nomePropriedade;
            Mensagem = mensagem;
            Formato = formato;
        }
    }
}
