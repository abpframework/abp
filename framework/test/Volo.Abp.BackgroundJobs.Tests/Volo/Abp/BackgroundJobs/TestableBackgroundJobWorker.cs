#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs;

/// <summary>
/// Exposes the protected members of <see cref="BackgroundJobWorker"/> for unit testing.
/// </summary>
public class TestableBackgroundJobWorker : BackgroundJobWorker
{
    public TestableBackgroundJobWorker(
        AbpAsyncTimer timer,
        IOptions<AbpBackgroundJobOptions> jobOptions,
        IOptions<AbpBackgroundJobWorkerOptions> workerOptions,
        IServiceScopeFactory serviceScopeFactory,
        IAbpDistributedLock distributedLock)
        : base(timer, jobOptions, workerOptions, serviceScopeFactory, distributedLock)
    {
    }

    public void ConfigureTest(
        BackgroundJobNameFilter? jobNameFilter = null,
        string? distributedLockName = null)
    {
        JobNameFilter = jobNameFilter ?? BackgroundJobNameFilter.None;
        DistributedLockName = distributedLockName ?? WorkerOptions.DistributedLockName;
    }

    public Task HandleJobSuccessPublicAsync(IBackgroundJobStore store, BackgroundJobInfo jobInfo, IClock clock)
    {
        return HandleJobSuccessAsync(store, jobInfo, clock);
    }

    public Task ExecuteJobsInParallelPublicAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        return ExecuteJobsInParallelAsync(workerContext);
    }

    public Task<List<BackgroundJobInfo>> GetWaitingJobsPublicAsync(PeriodicBackgroundWorkerContext workerContext, IBackgroundJobStore store)
    {
        return GetWaitingJobsAsync(workerContext, store);
    }

    public bool IsJobEligiblePublic(BackgroundJobInfo? jobInfo, IClock clock)
    {
        return IsJobEligible(jobInfo, clock);
    }
}
