using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Volo.Abp.Uow.MongoDB
{
    public class MongoDbDatabaseApi: IDatabaseApi
    {
        public IMongoDatabase Database { get; }

        public MongoDbDatabaseApi(IMongoDatabase database)
        {
            Database = database;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            //TODO: Does MongoDB has such a feature?
            return Task.CompletedTask;
        }
    }
}
