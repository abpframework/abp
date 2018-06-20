using System;
using Volo.Abp.MongoDB;

namespace Volo.Abp.SettingManagement.MongoDB
{
    public static class AbpSettingManagementMongoDbContextExtensions
    {
        public static void ConfigureSettingManagement(
            this IMongoModelBuilder builder,
            Action<MongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new SettingManagementMongoModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);

            builder.Entity<Setting>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Settings";
            });
        }
    }
}