using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DistributedLocking;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Threading;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs;

public class BackgroundJobWorker : IBackgroundJobWorker, ITransientDependency
{
    protected AbpBackgroundJobOptions JobOptions { get; }

    protected AbpBackgroundJobWorkerOptions WorkerOptions { get; }

    protected IAbpDistributedLock DistributedLock { get; }

    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected AbpAsyncTimer Timer { get; }

    public ILogger<BackgroundJobWorker> Logger { get; set; }

    protected string DistributedLockName { get; set; } = default!;

    protected BackgroundJobNameFilter JobNameFilter { get; set; } = BackgroundJobNameFilter.None;

    protected CancellationTokenSource StoppingTokenSource { get; }

    protected CancellationToken StoppingToken { get; }

    public BackgroundJobWorker(
        AbpAsyncTimer timer,
        IOptions<AbpBackgroundJobOptions> jobOptions,
        IOptions<AbpBackgroundJobWorkerOptions> workerOptions,
        IServiceScopeFactory serviceScopeFactory,
        IAbpDistributedLock distributedLock)
    {
        Timer = timer;
        DistributedLock = distributedLock;
        ServiceScopeFactory = serviceScopeFactory;
        WorkerOptions = workerOptions.Value;
        JobOptions = jobOptions.Value;
        Logger = NullLogger<BackgroundJobWorker>.Instance;

        Timer.Period = WorkerOptions.JobPollPeriod;
        Timer.Elapsed = TimerOnElapsed;

        StoppingTokenSource = new CancellationTokenSource();
        StoppingToken = StoppingTokenSource.Token;
    }

    public virtual Task StartAsync(
        string? distributedLockName = null,
        BackgroundJobNameFilter? jobNameFilter = null,
        CancellationToken cancellationToken = default)
    {
        DistributedLockName = distributedLockName ?? WorkerOptions.DistributedLockName;
        JobNameFilter = jobNameFilter ?? BackgroundJobNameFilter.None;

        Timer.Start(cancellationToken);

        return Task.CompletedTask;
    }

    public virtual Task StopAsync(CancellationToken cancellationToken = default)
    {
        StoppingTokenSource.Cancel();
        Timer.Stop(cancellationToken);
        StoppingTokenSource.Dispose();

        return Task.CompletedTask;
    }

    private async Task TimerOnElapsed(AbpAsyncTimer timer)
    {
        await RunAsync();
    }

    protected virtual async Task RunAsync()
    {
        using var scope = ServiceScopeFactory.CreateScope();

        try
        {
            var workerContext = new PeriodicBackgroundWorkerContext(scope.ServiceProvider, StoppingToken);

            if (WorkerOptions.MaxParallelJobExecutionCount > 1)
            {
                await ExecuteJobsInParallelAsync(workerContext);
            }
            else
            {
                await ExecuteJobsWithWorkerLockAsync(workerContext);
            }
        }
        catch (Exception ex)
        {
            await scope.ServiceProvider
                .GetRequiredService<IExceptionNotifier>()
                .NotifyAsync(new ExceptionNotificationContext(ex));

            Logger.LogException(ex);
        }
    }

