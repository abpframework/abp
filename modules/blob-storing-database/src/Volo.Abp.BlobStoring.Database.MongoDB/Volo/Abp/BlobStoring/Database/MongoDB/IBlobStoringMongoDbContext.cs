using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    [ConnectionStringName(BlobStoringDatabaseDbProperties.ConnectionStringName)]
    public interface IBlobStoringMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<DatabaseBlobContainer> BlobContainers { get; }
        
        IMongoCollection<DatabaseBlob> Blobs { get; }
    }
}
