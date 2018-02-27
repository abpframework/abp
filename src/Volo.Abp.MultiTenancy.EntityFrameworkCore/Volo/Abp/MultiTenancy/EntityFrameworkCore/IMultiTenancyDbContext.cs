using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.MultiTenancy.EntityFrameworkCore
{
    [ConnectionStringName("AbpMultiTenancy")]
    public interface IMultiTenancyDbContext : IEfCoreDbContext
    {
        DbSet<Tenant> Tenants { get; set; }

        DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }
    }
}