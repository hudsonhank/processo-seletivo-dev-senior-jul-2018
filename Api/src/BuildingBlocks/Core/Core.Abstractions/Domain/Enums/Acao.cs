using System.ComponentModel;

namespace Core.Abstractions.Domain.Enums
{
    public enum Acao
    {
        [Description("")]
        Null = 0,

        [Description(nameof(Criar))]
        Criar,

        [Description(nameof(Editar))]
        Editar,

        [Description(nameof(Desabilitar))]
        Desabilitar,

        [Description(nameof(Visualizar))]
        Visualizar,

        [Description(nameof(Listar))]
        Listar,

        [Description(nameof(Atributo))]
        Atributo
    }
}