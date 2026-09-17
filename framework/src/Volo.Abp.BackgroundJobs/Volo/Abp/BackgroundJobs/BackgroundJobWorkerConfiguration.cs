using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.BackgroundJobs;

/// <summary>
/// Configuration of a dedicated <see cref="BackgroundJobWorker"/> that processes only specific job types.
/// </summary>
public class BackgroundJobWorkerConfiguration
{
    /// <summary>
    /// A unique distributed lock name for this worker. It must be different from the names used by other workers.
    /// It is used to serialize the worker across application instances when
    /// <see cref="AbpBackgroundJobWorkerOptions.MaxParallelJobExecutionCount"/> is 1; in parallel mode
    /// (greater than 1) jobs are claimed with per-job locks instead and this lock is not acquired.
    /// </summary>
    public string LockName { get; }

    /// <summary>
    /// The job argument types that are processed exclusively by this worker.
    /// </summary>
    public IReadOnlyList<Type> JobArgsTypes { get; }

    public BackgroundJobWorkerConfiguration(string lockName, params Type[] jobArgsTypes)
    {
        LockName = Check.NotNullOrWhiteSpace(lockName, nameof(lockName));
        Check.NotNullOrEmpty(jobArgsTypes, nameof(jobArgsTypes));

        if (jobArgsTypes.Any(t => t == null))
        {
            throw new ArgumentException("Job args types cannot contain null.", nameof(jobArgsTypes));
        }

        JobArgsTypes = jobArgsTypes.ToList();
    }
}
