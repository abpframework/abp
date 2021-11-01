using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    public static class BlobStoringMongoDbContextExtensions
    {
        public static void ConfigureBlobStoring(
            this IMongoModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<DatabaseBlobContainer>(b =>
            {
                b.CollectionName = BlobStoringDatabaseDbProperties.DbTablePrefix + "BlobContainers";
            });

            builder.Entity<DatabaseBlob>(b =>
            {
                b.CollectionName = BlobStoringDatabaseDbProperties.DbTablePrefix + "Blobs";
            });
        }
    }
}
