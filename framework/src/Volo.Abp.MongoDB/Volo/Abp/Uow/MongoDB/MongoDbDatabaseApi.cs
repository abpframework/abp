using Volo.Abp.MongoDB;

namespace Volo.Abp.Uow.MongoDB;

public class MongoDbDatabaseApi : IDatabaseApi
{
    public IAbpMongoDbContext DbContext { get; }

    public MongoDbDatabaseApi(IAbpMongoDbContext dbContext)
    {
        DbContext = dbContext;
    }
}
