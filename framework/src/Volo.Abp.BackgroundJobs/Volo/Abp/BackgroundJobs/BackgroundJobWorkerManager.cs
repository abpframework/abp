using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;

namespace Volo.Abp.BackgroundJobs;

/// <summary>
/// Owns and controls the background job workers.
/// When no <see cref="AbpBackgroundJobWorkerOptions.WorkerConfigurations"/> is configured, a single
/// default worker processes all jobs. Otherwise, one dedicated worker is started per configuration
/// (each with its own distributed lock and job-type filter) plus a default worker for the remaining jobs.
/// The workers are resolved from DI, so a replaced <see cref="IBackgroundJobWorker"/> is respected.
/// </summary>
public class BackgroundJobWorkerManager : IBackgroundWorker
{
    protected AbpBackgroundJobOptions JobOptions { get; }

    protected AbpBackgroundJobWorkerOptions WorkerOptions { get; }

    protected IServiceProvider ServiceProvider { get; }

    protected List<IBackgroundJobWorker> Workers { get; }

    public BackgroundJobWorkerManager(
        IOptions<AbpBackgroundJobOptions> jobOptions,
        IOptions<AbpBackgroundJobWorkerOptions> workerOptions,
        IServiceProvider serviceProvider)
    {
        JobOptions = jobOptions.Value;
        WorkerOptions = workerOptions.Value;
        ServiceProvider = serviceProvider;
        Workers = new List<IBackgroundJobWorker>();
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken = default)
    {
        if (!JobOptions.IsJobExecutionEnabled)
        {
            return;
        }

        if (!WorkerOptions.WorkerConfigurations.Any())
        {
            await StartWorkerAsync(cancellationToken: cancellationToken);
            return;
        }

        // AddDedicatedWorker already rejects duplicate job types and lock names eagerly. This is the backstop
        // for what can only be known here: two different args types that resolve to the same job name.
        // Validate all configurations first, so a misconfiguration does not leave already-started workers running.
        var dedicatedWorkers = new List<DedicatedWorkerDefinition>();
        var allDedicatedJobNames = new List<string>();

        // The default worker uses WorkerOptions.DistributedLockName, so dedicated workers must not reuse it.
        var lockNames = new List<string> { WorkerOptions.DistributedLockName };

        foreach (var configuration in WorkerOptions.WorkerConfigurations)
        {
            var jobNames = configuration.JobArgsTypes
                .Select(GetJobName)
                .Distinct()
                .ToList();

            var alreadyConfigured = jobNames.Intersect(allDedicatedJobNames).ToList();
            if (alreadyConfigured.Any())
            {
                throw new AbpException(
                    $"The following background job(s) are configured for more than one dedicated worker: {string.Join(", ", alreadyConfigured)}. " +
                    $"Each job type can be handled by only one dedicated worker.");
            }

            if (lockNames.Contains(configuration.LockName))
            {
                throw new AbpException(
                    $"The distributed lock name '{configuration.LockName}' is used by more than one background job worker " +
                    $"(the default worker uses '{WorkerOptions.DistributedLockName}'). Each worker must have a unique lock name to run independently.");
            }

            lockNames.Add(configuration.LockName);
            allDedicatedJobNames.AddRange(jobNames);
            dedicatedWorkers.Add(new DedicatedWorkerDefinition(configuration.LockName, jobNames));
        }

        foreach (var dedicatedWorker in dedicatedWorkers)
        {
            await StartWorkerAsync(dedicatedWorker.LockName, BackgroundJobNameFilter.Include(dedicatedWorker.JobNames), cancellationToken);
        }

        // Default worker processes every job that is not handled by a dedicated worker.
        await StartWorkerAsync(null, BackgroundJobNameFilter.Exclude(allDedicatedJobNames), cancellationToken);
    }

    protected virtual string GetJobName(Type argsType)
    {
        try
        {
            return JobOptions.GetJob(argsType).JobName;
        }
        catch (AbpException ex)
        {
            throw new AbpException(
                $"No background job is registered for the args type '{argsType.FullName}' configured via AddDedicatedWorker. " +
                $"Register the job before configuring a dedicated worker for it.", ex);
        }
    }

    protected virtual async Task StartWorkerAsync(
        string? distributedLockName = null,
        BackgroundJobNameFilter? jobNameFilter = null,
        CancellationToken cancellationToken = default)
    {
        var worker = ServiceProvider.GetRequiredService<IBackgroundJobWorker>();
        await worker.StartAsync(distributedLockName, jobNameFilter, cancellationToken);
        Workers.Add(worker);
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken = default)
    {
        foreach (var worker in Workers)
        {
            await worker.StopAsync(cancellationToken);
        }

        Workers.Clear();
    }
}
