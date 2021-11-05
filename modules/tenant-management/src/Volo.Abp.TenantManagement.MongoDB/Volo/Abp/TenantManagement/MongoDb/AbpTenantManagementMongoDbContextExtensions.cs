using Volo.Abp.MongoDB;

namespace Volo.Abp.TenantManagement.MongoDB
{
    public static class AbpTenantManagementMongoDbContextExtensions
    {
        public static void ConfigureTenantManagement(
            this IMongoModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Tenant>(b =>
            {
                b.CollectionName = AbpTenantManagementDbProperties.DbTablePrefix + "Tenants";
            });
        }
    }
}