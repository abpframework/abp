using System;
using Volo.Abp.MongoDB;

namespace Volo.Abp.TenantManagement.MongoDB
{
    public static class AbpTenantManagementMongoDbContextExtensions
    {
        public static void ConfigureTenantManagement(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new TenantManagementMongoModelBuilderConfigurationOptions(
                AbpTenantManagementDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);

            builder.Entity<Tenant>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Tenants";
            });
        }
    }
}