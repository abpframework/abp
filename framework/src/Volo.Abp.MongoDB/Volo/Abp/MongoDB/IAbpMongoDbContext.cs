using MongoDB.Driver;

namespace Volo.Abp.MongoDB
{
    public interface IAbpMongoDbContext
    {
        IMongoClient Client { get; }

        IMongoDatabase Database { get; }

        IMongoCollection<T> Collection<T>();

        IClientSessionHandle SessionHandle { get; }
    }
}
