namespace Volo.Abp.Uow.MongoDB
{
    public class MongoDbDatabaseApi<TMongoDbContext> : IDatabaseApi
    {
        public TMongoDbContext DbContext { get; }

        public MongoDbDatabaseApi(TMongoDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
