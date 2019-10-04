using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.TenantManagement.MongoDB
{
    [ConnectionStringName(AbpTenantManagementConsts.ConnectionStringName)]
    public class TenantManagementMongoDbContext : AbpMongoDbContext, ITenantManagementMongoDbContext
    {
        public static string CollectionPrefix { get; set; } = AbpTenantManagementConsts.DefaultDbTablePrefix;

        public IMongoCollection<Tenant> Tenants => Collection<Tenant>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureTenantManagement(options =>
            {
                options.CollectionPrefix = CollectionPrefix;
            });
        }
    }
}