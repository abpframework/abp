using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

        public BackgroundJobsDbContext(
            DbContextOptions<BackgroundJobsDbContext> options,
            IOptions<NamingConventionOptions> namingConventionOptions)
            : base(options, namingConventionOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBackgroundJobs();

            builder.ConfigureNamingConvention<BackgroundJobsDbContext>(this.NamingConventionOptions);

        }

    }
}
