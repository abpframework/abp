using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.FeatureManagement.EntityFrameworkCore
{
    public static class FeatureManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureFeatureManagement(
            this ModelBuilder builder,
            Action<FeatureManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            if (builder.IsTenantOnlyDatabase())
            {
                return;
            }

            var options = new FeatureManagementModelBuilderConfigurationOptions(
                FeatureManagementDbProperties.DbTablePrefix,
                FeatureManagementDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<FeatureValue>(b =>
            {
                b.ToTable(options.TablePrefix + "FeatureValues", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).HasMaxLength(FeatureValueConsts.MaxNameLength).IsRequired();
                b.Property(x => x.Value).HasMaxLength(FeatureValueConsts.MaxValueLength).IsRequired();
                b.Property(x => x.ProviderName).HasMaxLength(FeatureValueConsts.MaxProviderNameLength);
                b.Property(x => x.ProviderKey).HasMaxLength(FeatureValueConsts.MaxProviderKeyLength);

                b.HasIndex(x => new { x.Name, x.ProviderName, x.ProviderKey });

                b.ApplyObjectExtensionMappings();
            });

            builder.TryConfigureObjectExtensions<FeatureManagementDbContext>();
        }
    }
}
