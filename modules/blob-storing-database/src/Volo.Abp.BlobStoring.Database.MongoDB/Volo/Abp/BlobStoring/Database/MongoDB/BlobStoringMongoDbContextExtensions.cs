using System;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    public static class BlobStoringMongoDbContextExtensions
    {
        public static void ConfigureBlobStoring(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BlobStoringMongoModelBuilderConfigurationOptions(
                BlobStoringDatabaseDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);

            builder.Entity<DatabaseBlobContainer>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "BlobContainers";
            });

            builder.Entity<DatabaseBlob>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Blobs";
            });
        }
    }
}