using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Volo.Abp.BlobStoring.Database.MongoDB
{
    public static class BlobStoringDatabaseMongoDbContextExtensions
    {
        public static void ConfigureDatabase(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BlobStoringDatabaseMongoModelBuilderConfigurationOptions(
                BlobStoringDatabaseDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}