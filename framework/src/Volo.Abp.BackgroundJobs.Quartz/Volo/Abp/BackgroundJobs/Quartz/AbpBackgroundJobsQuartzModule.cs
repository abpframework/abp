using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace Volo.Abp.BackgroundJobs.Quartz;

[DependsOn(
    typeof(AbpBackgroundJobsAbstractionsModule),
    typeof(AbpQuartzModule)
)]
public class AbpBackgroundJobsQuartzModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTransient(typeof(QuartzJobExecutionAdapter<>));
    }

    public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetService<IOptions<AbpBackgroundJobOptions>>().Value;
        if (!options.IsJobExecutionEnabled)
        {
            var quartzOptions = context.ServiceProvider.GetService<IOptions<AbpQuartzOptions>>().Value;
            quartzOptions.StartSchedulerFactory = scheduler => Task.CompletedTask;
        }
    }
}
