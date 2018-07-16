using System.ComponentModel;

namespace Core.Abstractions.Domain.Mensagens
{
    public enum TipoMensagemEnum
    {
        [Description(nameof(Alerta))]
        Alerta = 1,
        [Description(nameof(Erro))]
        Erro,
        [Description(nameof(Informacao))]
        Informacao,
        [Description(nameof(Sucesso))]
        Sucesso
    }
}