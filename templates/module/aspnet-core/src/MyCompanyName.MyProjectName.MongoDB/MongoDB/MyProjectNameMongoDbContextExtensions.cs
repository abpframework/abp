using System;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace MyCompanyName.MyProjectName.MongoDB
{
    public static class MyProjectNameMongoDbContextExtensions
    {
        public static void ConfigureMyProjectName(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new MyProjectNameMongoModelBuilderConfigurationOptions(
                MyProjectNameDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);
        }
    }
}