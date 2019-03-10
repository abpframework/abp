using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Abp.FeatureManagement.MongoDB
{
    public static class FeatureManagementMongoDbContextExtensions
    {
        public static void ConfigureFeatureManagement(
            this IMongoModelBuilder builder,
            Action<MongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new FeatureManagementMongoModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);
        }
    }
}