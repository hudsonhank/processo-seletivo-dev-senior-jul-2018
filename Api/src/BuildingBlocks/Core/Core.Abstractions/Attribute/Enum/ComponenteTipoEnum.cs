using System.ComponentModel;

namespace Core.Abstractions.Attribute.Enum
{
    public enum ComponenteTipoEnum
    {
        [Description("formulario")]
        Formulario,
        [Description("radiobutton")]
        RadioButton,
        [Description("textbox")]
        TextBox,
        [Description("textarea")]
        TextArea,
        [Description("dropdown")]
        DropDown,
        [Description("checkbox")]
        CheckBox,
        [Description("lista")]
        Lista,
        [Description("entidade")]
        Entidade,
        [Description("listaentidade")]
        ListaEntidade,
        [Description("listavalor")]
        ListaValor,
        [Description("hidden")]
        Hidden,
        [Description("condicionado")]
        Condicionado
    }
}
