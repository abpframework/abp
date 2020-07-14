using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Volo.CmsKit.MongoDB
{
    public static class CmsKitMongoDbContextExtensions
    {
        public static void ConfigureCmsKit(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new CmsKitMongoModelBuilderConfigurationOptions(
                CmsKitDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}