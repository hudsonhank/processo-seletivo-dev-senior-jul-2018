using System.ComponentModel;

namespace Core.Abstractions.Domain.Enums
{
    public enum TipoCampo
    {
        [Description("")]
        Null = 0,

        [Description(nameof(Texto))]
        Texto,

        [Description(nameof(NumeroInteiro))]
        NumeroInteiro,

        [Description(nameof(NumeroInteiroPositivo))]
        NumeroInteiroPositivo,

        [Description(nameof(NumeroDecimal))]
        NumeroDecimal,

        [Description(nameof(NumeroDecimalPositivo))]
        NumeroDecimalPositivo,

        [Description(nameof(Booleano))]
        Booleano,

        [Description(nameof(Lista))]
        Lista
    }
}