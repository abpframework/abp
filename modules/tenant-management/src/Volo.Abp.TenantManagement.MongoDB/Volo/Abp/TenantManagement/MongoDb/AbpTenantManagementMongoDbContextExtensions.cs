using System;
using Volo.Abp.MongoDB;

namespace Volo.Abp.TenantManagement.MongoDb
{
    public static class AbpTenantManagementMongoDbContextExtensions
    {
        public static void ConfigureTenantManagement(
            this IMongoModelBuilder builder,
            Action<MongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new TenantManagementMongoModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);

            builder.Entity<Tenant>(b =>
            {
                b.CollectionName = options.CollectionPrefix + "Tenants";
            });
        }
    }
}