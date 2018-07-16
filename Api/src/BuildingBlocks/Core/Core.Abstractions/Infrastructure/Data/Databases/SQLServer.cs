using Core.Abstractions.Infrastructure.Data.Databases;
using Core.Abstractions.Infrastructure.Data.Enums;

namespace Core.Abstractions.Infrastructure.Databases
{
    public class SQLServer : DatabaseBase
    {
        public SQLServer(DatabaseVersionEnum databaseVersion) : base(databaseVersion)
        {
            DatabaseName = DatabaseEnum.SQLServer;
            DatabaseVersion = databaseVersion;
        }
    }
}
