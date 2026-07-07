using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs;

/// <summary>
/// Periodically deletes retained successfully completed jobs older than
/// <see cref="AbpBackgroundJobWorkerOptions.SuccessfulJobRetentionTime"/>.
/// Only relevant when <see cref="AbpBackgroundJobWorkerOptions.StoreSuccessfulJobs"/> is enabled.
/// </summary>
public class BackgroundJobCleanupWorker : AsyncPeriodicBackgroundWorkerBase
{
    protected AbpBackgroundJobOptions JobOptions { get; }

    protected AbpBackgroundJobWorkerOptions WorkerOptions { get; }

    protected IAbpDistributedLock DistributedLock { get; }

    public BackgroundJobCleanupWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<AbpBackgroundJobOptions> jobOptions,
        IOptions<AbpBackgroundJobWorkerOptions> workerOptions,
        IAbpDistributedLock distributedLock)
        : base(timer, serviceScopeFactory)
    {
        JobOptions = jobOptions.Value;
        WorkerOptions = workerOptions.Value;
        DistributedLock = distributedLock;
        Timer.Period = WorkerOptions.CleanSuccessfulJobsPeriod;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        if (!JobOptions.IsJobExecutionEnabled ||
            !WorkerOptions.StoreSuccessfulJobs ||
            WorkerOptions.SuccessfulJobRetentionTime == null)
        {
            return;
        }

        var store = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobStore>();
        var clock = workerContext.ServiceProvider.GetRequiredService<IClock>();
        var completedBefore = clock.Now.Subtract(WorkerOptions.SuccessfulJobRetentionTime.Value);

        await using (var handle = await DistributedLock.TryAcquireAsync(WorkerOptions.CleanupDistributedLockName, cancellationToken: StoppingToken))
        {
            if (handle == null)
            {
                return;
            }

            int deletedCount;
            do
            {
                deletedCount = await store.DeleteAsync(WorkerOptions.ApplicationName, completedBefore, WorkerOptions.MaxJobFetchCount, StoppingToken);
            }
            while (deletedCount > 0 && deletedCount >= WorkerOptions.MaxJobFetchCount && !StoppingToken.IsCancellationRequested);
        }
    }
}
