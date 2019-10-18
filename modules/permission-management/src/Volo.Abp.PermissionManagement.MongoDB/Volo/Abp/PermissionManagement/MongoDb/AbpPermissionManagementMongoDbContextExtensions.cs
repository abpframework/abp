using System;
using Volo.Abp.MongoDB;

namespace Volo.Abp.PermissionManagement.MongoDB
{
    public static class AbpPermissionManagementMongoDbContextExtensions
    {
        public static void ConfigurePermissionManagement(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new PermissionManagementMongoModelBuilderConfigurationOptions(
                AbpPermissionManagementDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);

            builder.Entity<PermissionGrant>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "PermissionGrants";
            });
        }
    }
}