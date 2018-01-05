using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.MultiTenancy.EntityFrameworkCore
{
    public class MultiTenancyDbContext : AbpDbContext<MultiTenancyDbContext>
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

            //TODO: Split configuration to dedicated classes (probably a resuable extension method)

            builder.Entity<Tenant>(b =>
            {
                b.ToTable("MtTenants"); //TODO: Make all table and schema names changeable

                b.Property(t => t.Name).IsRequired().HasMaxLength(TenantConsts.MaxNameLength);

                b.HasMany(u => u.ConnectionStrings).WithOne().HasForeignKey(uc => uc.TenantId).IsRequired();

                b.HasIndex(u => u.Name);
            });

            builder.Entity<TenantConnectionString>(b =>
            {
                b.ToTable("MtTenantConnectionStrings");

                b.Property(cs => cs.Name).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxNameLength);
                b.Property(cs => cs.Value).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxValueLength);

                b.HasIndex(cs => cs.TenantId);
            });
        }
    }
}
