using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Volo.Abp.MongoDB.Clients;

public interface IAbpMongoClientFactory
{
    Task<MongoClient> GetAsync(MongoUrl mongoUrl);

    [Obsolete("Use GetAsync method")]
    MongoClient Get(MongoUrl mongoUrl);
}
