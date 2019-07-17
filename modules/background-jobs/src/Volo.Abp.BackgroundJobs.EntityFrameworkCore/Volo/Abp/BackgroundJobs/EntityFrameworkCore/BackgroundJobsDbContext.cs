using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.BackgroundJobs.EntityFrameworkCore
{
    [ConnectionStringName(BackgroundJobsConsts.ConnectionStringName)]
    public class BackgroundJobsDbContext : AbpDbContext<BackgroundJobsDbContext>, IBackgroundJobsDbContext
    {
        public static string TablePrefix { get; set; } = BackgroundJobsConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = BackgroundJobsConsts.DefaultDbSchema;

        public DbSet<BackgroundJobRecord> BackgroundJobs { get; set; }

        public BackgroundJobsDbContext(DbContextOptions<BackgroundJobsDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBackgroundJobs(options =>
            {
                options.TablePrefix = TablePrefix;
                options.Schema = Schema;
            });
        }
    }
}