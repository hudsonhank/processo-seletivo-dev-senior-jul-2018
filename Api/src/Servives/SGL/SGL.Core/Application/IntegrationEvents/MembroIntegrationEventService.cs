namespace SGL.Application.Livros.IntegrationEvents
{
    public interface ILivroIntegrationEventService
    {
        //Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }

    public class LivroIntegrationEventService : ILivroIntegrationEventService
    {
        //private readonly Func<DbConnection, IIntegrationEventLogService> IntegrationEventLogServiceFactory;
        //private readonly IEventBus _eventBus;
        //private readonly ProjetoContext LivroContext;
        //private readonly IIntegrationEventLogService _eventLogService;

        //public LivroIntegrationEventService(/*IEventBus eventBus,// ProjetoContext FinanceiroContext,
        //Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory*/)
        //{
        //    LivroContext = FinanceiroContext ?? throw new ArgumentNullException(nameof(FinanceiroContext));
        //    IntegrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
        //    _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        //    _eventLogService = _integrationEventLogServiceFactory(ProjetoContext.Database.GetDbConnection());
        //}

        //public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        //{
        //    await SaveEventAndOrderingContextChangesAsync(evt);

        //    _eventBus.Publish(evt);


        //    await _eventLogService.MarkEventAsPublishedAsync(evt);
        //}

        //private async Task SaveEventAndOrderingContextChangesAsync(IntegrationEvent evt)
        //{
        //    var result = await Task.FromResult(false);
        //    //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //    //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
        //    await ResilientTransaction.New(ProjetoContext)
        //        .ExecuteAsync(async () =>
        //        {
        //            // Achieving atomicity between original ordering database operation and the IntegrationEventLog thanks to a local transaction
        //            await ProjetoContext.SaveChangesAsync();
        //            await _eventLogService.SaveEventAsync(evt, ProjetoContext.Database.CurrentTransaction.GetDbTransaction());
        //        });
        //}



    }
}
