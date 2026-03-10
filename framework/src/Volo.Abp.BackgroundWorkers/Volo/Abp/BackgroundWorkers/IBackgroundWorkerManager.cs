using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers;

/// <summary>
/// Used to manage background workers.
/// </summary>
public interface IBackgroundWorkerManager : IRunnable
{
    /// <summary>
    /// Adds a new worker. Starts the worker immediately if <see cref="IBackgroundWorkerManager"/> has started.
    /// </summary>
    /// <param name="worker">
    /// The worker. It should be resolved from IOC.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task AddAsync(IBackgroundWorker worker, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a dynamic worker by name and handler.
    /// </summary>
    Task AddAsync(
        string workerName,
        Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a dynamic worker by name, schedule and handler.
    /// </summary>
    Task AddAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a previously added dynamic worker by name.
    /// Returns true if the worker was found and removed; false otherwise.
    /// </summary>
    Task<bool> RemoveAsync(string workerName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the schedule of a previously added dynamic worker.
    /// Returns true if the worker was found and updated; false otherwise.
    /// </summary>
    Task<bool> UpdateScheduleAsync(string workerName, DynamicBackgroundWorkerSchedule schedule, CancellationToken cancellationToken = default);
}
