using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.SettingManagement.EntityFrameworkCore
{
    [ConnectionStringName("AbpSettingManagement")]
    public class AbpSettingManagementDbContext : AbpDbContext<AbpSettingManagementDbContext>, ISettingManagementDbContext
    {
        public static string TablePrefix { get; set; } = AbpSettingManagementConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = AbpSettingManagementConsts.DefaultDbSchema;

        public DbSet<Setting> Settings { get; set; }

        public AbpSettingManagementDbContext(DbContextOptions<AbpSettingManagementDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureSettingManagement(TablePrefix, Schema);
        }
    }
}
