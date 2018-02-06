using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Settings.EntityFrameworkCore
{
    public class AbpSettingsDbContext : AbpDbContext<AbpSettingsDbContext>, IAbpSettingsDbContext
    {
        public DbSet<Setting> Settings { get; set; }

        public AbpSettingsDbContext(DbContextOptions<AbpSettingsDbContext> options)
            : base(options)
        {

        }
    }
}
