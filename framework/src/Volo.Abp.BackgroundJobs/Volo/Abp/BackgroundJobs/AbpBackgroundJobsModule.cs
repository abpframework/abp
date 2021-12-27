using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs;

[DependsOn(
    typeof(AbpBackgroundJobsAbstractionsModule),
    typeof(AbpBackgroundWorkersModule),
    typeof(AbpTimingModule),
    typeof(AbpGuidsModule),
    typeof(AbpDistributedLockingAbstractionsModule)
    )]
public class AbpBackgroundJobsModule : AbpModule
{
    public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundJobOptions>>().Value;
        if (options.IsJobExecutionEnabled)
        {
            await context.AddBackgroundWorkerAsync<IBackgroundJobWorker>();
        }
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundJobOptions>>().Value;
        if (options.IsJobExecutionEnabled)
        {
            AsyncHelper.RunSync(() => context.AddBackgroundWorkerAsync<IBackgroundJobWorker>());
        }
    }
}
