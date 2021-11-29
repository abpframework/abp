using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TenantManagement.MongoDB
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(AbpTenantManagementDbProperties.ConnectionStringName)]
    public interface ITenantManagementMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<Tenant> Tenants { get; }
    }
}
