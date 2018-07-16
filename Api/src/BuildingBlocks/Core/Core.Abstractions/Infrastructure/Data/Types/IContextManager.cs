namespace Core.Abstractions.Infrastructure.Data
{
    public interface IContextManager<TEntityKey, TContext>
        where TEntityKey : struct
    
        where TContext : IDatabaseContext, new()
    {
        IDatabaseContext GetContext();
        void OnBeginRequest();
        void OnEndRequest();
    }
}
