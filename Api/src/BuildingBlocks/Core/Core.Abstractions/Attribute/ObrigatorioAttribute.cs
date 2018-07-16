using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Abstractions.Attribute
{
    /// <summary>
    /// Valida se valor do item obrigatório foi informado.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ObrigatorioAttribute : ValidationAttribute
    {
        //public const string FORMATO_VALIDACAO_STRING = "formato_validacao_string";

        /// <summary>
        /// Tipo de validação (texto, numérica ou coleção) a ser realizada no item obrigatório.
        /// </summary>
        public TipoValidacaoObrig TipoValidacao { get; set; }

        /// <summary>
        /// Titulo do Campo formatado (como deve ser exibido em tela, por ex.)
        /// </summary>
        public string TituloCampo { get; set; }
        
		public ObrigatorioAttribute()
        {
            TipoValidacao = TipoValidacaoObrig.NaoInformado;
            TituloCampo = "";
            ErrorMessage = "";

        }

        public ObrigatorioAttribute(string mensagem)
        {
            TipoValidacao = TipoValidacaoObrig.NaoInformado;
            TituloCampo = "";
            ErrorMessage = mensagem;
        }

        public ObrigatorioAttribute(TipoValidacaoObrig tipo)
        {
            TipoValidacao = tipo;
        }

        public ObrigatorioAttribute(TipoValidacaoObrig tipo, string campo)
        {
            TipoValidacao = tipo;
            TituloCampo = campo;
        }

        public override string FormatErrorMessage(string name)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                string nameFormatted;

                if (TipoValidacao == TipoValidacaoObrig.ColecaoComItens)
                    nameFormatted = string.Format("É obrigatório adicionar ao menos um(a) {0}.", TituloCampo);
                else
                {
                    nameFormatted = string.Format("O campo {0} é obrigatório.",
                        (!string.IsNullOrWhiteSpace(TituloCampo))
                            ? TituloCampo
                            : string.Join("", name.Select(x => (Char.IsUpper(x) ? " " + x : x.ToString()))));
                }

                ErrorMessage = nameFormatted;
            }
            return ErrorMessage;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            switch (TipoValidacao)
            {
                case TipoValidacaoObrig.NaoInformado:
                case TipoValidacaoObrig.String:
                    var valor = Convert.ToString(value);

                    return !string.IsNullOrWhiteSpace(valor);

                case TipoValidacaoObrig.NumeroZeroOuPositivo:
                    var valorNumerico = Convert.ToDouble(value);

                    return valorNumerico >= 0;

                case TipoValidacaoObrig.NumeroPositivo:
                    var valorNumericoPositivo = Convert.ToDouble(value);

                    return valorNumericoPositivo > 0;

                case TipoValidacaoObrig.ColecaoComItens:
                    var colecao = value as IList;

                    if (colecao != null)
                    {
                        return colecao.Count > 0;
                    }
                    return false;
            }

            return true;
        }
    }

    public enum TipoValidacaoObrig
    {
        NaoInformado,
        String,
        NumeroZeroOuPositivo,
        NumeroPositivo,
        ColecaoComItens
    }
}