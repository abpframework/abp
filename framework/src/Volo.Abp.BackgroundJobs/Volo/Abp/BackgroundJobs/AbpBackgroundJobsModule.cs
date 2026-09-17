using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs;

[DependsOn(
    typeof(AbpBackgroundJobsAbstractionsModule),
    typeof(AbpBackgroundWorkersModule),
    typeof(AbpTimingModule),
    typeof(AbpGuidsModule),
    typeof(AbpDistributedLockingAbstractionsModule),
    typeof(AbpMultiTenancyModule)
    )]
public class AbpBackgroundJobsModule : AbpModule
{
    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        // The manager decides (based on options) whether and which workers to run.
        await context.AddBackgroundWorkerAsync<BackgroundJobWorkerManager>();

        // Only register the cleanup worker when it has something to do (retained successful jobs).
        var jobOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundJobOptions>>().Value;
        var workerOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundJobWorkerOptions>>().Value;
        if (jobOptions.IsJobExecutionEnabled && workerOptions.StoreSuccessfulJobs && workerOptions.SuccessfulJobRetentionTime != null)
        {
            await context.AddBackgroundWorkerAsync<BackgroundJobCleanupWorker>();
        }
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }
}
