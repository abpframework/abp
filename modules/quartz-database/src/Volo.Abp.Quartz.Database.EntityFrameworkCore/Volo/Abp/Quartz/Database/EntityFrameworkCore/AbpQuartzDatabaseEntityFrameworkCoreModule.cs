using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Quartz.Database.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpQuartzDatabaseDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class AbpQuartzDatabaseEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<QuartzDatabaseDbContext>(options =>
            {
                options.AddRepository<QuartzJobDetail, QuartzJobDetailRepository>();
                options.AddRepository<QuartzTrigger, QuartzTriggerRepository>();
                options.AddRepository<QuartzFiredTrigger, QuartzFiredTriggerRepository>();
                options.AddRepository<QuartzPausedTriggerGroup, QuartzPausedTriggerGroupRepository>();
                options.AddRepository<QuartzSchedulerState, QuartzSchedulerStateRepository>();
                options.AddRepository<QuartzCalendar, QuartzCalendarRepository>();
            });
        }
    }
}