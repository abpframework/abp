using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.BackgroundJobs;

public class AbpBackgroundJobWorkerOptions
{
    /// <summary>
    /// Application name.
    /// </summary>
    public string? ApplicationName { get; set; }

    /// <summary>
    /// Interval between polling jobs from <see cref="IBackgroundJobStore"/>.
    /// Default value: 5000 (5 seconds).
    /// </summary>
    public int JobPollPeriod { get; set; }

    /// <summary>
    /// Maximum count of jobs to fetch from data store in one loop.
    /// Also used as the batch size for the retention cleanup deletions (see <see cref="BackgroundJobCleanupWorker"/>).
    /// Default: 1000.
    /// </summary>
    public int MaxJobFetchCount { get; set; }

    /// <summary>
    /// Default duration (as seconds) for the first wait on a failure.
    /// Default value: 60 (1 minutes).
    /// </summary>
    public int DefaultFirstWaitDuration { get; set; }

    /// <summary>
    /// Default timeout value (as seconds) for a job before it's abandoned (<see cref="BackgroundJobInfo.IsAbandoned"/>).
    /// Default value: 172,800 (2 days).
    /// </summary>
    public int DefaultTimeout { get; set; }

    /// <summary>
    /// Default wait factor for execution failures.
    /// This amount is multiplated by last wait time to calculate next wait time.
    /// Default value: 2.0.
    /// </summary>
    public double DefaultWaitFactor { get; set; }

    /// <summary>
    /// Distributed lock name for the worker.
    /// Default value: "AbpBackgroundJobWorker".
    /// </summary>
    public string DistributedLockName { get; set; }

    /// <summary>
    /// When set to true, a successfully completed job is kept (its <see cref="BackgroundJobInfo.CompletionTime"/> is set)
    /// instead of being deleted, so it can be used for auditing/history. Completed jobs are excluded from the
    /// waiting jobs query and are removed by the retention cleanup (see <see cref="SuccessfulJobRetentionTime"/>).
    /// Default value: false (successful jobs are deleted).
    /// </summary>
    public bool StoreSuccessfulJobs { get; set; }

    /// <summary>
    /// How long a kept (successfully completed) job is retained before the cleanup deletes it.
    /// Only relevant when <see cref="StoreSuccessfulJobs"/> is true. Set to null to keep them forever (no automatic cleanup).
    /// Default value: 7 days.
    /// </summary>
    public TimeSpan? SuccessfulJobRetentionTime { get; set; }

    /// <summary>
    /// Interval (as milliseconds) between cleanup runs that delete retained successful jobs older than <see cref="SuccessfulJobRetentionTime"/>.
    /// Default value: 3,600,000 (1 hour).
    /// </summary>
    public int CleanSuccessfulJobsPeriod { get; set; }

    /// <summary>
    /// Distributed lock name for the cleanup worker.
    /// Default value: "AbpBackgroundJobCleanup".
    /// </summary>
    public string CleanupDistributedLockName { get; set; }

    /// <summary>
    /// Dedicated workers, each processing only the configured job types with its own distributed lock.
    /// When this list is not empty, an additional default worker is started that processes all
    /// other job types. When it is empty, a single default worker processes all job types (the default behavior).
    /// Use <see cref="AddDedicatedWorker(string, Type[])"/> to add a dedicated worker.
    /// </summary>
    public List<BackgroundJobWorkerConfiguration> WorkerConfigurations { get; }

    /// <summary>
    /// Maximum number of jobs that a single worker executes in parallel within one poll cycle.
    /// When it is 1 (default), jobs are executed sequentially under a single worker distributed lock
    /// (the default behavior). When greater than 1, the worker distributed lock is not used; instead
    /// each job is claimed with its own distributed lock so multiple application instances can execute
    /// different jobs concurrently.
    /// This value should be configured consistently across all application instances: mixing sequential
    /// (worker lock) and parallel (per-job lock) instances removes the common mutual exclusion and may
    /// let the same job run on more than one instance.
    /// Default value: 1.
    /// </summary>
    public int MaxParallelJobExecutionCount { get; set; }

    /// <summary>
    /// Prefix of the per-job distributed lock name used when <see cref="MaxParallelJobExecutionCount"/> is greater than 1.
    /// Default value: "AbpBackgroundJob:".
    /// </summary>
    public string PerJobDistributedLockPrefix { get; set; }

