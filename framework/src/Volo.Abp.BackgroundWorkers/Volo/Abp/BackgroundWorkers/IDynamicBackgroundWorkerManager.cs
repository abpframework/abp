using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundWorkers;

/// <summary>
/// Manages dynamic background workers that are registered at runtime
/// without requiring a strongly-typed worker class.
/// <para>
/// Implementations may differ in capabilities. Check <see cref="ISupportsRuntimeRegistration"/>
/// before calling <see cref="AddAsync"/> / <see cref="RemoveAsync"/> / <see cref="UpdateScheduleAsync"/>,
/// and <see cref="ISupportsCronScheduling"/> before passing
/// <see cref="DynamicBackgroundWorkerSchedule.CronExpression"/>.
/// </para>
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
        DynamicBackgroundWorkerHandler handler,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a previously added dynamic worker by name.
    /// Always attempts to remove both the in-memory handler and any persistent scheduling record
    /// (Hangfire RecurringJob or Quartz job), regardless of current in-memory state.
    /// Returns true if the handler was registered in memory at the time of the call, or if
    /// the persistent scheduling record was found and deleted (provider-dependent).
    /// May return false after an application restart even if a persistent record was cleaned up,
    /// when the provider cannot report whether a persistent record existed (e.g. Hangfire).
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

    /// <summary>
    /// Stops all dynamic workers and releases resources.
    /// Called during application shutdown.
    /// </summary>
    Task StopAllAsync(CancellationToken cancellationToken = default);
}
