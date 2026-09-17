using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs;

public class TestableBackgroundJobCleanupWorker : BackgroundJobCleanupWorker
{
    public TestableBackgroundJobCleanupWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<AbpBackgroundJobOptions> jobOptions,
        IOptions<AbpBackgroundJobWorkerOptions> workerOptions,
        IAbpDistributedLock distributedLock)
        : base(timer, serviceScopeFactory, jobOptions, workerOptions, distributedLock)
    {
    }

    public Task DoWorkPublicAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        return DoWorkAsync(workerContext);
    }
}
