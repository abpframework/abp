using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.DemoApp.Jobs;
using Volo.Abp.BackgroundJobs.DemoApp.Shared;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.DemoApp;

[DependsOn(
    typeof(DemoAppSharedModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule)
    )]
public class DemoAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(opts =>
            {
                opts.UseSqlServer();
            });
        });

        Configure<AbpBackgroundJobWorkerOptions>(options =>
        {
            //Configure for fast running
            options.JobPollPeriod = 1000;
            options.DefaultFirstWaitDuration = 1;
            options.DefaultWaitFactor = 1;

            // Keep every successfully completed job as history (marks CompletionTime instead of deleting).
            // Completed jobs are excluded from the waiting query and pruned after SuccessfulJobRetentionTime.
            options.StoreSuccessfulJobs = true;
            options.SuccessfulJobRetentionTime = System.TimeSpan.FromDays(1);

            // A dedicated worker (with its own distributed lock "DemoFeesWorkerLock") that only processes
            // the slow fee-calculation jobs, so they don't block other jobs. A default worker is added automatically
            // and processes all the remaining job types (e.g. SendEmailJob).
            options.AddDedicatedWorker<CalculateAwsFeesJobArgs, CalculateAzureFeesJobArgs>("DemoFeesWorkerLock");

            // Let each worker execute up to 4 jobs in parallel (each job claimed with its own distributed lock,
            // so multiple application instances can execute different jobs concurrently).
            options.MaxParallelJobExecutionCount = 4;
        });
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        // Enqueue a few demo jobs. The fee-calculation jobs are handled by the dedicated worker,
        // while SendEmailJob is handled by the default worker.
        var backgroundJobManager = context.ServiceProvider.GetRequiredService<IBackgroundJobManager>();

        await backgroundJobManager.EnqueueAsync(new CalculateAwsFeesJobArgs { AccountId = "acc-1" });
        await backgroundJobManager.EnqueueAsync(new CalculateAzureFeesJobArgs { SubscriptionId = "sub-1" });
        await backgroundJobManager.EnqueueAsync(new SendEmailJobArgs { To = "user@example.com", Subject = "Welcome" });
    }
}
