using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.SettingManagement.EntityFrameworkCore
{
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

            optionsBuilder.NamingConventionsRewriteName(AbpSettingManagementDbProperties.DbNamingConvention);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureSettingManagement();

            builder.NamingConventionsRewriteName(AbpSettingManagementDbProperties.DbNamingConvention);

        }

    }
}
