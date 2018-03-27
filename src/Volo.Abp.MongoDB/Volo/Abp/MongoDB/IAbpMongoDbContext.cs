using System.Collections.Generic;
using MongoDB.Driver;

namespace Volo.Abp.MongoDB
{
    public interface IAbpMongoDbContext
    {
        IMongoDatabase Database { get; }

        IMongoCollection<T> Collection<T>();

        string GetCollectionName<T>();

        IReadOnlyList<MongoEntityMapping> GetMappings(); //TODO: Consider to remove from the interface
    }
}