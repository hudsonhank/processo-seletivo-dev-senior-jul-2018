using Core.Abstractions.Attribute.Enum;
using Core.Abstractions.Extension;
using System.Collections.Generic;

namespace Core.Abstractions.Types.Formulario
{
    public class FormularioGrupo : IControle
    {
        public virtual string Key { get; set; }
        public virtual object Valor { get; set; }
        public virtual string GrupoName { get; set; } = string.Empty;
        public virtual string Label { get; set; } = "Grupo de controle";
        public virtual string Class { get; set; } = "panel box box-success";
        public virtual int Order { get; set; } = 1;
        public virtual ControleTypeEnum Tipo { get; } = ControleTypeEnum.Grupo;
        public virtual IList<FormularioValidacao> Validacoes { get; set; }
        public virtual Dictionary<string, List<ItemEnum>> ListaValores { get; set; }

        public virtual List<IControle> Controles { get; set; }
        public string ControleTipo { get; set; } = ComponenteTipoEnum.Formulario.GetDescription();

        public FormularioGrupo(string key, string controleTipo = "", int order = 1)
        {
            Key = key;
            ControleTipo = !string.IsNullOrEmpty(controleTipo) ? controleTipo : ComponenteTipoEnum.Formulario.GetDescription();
            Order = order;
            Tipo = ControleTypeEnum.Grupo;
            Controles = new List<IControle>();
            Validacoes = new List<FormularioValidacao>();
            ListaValores = new Dictionary<string, List<ItemEnum>>();
        }
    }
}
