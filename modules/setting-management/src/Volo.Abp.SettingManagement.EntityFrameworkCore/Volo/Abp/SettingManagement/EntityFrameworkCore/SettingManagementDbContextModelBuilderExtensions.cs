using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Volo.Abp.SettingManagement.EntityFrameworkCore
{
    public class SettingManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public SettingManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix,
            [CanBeNull] string schema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }

    public static class SettingManagementDbContextModelBuilderExtensions
    {
        //TODO: Instead of getting parameters, get a action of SettingManagementModelBuilderConfigurationOptions like other modules
        public static void ConfigureSettingManagement(
            [NotNull] this ModelBuilder builder,
            [CanBeNull] Action<SettingManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new SettingManagementModelBuilderConfigurationOptions(
                AbpSettingManagementDbProperties.DbTablePrefix,
                AbpSettingManagementDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<Setting>(b =>
            {
                b.ToTable(options.TablePrefix + "Settings", options.Schema);

                b.Property(x => x.Name).HasMaxLength(SettingConsts.MaxNameLength).IsRequired();
                b.Property(x => x.Value).HasMaxLength(SettingConsts.MaxValueLength).IsRequired();
                b.Property(x => x.ProviderName).HasMaxLength(SettingConsts.MaxProviderNameLength);
                b.Property(x => x.ProviderKey).HasMaxLength(SettingConsts.MaxProviderKeyLength);

                b.HasIndex(x => new {x.Name, x.ProviderName, x.ProviderKey});
            });
        }
    }
}