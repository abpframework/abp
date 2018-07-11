using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyModuleName.MongoDB
{
    public static class AbpUsersMongoDbContextExtensions
    {
        public static void ConfigureMyModuleName(
            this IMongoModelBuilder builder,
            Action<MongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new MyModuleNameMongoModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);
        }
    }
}