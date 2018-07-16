using System.ComponentModel;

namespace Core.Abstractions.Domain.Enums
{
    public enum TipoValidacao
    {
        [Description("")]
        Null = 0,

        [Description(nameof(Requerido))]
        Requerido,

        [Description(nameof(Maximo))]
        Maximo,

        [Description(nameof(Minimo))]
        Minimo,

        [Description(nameof(Formato))]
        Formato,

        [Description(nameof(ListaComItens))]
        ListaComItens,

        [Description(nameof(Booleano))]
        Booleano,

        [Description(nameof(RegraNegocio))]
        RegraNegocio
    }
}