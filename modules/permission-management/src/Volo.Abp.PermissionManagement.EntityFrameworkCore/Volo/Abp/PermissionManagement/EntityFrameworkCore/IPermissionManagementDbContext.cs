using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.PermissionManagement.EntityFrameworkCore
{
    [ConnectionStringName(AbpPermissionManagementConsts.ConnectionStringName)]
    public interface IPermissionManagementDbContext : IEfCoreDbContext
    {
        DbSet<PermissionGrant> PermissionGrants { get; set; }
    }
}