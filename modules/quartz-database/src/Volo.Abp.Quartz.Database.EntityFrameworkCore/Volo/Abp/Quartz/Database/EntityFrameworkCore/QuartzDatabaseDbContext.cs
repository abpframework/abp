using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Quartz.Database.EntityFrameworkCore
{
    [ConnectionStringName(QuartzDatabaseDbProperties.ConnectionStringName)]
    public class QuartzDatabaseDbContext : AbpDbContext<QuartzDatabaseDbContext>, IQuartzDatabaseDbContext
    {
        public DbSet<QuartzBlobTrigger> BlobTriggers { get; set; }
        public DbSet<QuartzCalendar> Calendars { get; set; }
        public DbSet<QuartzCronTrigger> CronTriggers { get; set; }
        public DbSet<QuartzFiredTrigger> FiredTriggers { get; set; }
        public DbSet<QuartzJobDetail> JobDetails { get; set; }
        public DbSet<QuartzLock> Locks { get; set; }
        public DbSet<QuartzPausedTriggerGroup> PausedTriggerGroups { get; set; }
        public DbSet<QuartzSchedulerState> SchedulerStates { get; set; }
        public DbSet<QuartzSimplePropertyTrigger> SimplePropertyTriggers { get; set; }
        public DbSet<QuartzSimpleTrigger> SimpleTriggers { get; set; }
        public DbSet<QuartzTrigger> Triggers { get; set; }

        public QuartzDatabaseDbContext(DbContextOptions<QuartzDatabaseDbContext> options)
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