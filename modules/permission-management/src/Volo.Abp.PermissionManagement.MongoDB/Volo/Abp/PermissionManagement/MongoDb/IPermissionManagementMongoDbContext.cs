using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Volo.Abp.PermissionManagement.MongoDB
{
    [ConnectionStringName(AbpPermissionManagementDbProperties.ConnectionStringName)]
    public interface IPermissionManagementMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<PermissionGrant> PermissionGrants { get; }
    }
}