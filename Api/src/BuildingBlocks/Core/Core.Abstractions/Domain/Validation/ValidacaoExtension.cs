using Core.Abstractions.Domain.Enums;
using Core.Abstractions.Domain.Mensagens;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Abstractions.Domain.Validation.Extensions
{
    public static class ValidacaoExtension
    {
        public static IList<Validacao> GetValidacoes<T>(this T item)
        {
            var validacoes = new List<Validacao>();
            
            var propBaseIgnoradas = new List<string>() { "OptionsAttribute", "RelatorioListagemAttribute", "PropriedadeInfoAttribute", "KeyAttribute", "EntityPaiAttribute", "TIdAttribute", "JsonIgnoreAttribute" };
            
            foreach (var property in typeof(T).GetProperties())
            {
                //var attributes = System.Attribute.GetCustomAttributes(property).ToList().Where(b => b.GetType().Name != "RelatorioListagemAttribute").ToArray();
                var attributes = System.Attribute.GetCustomAttributes(property).ToList().Where(b => !propBaseIgnoradas.Contains(b.GetType().Name)).ToArray();

                var customAttributes = property.CustomAttributes;

                var tipoTitulo = property.PropertyType.Name;

                var index = 0;
                var msg = string.Empty;

                //foreach (var attribute in customAttributes.Where(a=> a.AttributeType.Name != "RelatorioListagemAttribute" && a.AttributeType.Name != "PropriedadeInfoAttribute" && a.GetType() != typeof(EntityPaiAttribute) && a.GetType() != typeof(TIdAttribute)))
                    foreach (var attribute in customAttributes.Where(a => !propBaseIgnoradas.Contains(a.AttributeType.Name)))
                    {
                    var validationAttribute = ((ValidationAttribute)attributes[index]);

                    var validacao = new Validacao()
                    {
                        TituloPropriedade = property.Name,
                        Acao = Acao.Atributo,
                        Valor = property.GetValue(item),
                        ValidationAttribute = validationAttribute
                    };

                    switch (attribute.AttributeType.Name)
                    {
                        case "RequiredAttribute":
                        case "ObrigatorioAttribute":
                        case "LoginAttribute":
                        case "SenhaAttribute":
                            validacao.Tipo = TipoValidacao.Requerido;
                            break;

                        case "MaxLengthAttribute":
                        case "MaximoAttribute":
                            validacao.Tipo = TipoValidacao.Maximo;
                            break;
                        case "MinimoAttribute":
                            validacao.Tipo = TipoValidacao.Minimo;
                            break;
                        case "CpfAttribute":
                        case "CnpjAttribute":
                        case "CnpjCpfAttribute":
                        case "EmailAttribute":
                        case "HoraMinutoAttribute":
                        case "IeAttribute":
                        case "TelefoneAttribute":
                        case "CoordenadaAttribute":
                            validacao.Tipo = TipoValidacao.Formato;
                            break;
                    }

                    index++;

                    switch (tipoTitulo.ToLower())
                    {
                        case "string":
                            validacao.TipoCampo = TipoCampo.Texto;
                            break;
                        case "int":
                            validacao.TipoCampo = TipoCampo.NumeroInteiro;
                            break;
                        case "uint":
                        case "uint64":
                        case "uint32":
                        case "ulong":
                            validacao.TipoCampo = TipoCampo.NumeroInteiroPositivo;
                            break;
                        case "bool":
                            validacao.TipoCampo = TipoCampo.Booleano;
                            break;
                    }

                    validacao.Mensagem = new BaseMensagem { Texto = validationAttribute.ErrorMessage, Chave = property.Name + validacao.Tipo.ToString() };
                    validacoes.Add(validacao);
                }
            }

            return validacoes;
        }

        public static IList<Validacao> Merge(this IList<Validacao> value, IList<Validacao> validacoes)
        {
            if (validacoes != null && validacoes.Count > 0)
            {
                validacoes.ToList().ForEach(c =>
                {
                    if (c != null)
                    {
                        value.Add(c);
                    }
                });
            }

            return value;
        }
    }
}