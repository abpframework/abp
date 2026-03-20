using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundWorkers;

/// <summary>
/// Manages dynamic background workers that are registered at runtime
/// without requiring a strongly-typed worker class.
/// </summary>
public interface IDynamicBackgroundWorkerManager
{
    /// <summary>
    /// Adds a dynamic worker by name, schedule and handler.
    /// If a worker with the same name already exists, it will be replaced.
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
    Task<bool> UpdateScheduleAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a dynamic worker with the given name is registered.
    /// </summary>
    bool IsRegistered(string workerName);
}
