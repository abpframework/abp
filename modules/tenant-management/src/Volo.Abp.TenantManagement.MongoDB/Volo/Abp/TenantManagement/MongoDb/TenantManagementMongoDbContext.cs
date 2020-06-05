using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.TenantManagement.MongoDB
{
    [ConnectionStringName(AbpTenantManagementDbProperties.ConnectionStringName)]
    public class TenantManagementMongoDbContext : AbpMongoDbContext, ITenantManagementMongoDbContext
    {
        public IMongoCollection<Tenant> Tenants => Collection<Tenant>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureTenantManagement();
        }
    }
}