using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BackgroundJobs.EntityFrameworkCore
{
    [IgnoreMultiTenancy]
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

            optionsBuilder.NamingConventionsRewriteName(AbpCommonDbProperties.DbNamingConvention);

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBackgroundJobs();

            builder.NamingConventionsRewriteName(AbpCommonDbProperties.DbNamingConvention);

        }

    }
}
