using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.TenantManagement.EntityFrameworkCore
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(AbpTenantManagementDbProperties.ConnectionStringName)]
    public interface ITenantManagementDbContext : IEfCoreDbContext
    {
        DbSet<Tenant> Tenants { get; }

        DbSet<TenantConnectionString> TenantConnectionStrings { get; }
    }
}
