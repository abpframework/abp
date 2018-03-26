using System.Collections.Generic;

namespace Volo.Abp.MongoDB
{
    public interface IAbpMongoDbContext
    {
        IReadOnlyList<MongoEntityMapping> GetMappings();

        string GetCollectionName<T>();
    }
}