using System;
using Volo.Abp.MongoDB;

namespace Volo.Abp.FeatureManagement.MongoDB
{
    public static class FeatureManagementMongoDbContextExtensions
    {
        public static void ConfigureFeatureManagement(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new FeatureManagementMongoModelBuilderConfigurationOptions(
                FeatureManagementDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);

            builder.Entity<FeatureValue>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "FeatureValues";
            });
        }
    }
}