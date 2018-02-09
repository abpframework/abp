using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Settings.EntityFrameworkCore
{
    [ConnectionStringName("AbpSettings")]
    public class AbpSettingsDbContext : AbpDbContext<AbpSettingsDbContext>, IAbpSettingsDbContext
    {
        public static string TablePrefix { get; set; } = AbpSettingsConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = AbpSettingsConsts.DefaultDbSchema;

        public DbSet<Setting> Settings { get; set; }

        public AbpSettingsDbContext(DbContextOptions<AbpSettingsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureAbpSettings(TablePrefix, Schema);
        }
    }
}
