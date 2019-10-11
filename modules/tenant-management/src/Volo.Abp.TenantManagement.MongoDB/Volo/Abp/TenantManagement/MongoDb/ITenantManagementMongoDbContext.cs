using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.TenantManagement.MongoDB
{
    [ConnectionStringName(AbpTenantManagementDbProperties.ConnectionStringName)]
    public interface ITenantManagementMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<Tenant> Tenants { get; }
    }
}