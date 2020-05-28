using System;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    public static class BlobStoringDatabaseMongoDbContextExtensions
    {
        public static void ConfigureDatabaseBlobStoring(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BlobStoringDatabaseMongoModelBuilderConfigurationOptions(
                BlobStoringDatabaseDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);

            builder.Entity<Container>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Containers";
            });

            builder.Entity<Blob>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Blobs";
            });
        }
    }
}