using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace Volo.Abp.BackgroundJobs.Quartz
{
    [DependsOn(
        typeof(AbpBackgroundJobsAbstractionsModule),
        typeof(AbpQuartzModule)
    )]
    public class AbpBackgroundJobsQuartzModule :AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var options = context.Services.ExecutePreConfiguredActions<AbpBackgroundJobOptions>();
            if (!options.IsJobExecutionEnabled)
            {
                context.Services.AddSingleton<IBackgroundJobManager>(x => new NullBackgroundJobManager());
            }
            else
            {
                context.Services.AddTransient(typeof(QuartzJobExecutionAdapter<>));
            }
        }
    }
}
