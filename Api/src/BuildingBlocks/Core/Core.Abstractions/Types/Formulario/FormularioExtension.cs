using Core.Abstractions.Attribute;
using Core.Abstractions.Attribute.Enum;
using Core.Abstractions.Domain.Enums;
using Core.Abstractions.Domain.Mensagens;
using Core.Abstractions.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Core.Abstractions.Types.Formulario
{
    public static class FormularioExtension
    {
        public static FormularioGrupo Formulario<T>(this T item)
        {
            FormularioGrupo formulario = new FormularioGrupo("Principal");
            formulario.Controles = GetControles(typeof(T), formulario);
            return formulario;
        }

        private static List<IControle> GetControles(Type item, FormularioGrupo formGroup)
        {
            FormularioControle input = null;            
            System.Attribute[] attributes;
            System.Attribute[] attributesEspeciais;

            var inputs = new List<IControle>();
            var tipoNome = string.Empty;            
            var tipos = new List<Type>() { typeof(EntityPaiAttribute),  typeof(TIdAttribute) };
            var queroEssestipos = new List<Type>() { typeof(PropriedadeInfoAttribute), typeof(OptionsAttribute) };
            var propBase = new List<string>() { "TId", "Ativo", "DataCadastro", "DataAtualizacao" };
            var msg = string.Empty;

            var json = string.Empty;
            
            foreach (var property in item.GetProperties().Where(x => !propBase.Contains(x.Name)))
            {
                attributes = System.Attribute.GetCustomAttributes(property).ToList().Where(b => !tipos.Contains(b.GetType())).ToArray();

                attributesEspeciais = System.Attribute.GetCustomAttributes(property).ToList().Where(b => queroEssestipos.Contains(b.GetType())).ToArray();

                //var attrs = property.GetCustomAttributes(true);

                tipoNome = property.PropertyType.Name;
                input = new FormularioControle(property.Name) { Label = property.Name };
                
                msg = string.Empty;

                if (ConstruirControles(inputs, input, property, tipos))
                {
                    
                    inputs.Add(input);
                }
            }

            return inputs;
        }
        
        private static bool ConstruirControles(List<IControle> inputs, FormularioControle input, PropertyInfo property, List<Type> tipos)
        {
            var attributes = System.Attribute.GetCustomAttributes(property).ToList().Where(b => !tipos.Contains(b.GetType())).ToArray();

            OptionsAttribute options = (attributes.Where(x => x.GetType() == typeof(OptionsAttribute)).FirstOrDefault() as System.Attribute) as OptionsAttribute;

            var inserir = true;
            var contador = 1;
            var index = 0;

            var lista = property.CustomAttributes.Where(a => !tipos.Contains(a.GetType()) && a.AttributeType.Name != "RelatorioListagemAttribute").ToArray();

            if ((attributes.Where(x => x.GetType() == typeof(PropriedadeInfoAttribute)).FirstOrDefault() as ValidationAttribute) is PropriedadeInfoAttribute info)
            {
                input.ControleTipo = info.Tipo.GetDescription();
                input.Label = info.Label;
                input.PlaceHolder = info.PlaceHolder;
                input.Valor = info.Valor;

                if (options != null)
                {
                    input.Class = options.CSSClass;
                    input.TypeData = options.TypeData;
                    input.GrupoName = options.GrupoName;
                    input.ListaValores = options.ListaValores;
                    input.Pattern = options.Pattern;

                    input.ControleCondicaoKey = options.ControleCondicaoKey;
                    input.ControleCondicaoValor = options.ControleCondicaoValor;                    
                    input.AddLista = options.AddLista;
                    input.AddListaChave = options.AddListaChave;
                    input.Editavel = options.Editavel;
                    input.Listagem = options.Listagem;

                    if (info.Tipo == ComponenteTipoEnum.Entidade)
                    {
                        var entityFilho = options.EntidadeTipo.Assembly.ExportedTypes.Where(x => x.Name == options.EntidadeTipo.Name).SingleOrDefault();
                        var grupo = new FormularioGrupo(property.Name, ComponenteTipoEnum.Entidade.GetDescription(), input.Order)
                        {
                            Label = input.Label
                        };
                        grupo.Controles = GetControles(entityFilho, grupo);
                        grupo.Validacoes = input.Validacoes;
                        inputs.Add(grupo);
                    }

                    if (info.Tipo == ComponenteTipoEnum.ListaEntidade)
                    {
                        var entityFilho = options.EntidadeTipo.Assembly.ExportedTypes.Where(x => x.Name == options.EntidadeTipo.Name).SingleOrDefault();
                        //var model = Activator.CreateInstance(entityFilho);
                        var grupo = new FormularioGrupo(options.EntidadeTipo.Name, ComponenteTipoEnum.ListaEntidade.GetDescription());
                        grupo.Controles = GetControles(entityFilho, grupo);
                        grupo.Label = options.Description;
                        var array = new FormularioArray(property.Name, ComponenteTipoEnum.ListaEntidade.GetDescription(), input.Order, grupo)
                        {
                            Label = input.Label,

                        };

                        var json =string.Empty;
                        
                        foreach (var item in grupo.Controles)
                        {
                            switch (item.Tipo)
                            {
                                case ControleTypeEnum.Controle:
                                    json += $"\"{item.Key}\":\"{item.Valor}\",";
                                    
                                    break;
                                case ControleTypeEnum.Grupo:
                                    json += $"\"{item.Key}\":null,";
                                    
                                    break;
                                case ControleTypeEnum.Array:
                                    json += $"\"{item.Key}\":[],";
                                    break;                                
                            }
                            //json += $"'{item.Key}':'{((item.Tipo == ControleTypeEnum.Array) ? "[]" : "")}',";
                        };

                        ///array.Default=JsonConvert.SerializeObject(model, Formatting.Indented);
                        array.Default =  "{"+ json.Substring(0,json.Length-1) + "}";

                        array.Validacoes = input.Validacoes;

                        if (array.Validacoes != null && array.Validacoes.Count > 0)
                        {
                            array.Validacoes[0].TipoCampo = TipoCampo.Lista;
                        }
                        inputs.Add(array);
                    }

                    if (info.Tipo == ComponenteTipoEnum.ListaValor)
                    {
                        input.ControleTipo = options.TipoLista.GetDescription();
                        input.Key = options.EntidadeTipo.Name;
                        var array = new FormularioArray(property.Name, ComponenteTipoEnum.ListaValor.GetDescription(), input.Order, input)
                        {
                            Label = input.Label
                        };
                        array.Label = options.Description;
                        array.Validacoes = input.Validacoes;
                        if (array.Validacoes != null && array.Validacoes.Count > 0)
                        {
                            array.Validacoes[0].TipoCampo = TipoCampo.Lista;
                        }

                        inputs.Add(array);
                    }
                }
                input.Order = contador++;
                inserir = info.Tipo != ComponenteTipoEnum.ListaEntidade && info.Tipo != ComponenteTipoEnum.ListaValor && info.Tipo != ComponenteTipoEnum.Entidade;
            }

            foreach (var attribute in attributes)
            {
                var attr = ((ValidationAttribute)attributes[index]);
                input.Validacoes.Add(AtribuirValidacoes(attribute, property, attr, input));
                index++;

            }

            return inserir;
        }

        private static FormularioValidacao AtribuirValidacoes(System.Attribute attribute, PropertyInfo property, ValidationAttribute attr, FormularioControle input)
        {
            var validacao = new FormularioValidacao(){ NomePropriedade = property.Name,
                //Valor = property.GetValue(item),
                //Validador = attr
            };

            switch (attribute.GetType().Name)
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
                    validacao.Formato = (attr as MaximoAttribute).Quantidade.ToString();
                    input.Maximo = (attr as MaximoAttribute).Quantidade;
                    break;
                case "MinimoAttribute":
                    validacao.Tipo = TipoValidacao.Minimo;
                    validacao.Formato = (attr as MinimoAttribute).Quantidade.ToString();
                    break;
                
            }

            switch (property.PropertyType.Name.ToLower())
            {
                case "string":
                    validacao.TipoCampo = TipoCampo.Texto;
                    break;
                case "int":
                case "int32":
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

            validacao.Mensagem = new BaseMensagem { Texto = attr.ErrorMessage, Chave = property.Name + validacao.Tipo.ToString() };
            
            return validacao;
        }
    }
}
