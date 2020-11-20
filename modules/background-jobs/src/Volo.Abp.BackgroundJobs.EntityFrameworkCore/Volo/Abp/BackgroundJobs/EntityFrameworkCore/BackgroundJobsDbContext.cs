using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.BackgroundJobs.EntityFrameworkCore
{
    [ConnectionStringName(BackgroundJobsDbProperties.ConnectionStringName)]
    public class BackgroundJobsDbContext : AbpDbContext<BackgroundJobsDbContext>, IBackgroundJobsDbContext
    {
        public DbSet<BackgroundJobRecord> BackgroundJobs { get; set; }

        public BackgroundJobsDbContext(DbContextOptions<BackgroundJobsDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.NamingConventionsRewriteName(BackgroundJobsDbProperties.DbNamingConvention);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBackgroundJobs();

            builder.NamingConventionsRewriteName(BackgroundJobsDbProperties.DbNamingConvention);

        }

    }
}