using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    [ConnectionStringName(BlobStoringDatabaseDbProperties.ConnectionStringName)]
    public interface IBlobStoringDatabaseMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<Container> Containers { get; }
        
        IMongoCollection<Blob> Blobs { get; }
    }
}
