using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.TenantManagement.MongoDb
{
    [ConnectionStringName(AbpTenantManagementConsts.ConnectionStringName)]
    public interface ITenantManagementMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<Tenant> Tenants { get; }
    }
}