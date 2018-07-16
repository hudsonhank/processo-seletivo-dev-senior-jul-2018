using Core.Abstractions.Attribute.Enum;
using Core.Abstractions.Extension;
using System.Collections.Generic;

namespace Core.Abstractions.Types.Formulario
{
    public class FormularioArray : IControle
    {
        public virtual string Key { get; set; }
        public virtual object Valor { get; set; } = string.Empty;
        public virtual string Default { get; set; } = string.Empty;
        public virtual string GrupoName { get; set; } = string.Empty;
        public virtual string Label { get; set; } = "Array de controle";
        public virtual string Class { get; set; } = "panel box box-primary";
        public virtual int Order { get; set; } = 1;
        public virtual ControleTypeEnum Tipo { get; } = ControleTypeEnum.Array;
        public string ControleTipo { get; } = ComponenteTipoEnum.TextBox.GetDescription();
        public virtual IList<FormularioValidacao> Validacoes { get; set; }

        public virtual List<IControle> Controles { get; set; }
        public virtual IControle ControlType { get; set; }

        public FormularioArray(string key, string controleTipo, int order = 1, IControle controlType = null)
        {
            Key = key;
            Order = order;
            ControleTipo = controleTipo;
            ControlType = controlType;
            Tipo = ControleTypeEnum.Array;
            Controles = new List<IControle>();
            Validacoes = new List<FormularioValidacao>();
        }

    }
}
