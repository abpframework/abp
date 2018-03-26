using MongoDB.Driver;

namespace Volo.Abp.Uow.MongoDB
{
    public class MongoDbDatabaseApi : IDatabaseApi
    {
        public IMongoDatabase Database { get; }

        public MongoDbDatabaseApi(IMongoDatabase database)
        {
            Database = database;
        }
    }
}
