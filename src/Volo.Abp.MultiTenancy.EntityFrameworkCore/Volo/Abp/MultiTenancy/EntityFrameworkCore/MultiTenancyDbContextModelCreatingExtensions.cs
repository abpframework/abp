using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.MultiTenancy.EntityFrameworkCore
{
    public static class MultiTenancyDbContextModelCreatingExtensions
    {
        public static void ConfigureMultiTenancy(this IMultiTenancyDbContext dbContext, ModelBuilder builder)
        {
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