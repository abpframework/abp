using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.Settings.EntityFrameworkCore
{
    public static class AbpSettingsDbContextModelBuilderExtensions
    {
        public static void ConfigureAbpSettings(
            [NotNull] this ModelBuilder builder,
            [CanBeNull] string tablePrefix = AbpSettingsConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = AbpSettingsConsts.DefaultDbSchema)
        {
            Check.NotNull(builder, nameof(builder));

            if (tablePrefix == null)
            {
                tablePrefix = "";
            }

            builder.Entity<Setting>(b =>
            {
                b.ToTable(tablePrefix + "Settings", schema);

                b.Property(x => x.Name).HasMaxLength(SettingsConsts.MaxNameLength).IsRequired();
                b.Property(x => x.Value).HasMaxLength(SettingsConsts.MaxValueLength).IsRequired();
                b.Property(x => x.ProviderName).HasMaxLength(SettingsConsts.MaxProviderNameLength);
                b.Property(x => x.ProviderKey).HasMaxLength(SettingsConsts.MaxProviderKeyLength);

                b.HasIndex(x => new {x.Name, x.ProviderName, x.ProviderKey});
            });
        }
    }
}