using System.Collections.Generic;
using System.ComponentModel;

namespace Core.Abstractions.Types.Formulario
{
    public interface IControle
    {
        string Key { get; set; }
        object Valor { get; set; }
        string GrupoName { get; set; }
        string Label { get; set; }
        string Class { get; set; }
        int Order { get; set; }
        string ControleTipo { get; }
        ControleTypeEnum Tipo { get; }

        IList<FormularioValidacao> Validacoes { get; set; }
    }

    public enum ControleTypeEnum
    {
        [Description("controle")]
        Controle = 1,
        [Description("grupo")]
        Grupo = 2,
        [Description("array")]
        Array = 3
    }
}
