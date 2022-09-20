using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB;

[ConnectionStringName(BlobStoringDatabaseDbProperties.ConnectionStringName)]
public class BlobStoringMongoDbContext : AbpMongoDbContext, IBlobStoringMongoDbContext
{
    public IMongoCollection<DatabaseBlobContainer> BlobContainers => Collection<DatabaseBlobContainer>();

    public IMongoCollection<DatabaseBlob> Blobs => Collection<DatabaseBlob>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureBlobStoring();
    }
}
