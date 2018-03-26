using MongoDB.Driver;

namespace Volo.Abp.MongoDB
{
    public interface IMongoDatabaseProvider<TMongoDbContext>
        where TMongoDbContext : IAbpMongoDbContext
    {
        TMongoDbContext DbContext { get; }

        IMongoDatabase GetDatabase();
    }
}