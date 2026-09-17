using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs;

/// <summary>
/// A background job worker that polls and executes waiting jobs.
/// Instances are created, configured and started by <see cref="BackgroundJobWorkerManager"/>.
/// </summary>
public interface IBackgroundJobWorker
{
    /// <summary>
    /// Starts this worker.
    /// </summary>
    /// <param name="distributedLockName">
    /// Distributed lock name for this worker. When null, <see cref="AbpBackgroundJobWorkerOptions.DistributedLockName"/> is used.
    /// </param>
    /// <param name="jobNameFilter">Filters the jobs this worker processes by name. When null, all jobs are processed.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task StartAsync(
        string? distributedLockName = null,
        BackgroundJobNameFilter? jobNameFilter = null,
        CancellationToken cancellationToken = default);

    Task StopAsync(CancellationToken cancellationToken = default);
}
