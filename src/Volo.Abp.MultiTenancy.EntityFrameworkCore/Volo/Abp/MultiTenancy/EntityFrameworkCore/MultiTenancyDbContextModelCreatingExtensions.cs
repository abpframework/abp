using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.MultiTenancy.EntityFrameworkCore
{
    public static class MultiTenancyDbContextModelCreatingExtensions
    {
        public static void ConfigureMultiTenancy(
            this ModelBuilder builder,
            [CanBeNull]string tablePrefix = AbpMultiTenancyConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = AbpMultiTenancyConsts.DefaultDbSchema)
        {
            Check.NotNull(builder, nameof(builder));

            if (tablePrefix == null)
            {
                tablePrefix = "";
            }

            builder.Entity<Tenant>(b =>
            {
                b.ToTable(tablePrefix + "Tenants", schema);

                b.Property(t => t.Name).IsRequired().HasMaxLength(TenantConsts.MaxNameLength);

                b.HasMany(u => u.ConnectionStrings).WithOne().HasForeignKey(uc => uc.TenantId).IsRequired();

                b.HasIndex(u => u.Name).IsUnique();
            });

            builder.Entity<TenantConnectionString>(b =>
            {
                b.ToTable(tablePrefix + "TenantConnectionStrings", schema);

                b.HasKey(x => new { x.TenantId, x.Name });

                b.Property(cs => cs.Name).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxNameLength);
                b.Property(cs => cs.Value).IsRequired().HasMaxLength(TenantConnectionStringConsts.MaxValueLength);
            });
        }
    }
}