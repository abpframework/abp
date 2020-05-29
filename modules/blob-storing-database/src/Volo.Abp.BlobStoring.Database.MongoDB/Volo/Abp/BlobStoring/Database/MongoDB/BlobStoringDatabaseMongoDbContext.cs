using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    [ConnectionStringName(BlobStoringDatabaseDbProperties.ConnectionStringName)]
    public class BlobStoringDatabaseMongoDbContext : AbpMongoDbContext, IBlobStoringDatabaseMongoDbContext
    {
        public IMongoCollection<Container> Containers => Collection<Container>();

        public IMongoCollection<Blob> Blobs => Collection<Blob>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureDatabaseBlobStoring();
        }
    }
}