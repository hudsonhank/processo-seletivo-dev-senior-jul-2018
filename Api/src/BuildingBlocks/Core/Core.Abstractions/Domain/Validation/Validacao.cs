using Core.Abstractions.Domain.Enums;
using Core.Abstractions.Domain.Mensagens;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.Domain.Validation
{
    public class Validacao
    {
        public IMensagem Mensagem { get; set; }
        public string TituloPropriedade { get; set; }
        public TipoValidacao Tipo { get; set; }
        public TipoCampo TipoCampo { get; set; }
        public Acao Acao { get; set; }

        public object Valor { get; set; }
        public string Formato { get; set; }
        public ValidationAttribute ValidationAttribute { get; set; }

        public Validacao()
        {

        }

        public Validacao(TipoValidacao tipoValidacao, ValidationAttribute validationAttribute, string nomePropriedade, object valor, IMensagem mensagem)
        {
            Tipo = tipoValidacao;
            ValidationAttribute = validationAttribute;
            TituloPropriedade = nomePropriedade;
            Valor = valor;
            Mensagem = mensagem;
        }

        public Validacao(TipoValidacao tipoValidacao, TipoCampo tipoCampo, object valor = null, IMensagem mensagem = null, string nomePropriedade = "", string formato = "", ValidationAttribute validationAttribute = null)
        {
            Tipo = tipoValidacao;
            TipoCampo = tipoCampo;
            TituloPropriedade = nomePropriedade;
            Mensagem = mensagem;
            Valor = valor;
            Formato = formato;
            ValidationAttribute = validationAttribute;
        }
        
        public bool IsValido()
        {
            if (ValidationAttribute != null)
            {
                return ValidationAttribute.IsValid(Valor);
            }

            switch (Tipo)
            {
                #region Booleano

                case TipoValidacao.Booleano:
                case TipoValidacao.RegraNegocio:
                    return !Convert.ToBoolean(Valor);

                #endregion

                #region Requerido
                case TipoValidacao.Requerido:
                    var valorNumerico = decimal.MinValue;
                    var valorNumericoInteiroPositivo = uint.MinValue;
                    switch (TipoCampo)
                    {
                        case TipoCampo.Texto:
                            var texto = Convert.ToString(Valor);
                            return !string.IsNullOrEmpty(texto);

                        case TipoCampo.NumeroInteiroPositivo:
                            try
                            {
                                uint.TryParse(Convert.ToString(Valor), out valorNumericoInteiroPositivo);
                                return valorNumericoInteiroPositivo > 0;
                            }
                            catch
                            {
                                return false;
                            }

                        case TipoCampo.NumeroDecimal:
                            try
                            {
                                return decimal.TryParse(Convert.ToString(Valor), out valorNumerico);
                            }
                            catch
                            {
                                return false;
                            }

                        case TipoCampo.NumeroDecimalPositivo:
                            try
                            {                                
                                decimal.TryParse(Convert.ToString(Valor), out valorNumerico);
                                return valorNumerico > 0;
                            }
                            catch
                            {
                                return false;
                            }

                        case TipoCampo.Lista:
                            return (Valor as IList != null && (Valor as IList).Count > 0);
                    }
                    return false;
                #endregion

                #region ListaComItens
                case TipoValidacao.ListaComItens:
                    return (Valor as IList != null && (Valor as IList).Count > 0);
                #endregion
            }

            return true;
        }
    }
}
