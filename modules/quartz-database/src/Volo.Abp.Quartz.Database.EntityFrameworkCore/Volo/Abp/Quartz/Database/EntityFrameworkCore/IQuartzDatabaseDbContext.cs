using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Quartz.Database.EntityFrameworkCore
{
    [ConnectionStringName(QuartzDatabaseDbProperties.ConnectionStringName)]
    public interface IQuartzDatabaseDbContext : IEfCoreDbContext
    {
        DbSet<QuartzBlobTrigger> BlobTriggers { get; }

        DbSet<QuartzCalendar> Calendars { get; }

        DbSet<QuartzCronTrigger> CronTriggers { get; }

        DbSet<QuartzFiredTrigger> FiredTriggers { get; }

        DbSet<QuartzJobDetail> JobDetails { get; }

        DbSet<QuartzLock> Locks { get; }

        DbSet<QuartzPausedTriggerGroup> PausedTriggerGroups { get; }

        DbSet<QuartzSchedulerState> SchedulerStates { get; }

        DbSet<QuartzSimplePropertyTrigger> SimplePropertyTriggers { get; }

        DbSet<QuartzSimpleTrigger> SimpleTriggers { get; }

        DbSet<QuartzTrigger> Triggers { get; }
    }
}
