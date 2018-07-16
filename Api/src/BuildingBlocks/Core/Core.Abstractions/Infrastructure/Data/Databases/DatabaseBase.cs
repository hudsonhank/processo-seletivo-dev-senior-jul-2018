using Core.Abstractions.Infrastructure.Data.Enums;

namespace Core.Abstractions.Infrastructure.Data.Databases
{
    public class DatabaseBase : IDatabase
    {
        public DatabaseEnum DatabaseName { get; set; }
        public DatabaseVersionEnum DatabaseVersion { get; set; }
        //private DatabaseEnum databaseName;
        //public DatabaseEnum DatabaseName
        //{
        //    get { return databaseName; }
        //    protected set { databaseName = value; }
        //}

        //private DatabaseVersionEnum databaseVersion;
        //public DatabaseVersionEnum DatabaseVersion
        //{
        //    get { return databaseVersion; }
        //    protected set { databaseVersion = value; }
        //}

        public DatabaseBase(DatabaseVersionEnum databaseVersion)
        {
            DatabaseVersion = databaseVersion;
        }
    }
}
