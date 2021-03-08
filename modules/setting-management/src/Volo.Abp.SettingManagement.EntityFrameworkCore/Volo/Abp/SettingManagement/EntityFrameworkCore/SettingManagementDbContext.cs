using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.SettingManagement.EntityFrameworkCore
{
    [IgnoreMultiTenancy]
    [ConnectionStringName(AbpSettingManagementDbProperties.ConnectionStringName)]
    public class SettingManagementDbContext : AbpDbContext<SettingManagementDbContext>, ISettingManagementDbContext
    {
        public DbSet<Setting> Settings { get; set; }

        public SettingManagementDbContext(DbContextOptions<SettingManagementDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            AbpDbContextEvent.OnConfiguring(nameof(SettingManagementDbContext), optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureSettingManagement();

            AbpDbContextEvent.OnModelCreating(nameof(SettingManagementDbContext), builder);

        }

    }
}
