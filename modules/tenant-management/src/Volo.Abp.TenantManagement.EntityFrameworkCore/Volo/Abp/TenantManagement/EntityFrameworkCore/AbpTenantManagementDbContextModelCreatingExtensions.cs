using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.TenantManagement.EntityFrameworkCore
{
    public static class AbpTenantManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureTenantManagement(
            this ModelBuilder builder,
            [CanBeNull] string tablePrefix = AbpTenantManagementConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = AbpTenantManagementConsts.DefaultDbSchema)
        {
            Check.NotNull(builder, nameof(builder));

            if (tablePrefix == null)
            {
                tablePrefix = "";
            }

            builder.Entity<Tenant>(b =>
            {
                b.ToTable(tablePrefix + "Tenants", schema);

                b.ConfigureExtraProperties();

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