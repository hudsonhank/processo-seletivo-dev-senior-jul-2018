using Core.Abstractions.Infrastructure.Data.Databases;
using Core.Abstractions.Infrastructure.Data.Enums;

namespace Core.Abstractions.Infrastructure.Databases
{
    public class Oracle : DatabaseBase
    {
        public Oracle(DatabaseVersionEnum databaseVersion) : base(databaseVersion)
        {
            DatabaseName = DatabaseEnum.Oracle;
            DatabaseVersion = databaseVersion;
        }
    }
}
