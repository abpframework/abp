using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.SettingManagement.EntityFrameworkCore
{
    public static class SettingManagementDbContextModelBuilderExtensions
    {
        //TODO: Instead of getting parameters, get a action of SettingManagementModelBuilderConfigurationOptions like other modules
        public static void ConfigureSettingManagement(
            [NotNull] this ModelBuilder builder,
            [CanBeNull] string tablePrefix = AbpSettingManagementConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = AbpSettingManagementConsts.DefaultDbSchema)
        {
            Check.NotNull(builder, nameof(builder));

            if (tablePrefix == null)
            {
                tablePrefix = "";
            }

            builder.Entity<Setting>(b =>
            {
                b.ToTable(tablePrefix + "Settings", schema);

                b.Property(x => x.Name).HasMaxLength(SettingConsts.MaxNameLength).IsRequired();
                b.Property(x => x.Value).HasMaxLength(SettingConsts.MaxValueLength).IsRequired();
                b.Property(x => x.ProviderName).HasMaxLength(SettingConsts.MaxProviderNameLength);
                b.Property(x => x.ProviderKey).HasMaxLength(SettingConsts.MaxProviderKeyLength);

                b.HasIndex(x => new {x.Name, x.ProviderName, x.ProviderKey});
            });
        }
    }
}