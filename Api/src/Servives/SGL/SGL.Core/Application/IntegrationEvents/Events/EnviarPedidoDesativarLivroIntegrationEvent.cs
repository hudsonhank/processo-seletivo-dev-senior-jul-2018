using System;

namespace SGL.Application.Livros.IntegrationEvents.Events
{
    public class EnviarPedidoInutilizarLivroIntegrationEvent// : IntegrationEvent
    {
        public Guid RequestId { get; set; }
        public virtual string CodigoUnico { get; set; }
        public virtual string MotivoDaInutilizacao { get; set; }

        public EnviarPedidoInutilizarLivroIntegrationEvent(string numeroTombo, string motivoDaInutilizacao, Guid requestId)
        {
            CodigoUnico = numeroTombo;
            RequestId = requestId;
            MotivoDaInutilizacao = motivoDaInutilizacao;
        }
    }
}
