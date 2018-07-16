using MediatR;
using Microsoft.Extensions.Logging;
using SGL.Application.Livros.IntegrationEvents;
using SGL.Application.Livros.IntegrationEvents.Events;
using System;
using System.Threading.Tasks;

namespace SGL.API.IntegrationEvents.EventHandling
{
    public class EnviarPedidoInutilizarLivroIntegrationEventHandler //: IIntegrationEventHandler<EnviarPedidoInutilizarLivroIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILoggerFactory _logger;
        private readonly ILivroIntegrationEventService LivroIntegrationEventService;

        public EnviarPedidoInutilizarLivroIntegrationEventHandler(IMediator mediator,
            ILoggerFactory logger, ILivroIntegrationEventService orderingIntegrationEventService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            LivroIntegrationEventService = (LivroIntegrationEventService)orderingIntegrationEventService ?? throw new ArgumentNullException(nameof(orderingIntegrationEventService));
        }

        /// <summary>
        /// Integration event handler which starts the create order process
        /// </summary>
        /// <param name="eventMsg">
        /// Integration event message which is sent by the
        /// basket.api once it has successfully process the 
        /// order items.
        /// </param>
        /// <returns></returns>
        public async Task Handle(EnviarPedidoInutilizarLivroIntegrationEvent eventMsg)
        {
            var result = await Task.FromResult( false);

            // Send Integration event to clean basket once basket is converted to Order and before starting with the order creation process


            /*
            var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(eventMsg.UserId);

            await _orderingIntegrationEventService.PublishThroughEventBusAsync(orderStartedIntegrationEvent);

            if (eventMsg.RequestId != Guid.Empty)
            {
                var createOrderCommand = new CreateOrderCommand(eventMsg.Basket.Items, eventMsg.UserId, eventMsg.UserName, eventMsg.City, eventMsg.Street,
                    eventMsg.State, eventMsg.Country, eventMsg.ZipCode,
                    eventMsg.CardNumber, eventMsg.CardHolderName, eventMsg.CardExpiration,
                    eventMsg.CardSecurityNumber, eventMsg.CardTypeId);

                var requestCreateOrder = new IdentifiedCommand<CreateOrderCommand, bool>(createOrderCommand, eventMsg.RequestId);
                result = await _mediator.Send(requestCreateOrder);
            }*/

            _logger.CreateLogger(nameof(EnviarPedidoInutilizarLivroIntegrationEvent))
                .LogTrace(result ? $"UserCheckoutAccepted integration event has been received and a create new order process is started with requestId: {eventMsg.RequestId}" :
                    $"UserCheckoutAccepted integration event has been received but a new order process has failed with requestId: {eventMsg.RequestId}");
        }
    }
}
