using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.MultiTenancy.EntityFrameworkCore
{
    public class MultiTenancyDbContext : AbpDbContext<MultiTenancyDbContext>, IMultiTenancyDbContext
    {
        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

        public MultiTenancyDbContext(DbContextOptions<MultiTenancyDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            this.ConfigureMultiTenancy(builder);
        }
    }
}