    protected virtual async Task ExecuteJobsWithWorkerLockAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        await using (var handler = await DistributedLock.TryAcquireAsync(DistributedLockName, cancellationToken: StoppingToken))
        {
            if (handler != null)
            {
                await ExecuteWaitingJobsAsync(workerContext);
            }
            else
            {
                await WaitForNextTryAsync();
            }
        }
    }

    protected virtual async Task ExecuteWaitingJobsAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        var store = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobStore>();

        var waitingJobs = await GetWaitingJobsAsync(workerContext, store);

        if (!waitingJobs.Any())
        {
            return;
        }

        var jobExecuter = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobExecuter>();
        var clock = workerContext.ServiceProvider.GetRequiredService<IClock>();
        var serializer = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobSerializer>();

        foreach (var jobInfo in waitingJobs)
        {
            await TryExecuteJobAsync(workerContext, store, jobInfo, jobExecuter, clock, serializer);
        }
    }

    /// <summary>
    /// Executes waiting jobs in parallel across application instances, up to
    /// <see cref="AbpBackgroundJobWorkerOptions.MaxParallelJobExecutionCount"/> jobs per cycle.
    /// </summary>
    protected virtual async Task ExecuteJobsInParallelAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        var store = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobStore>();

        var waitingJobs = await GetWaitingJobsAsync(workerContext, store);

        if (!waitingJobs.Any())
        {
            return;
        }

        var runningTasks = new List<Task>();

        // Await already-started jobs even if acquiring a lock for a later job throws,
        // so no claimed job is left running detached from this cycle.
        try
        {
            foreach (var jobInfo in waitingJobs)
            {
                if (runningTasks.Count >= WorkerOptions.MaxParallelJobExecutionCount || StoppingToken.IsCancellationRequested)
                {
                    break;
                }

                var handle = await DistributedLock.TryAcquireAsync(GetPerJobDistributedLockName(jobInfo), cancellationToken: StoppingToken);
                if (handle == null)
                {
                    // Another instance is already processing this job.
                    continue;
                }

                runningTasks.Add(ExecuteClaimedJobAsync(jobInfo, handle));
            }
        }
        finally
        {
            await Task.WhenAll(runningTasks);
        }
    }

    protected virtual async Task ExecuteClaimedJobAsync(BackgroundJobInfo jobInfo, IAbpDistributedLockHandle handle)
    {
        await using (handle)
        {
            // Each concurrently executed job runs in its own service scope so that scoped services
            // (e.g. the DbContext and the unit of work) are not shared across parallel jobs.
            using var scope = ServiceScopeFactory.CreateScope();

            try
            {
                var workerContext = new PeriodicBackgroundWorkerContext(scope.ServiceProvider, StoppingToken);
                var store = scope.ServiceProvider.GetRequiredService<IBackgroundJobStore>();
                var clock = scope.ServiceProvider.GetRequiredService<IClock>();

                // Re-read under the lock: another instance may have completed, abandoned or rescheduled this job
                // between fetching the waiting list and acquiring the per-job lock.
                var currentJobInfo = await store.FindAsync(jobInfo.Id);
                if (!IsJobEligible(currentJobInfo, clock))
                {
                    return;
                }

                var jobExecuter = scope.ServiceProvider.GetRequiredService<IBackgroundJobExecuter>();
                var serializer = scope.ServiceProvider.GetRequiredService<IBackgroundJobSerializer>();

                await TryExecuteJobAsync(workerContext, store, currentJobInfo, jobExecuter, clock, serializer);
            }
            catch (Exception ex)
            {
                await scope.ServiceProvider
                    .GetRequiredService<IExceptionNotifier>()
                    .NotifyAsync(new ExceptionNotificationContext(ex));

                Logger.LogException(ex);
            }
        }
    }

    protected virtual bool IsJobEligible(BackgroundJobInfo? jobInfo, IClock clock)
    {
        return jobInfo != null &&
               jobInfo.ApplicationName == WorkerOptions.ApplicationName &&
               !jobInfo.IsAbandoned &&
               jobInfo.CompletionTime == null &&
               jobInfo.NextTryTime <= clock.Now &&
               JobNameFilter.IsMatch(jobInfo.JobName);
    }

    protected virtual string GetPerJobDistributedLockName(BackgroundJobInfo jobInfo)
    {
        return WorkerOptions.PerJobDistributedLockPrefix + jobInfo.Id;
    }

    protected virtual async Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(
        PeriodicBackgroundWorkerContext workerContext,
        IBackgroundJobStore store)
    {
        return await store.GetWaitingJobsAsync(
            WorkerOptions.ApplicationName,
            WorkerOptions.MaxJobFetchCount,
            JobNameFilter);
    }

    protected virtual async Task TryExecuteJobAsync(
        PeriodicBackgroundWorkerContext workerContext,
        IBackgroundJobStore store,
        BackgroundJobInfo jobInfo,
        IBackgroundJobExecuter jobExecuter,
        IClock clock,
        IBackgroundJobSerializer serializer)
    {
        jobInfo.TryCount++;
        jobInfo.LastTryTime = clock.Now;

        try
        {
            var jobConfiguration = JobOptions.GetJob(jobInfo.JobName);
            var jobArgs = serializer.Deserialize(jobInfo.JobArgs, jobConfiguration.ArgsType);
            var context = new JobExecutionContext(
                workerContext.ServiceProvider,
                jobConfiguration.JobType,
                jobArgs,
                workerContext.CancellationToken);

            try
            {
                await jobExecuter.ExecuteAsync(context);

                await HandleJobSuccessAsync(store, jobInfo, clock);
            }
            catch (BackgroundJobExecutionException)
            {
                await HandleJobFailureAsync(store, jobInfo, clock);
            }
        }
        catch (Exception ex)
        {
            await HandleJobErrorAsync(store, jobInfo, ex);
        }
    }

    protected virtual async Task HandleJobSuccessAsync(IBackgroundJobStore store, BackgroundJobInfo jobInfo, IClock clock)
    {
        if (WorkerOptions.StoreSuccessfulJobs)
        {
            // Keep the job as history: mark it completed instead of deleting. It is then excluded from the
            // waiting jobs query and removed later by the retention cleanup.
            jobInfo.CompletionTime = clock.Now;
            await store.UpdateAsync(jobInfo);
        }
        else
        {
            await store.DeleteAsync(jobInfo.Id);
        }
    }

    protected virtual async Task HandleJobFailureAsync(IBackgroundJobStore store, BackgroundJobInfo jobInfo, IClock clock)
    {
        var nextTryTime = CalculateNextTryTime(jobInfo, clock);

        if (nextTryTime.HasValue)
        {
            jobInfo.NextTryTime = nextTryTime.Value;
        }
        else
        {
            jobInfo.IsAbandoned = true;
        }

        await TryUpdateAsync(store, jobInfo);
    }

    protected virtual async Task HandleJobErrorAsync(IBackgroundJobStore store, BackgroundJobInfo jobInfo, Exception ex)
    {
        Logger.LogException(ex);
        jobInfo.IsAbandoned = true;
        await TryUpdateAsync(store, jobInfo);
    }

    protected virtual async Task WaitForNextTryAsync()
    {
        try
        {
            await Task.Delay(WorkerOptions.JobPollPeriod * 12, StoppingToken);
        }
        catch (TaskCanceledException) { }
    }

    protected virtual async Task TryUpdateAsync(IBackgroundJobStore store, BackgroundJobInfo jobInfo)
    {
        try
        {
            await store.UpdateAsync(jobInfo);
        }
        catch (Exception updateEx)
        {
            Logger.LogException(updateEx);
        }
    }

    protected virtual DateTime? CalculateNextTryTime(BackgroundJobInfo jobInfo, IClock clock)
    {
        var nextWaitDuration = WorkerOptions.DefaultFirstWaitDuration *
                               (Math.Pow(WorkerOptions.DefaultWaitFactor, jobInfo.TryCount - 1));
        var nextTryDate = jobInfo.LastTryTime?.AddSeconds(nextWaitDuration) ??
                          clock.Now.AddSeconds(nextWaitDuration);

        if (nextTryDate.Subtract(jobInfo.CreationTime).TotalSeconds > WorkerOptions.DefaultTimeout)
        {
            return null;
        }

        return nextTryDate;
    }
}
