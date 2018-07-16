using Core.Abstractions.Infrastructure.Data.Enums;

namespace Core.Abstractions.Infrastructure.Data
{
    public interface IDatabase
    {
        DatabaseEnum DatabaseName { get; }
        DatabaseVersionEnum DatabaseVersion { get; }
    }
}
