using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Quartz.Database.EntityFrameworkCore;

namespace Quartz.Database.Sqlite.ConsoleApp.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class QuartzDatabaseDemoDbContext : AbpDbContext<QuartzDatabaseDemoDbContext>
    {
        public QuartzDatabaseDemoDbContext(DbContextOptions<QuartzDatabaseDemoDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureQuartzDatabase();
        }
    }
}
