using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.TenantManagement.EntityFrameworkCore
{
    [ConnectionStringName(AbpTenantManagementConsts.ConnectionStringName)]
    public class TenantManagementDbContext : AbpDbContext<TenantManagementDbContext>, ITenantManagementDbContext
    {
        public static string TablePrefix { get; set; } = AbpTenantManagementConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = AbpTenantManagementConsts.DefaultDbSchema;

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

        public TenantManagementDbContext(DbContextOptions<TenantManagementDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureTenantManagement(TablePrefix, Schema);
        }
    }
}
