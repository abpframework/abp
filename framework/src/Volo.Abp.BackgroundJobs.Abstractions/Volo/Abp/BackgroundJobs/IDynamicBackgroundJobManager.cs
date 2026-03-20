using System;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs;

/// <summary>
/// Manages dynamic background jobs that can be enqueued by name
/// without requiring a strongly-typed job args class at compile time.
/// Also supports registering dynamic job handlers at runtime.
/// </summary>
public interface IDynamicBackgroundJobManager
{
    /// <summary>
    /// Enqueues a job by name with dynamic payload.
    /// If a typed job configuration exists for this name, the args will be
    /// deserialized to the configured args type and enqueued through the typed pipeline.
    /// If a dynamic handler is registered, the args will be wrapped as
    /// <see cref="DynamicBackgroundJobArgs"/> and enqueued through the standard typed pipeline.
    /// </summary>
    /// <param name="jobName">Name of the background job.</param>
    /// <param name="args">Job arguments (will be serialized to JSON).</param>
    /// <param name="priority">Job priority.</param>
    /// <param name="delay">Job delay (wait duration before first try).</param>
    /// <returns>Unique identifier of a background job.</returns>
    Task<string> EnqueueAsync(
        string jobName,
        object args,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null);

    /// <summary>
    /// Registers a dynamic job handler at runtime.
    /// </summary>
    /// <param name="jobName">Unique name for the dynamic job.</param>
    /// <param name="handler">The handler delegate to execute when the job runs.</param>
    void RegisterHandler(string jobName, DynamicBackgroundJobHandler handler);

    /// <summary>
    /// Unregisters a previously registered dynamic job handler.
    /// </summary>
    /// <param name="jobName">Name of the dynamic job to unregister.</param>
    /// <returns>True if the handler was found and removed; false otherwise.</returns>
    bool UnregisterHandler(string jobName);

    /// <summary>
    /// Checks whether a dynamic handler is registered for the given job name.
    /// </summary>
    /// <param name="jobName">Name of the dynamic job.</param>
    /// <returns>True if registered; false otherwise.</returns>
    bool IsHandlerRegistered(string jobName);
}
