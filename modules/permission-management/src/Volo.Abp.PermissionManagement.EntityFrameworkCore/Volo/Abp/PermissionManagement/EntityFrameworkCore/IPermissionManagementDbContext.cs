using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.PermissionManagement.EntityFrameworkCore;

[ConnectionStringName(AbpPermissionManagementDbProperties.ConnectionStringName)]
public interface IPermissionManagementDbContext : IEfCoreDbContext
{
    DbSet<PermissionGroupDefinitionRecord> PermissionGroups { get; }
    
    DbSet<PermissionDefinitionRecord> Permissions { get; }
    
    DbSet<PermissionGrant> PermissionGrants { get; }
}
