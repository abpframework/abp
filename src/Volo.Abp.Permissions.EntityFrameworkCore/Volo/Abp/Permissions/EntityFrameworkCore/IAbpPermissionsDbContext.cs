using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Permissions.EntityFrameworkCore
{
    [ConnectionStringName("AbpPermissions")]
    public interface IAbpPermissionsDbContext : IEfCoreDbContext
    {
        DbSet<PermissionGrant> PermissionGrants { get; set; }
    }
}