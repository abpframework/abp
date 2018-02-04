using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.MultiTenancy.EntityFrameworkCore
{
    [ConnectionStringName("AbpMultiTenancy")]
    public class MultiTenancyDbContext : AbpDbContext<MultiTenancyDbContext>, IMultiTenancyDbContext
    {
        public static string TablePrefix { get; set; } = AbpMultiTenancyConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = AbpMultiTenancyConsts.DefaultDbSchema;

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

        public MultiTenancyDbContext(DbContextOptions<MultiTenancyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureMultiTenancy(TablePrefix, Schema);
        }
    }
}
