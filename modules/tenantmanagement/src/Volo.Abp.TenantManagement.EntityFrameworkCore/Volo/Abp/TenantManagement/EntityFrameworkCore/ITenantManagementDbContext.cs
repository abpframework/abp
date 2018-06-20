using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.TenantManagement.EntityFrameworkCore
{
    [ConnectionStringName("AbpTenantManagement")]
    public interface ITenantManagementDbContext : IEfCoreDbContext
    {
        DbSet<Tenant> Tenants { get; set; }

        DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }
    }
}