    public AbpBackgroundJobWorkerOptions()
    {
        MaxJobFetchCount = 1000;
        JobPollPeriod = 5000;
        DefaultFirstWaitDuration = 60;
        DefaultTimeout = 172800;
        DefaultWaitFactor = 2.0;
        DistributedLockName = "AbpBackgroundJobWorker";
        WorkerConfigurations = new List<BackgroundJobWorkerConfiguration>();
        MaxParallelJobExecutionCount = 1;
        PerJobDistributedLockPrefix = "AbpBackgroundJob:";
        SuccessfulJobRetentionTime = TimeSpan.FromDays(7);
        CleanSuccessfulJobsPeriod = 3600000;
        CleanupDistributedLockName = "AbpBackgroundJobCleanup";
    }

    /// <summary>
    /// Adds a dedicated worker that processes only the given job argument types.
    /// </summary>
    /// <param name="lockName">A unique distributed lock name for this worker.</param>
    /// <param name="jobArgsTypes">The job argument types handled exclusively by this worker.</param>
    public AbpBackgroundJobWorkerOptions AddDedicatedWorker(string lockName, params Type[] jobArgsTypes)
    {
        Check.NotNullOrEmpty(jobArgsTypes, nameof(jobArgsTypes));

        var configuration = new BackgroundJobWorkerConfiguration(lockName, jobArgsTypes.Distinct().ToArray());

        var duplicateType = configuration.JobArgsTypes.FirstOrDefault(
            type => WorkerConfigurations.Any(c => c.JobArgsTypes.Contains(type)));
        if (duplicateType != null)
        {
            throw new AbpException(
                $"The background job args type '{duplicateType.FullName}' is already assigned to a dedicated worker. " +
                $"Each job type can be handled by only one dedicated worker.");
        }

        if (lockName == DistributedLockName || WorkerConfigurations.Any(c => c.LockName == lockName))
        {
            throw new AbpException(
                $"The distributed lock name '{lockName}' is already used by another background job worker. " +
                $"Each worker must have a unique lock name.");
        }

        WorkerConfigurations.Add(configuration);
        return this;
    }

    public AbpBackgroundJobWorkerOptions AddDedicatedWorker<TArgs>(string lockName)
    {
        return AddDedicatedWorker(lockName, typeof(TArgs));
    }

    public AbpBackgroundJobWorkerOptions AddDedicatedWorker<TArgs1, TArgs2>(string lockName)
    {
        return AddDedicatedWorker(lockName, typeof(TArgs1), typeof(TArgs2));
    }

    public AbpBackgroundJobWorkerOptions AddDedicatedWorker<TArgs1, TArgs2, TArgs3>(string lockName)
    {
        return AddDedicatedWorker(lockName, typeof(TArgs1), typeof(TArgs2), typeof(TArgs3));
    }

    /// <summary>
    /// Adds a dedicated worker with a lock name derived from the given job argument type names.
    /// Use the <c>AddDedicatedWorker(string lockName, ...)</c> overloads to set an explicit lock name.
    /// </summary>
    /// <param name="jobArgsTypes">The job argument types handled exclusively by this worker.</param>
    public AbpBackgroundJobWorkerOptions AddDedicatedWorker(params Type[] jobArgsTypes)
    {
        return AddDedicatedWorker(GetDedicatedWorkerLockName(jobArgsTypes), jobArgsTypes);
    }

    public AbpBackgroundJobWorkerOptions AddDedicatedWorker<TArgs>()
    {
        return AddDedicatedWorker(typeof(TArgs));
    }

    public AbpBackgroundJobWorkerOptions AddDedicatedWorker<TArgs1, TArgs2>()
    {
        return AddDedicatedWorker(typeof(TArgs1), typeof(TArgs2));
    }

    public AbpBackgroundJobWorkerOptions AddDedicatedWorker<TArgs1, TArgs2, TArgs3>()
    {
        return AddDedicatedWorker(typeof(TArgs1), typeof(TArgs2), typeof(TArgs3));
    }

    protected virtual string GetDedicatedWorkerLockName(Type[] jobArgsTypes)
    {
        Check.NotNullOrEmpty(jobArgsTypes, nameof(jobArgsTypes));

        if (jobArgsTypes.Any(t => t == null))
        {
            throw new ArgumentException("Job args types cannot contain null.", nameof(jobArgsTypes));
        }

        // Hash the (stable, sorted) full type names so the derived lock name stays short and within the
        // length limits of distributed lock providers (e.g. SQL Server sp_getapplock is limited to 255 chars).
        var key = string.Join(",", jobArgsTypes.Select(t => t.FullName).Distinct().OrderBy(n => n, StringComparer.Ordinal));
        return "AbpBackgroundJobDedicatedWorker:" + key.ToMd5();
    }
}
