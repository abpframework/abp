using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Quartz;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers.Quartz;

[Dependency(ReplaceServices = true)]
public class QuartzDynamicBackgroundWorkerManager :
    IDynamicBackgroundWorkerManager,
    ISupportsRuntimeRegistration,
    ISupportsCronScheduling,
    ISingletonDependency
{
    public const string DynamicWorkerNameKey = "AbpDynamicWorkerName";

    protected IScheduler Scheduler { get; }
    protected IDynamicBackgroundWorkerHandlerRegistry HandlerRegistry { get; }
    public ILogger<QuartzDynamicBackgroundWorkerManager> Logger { get; set; }

    public QuartzDynamicBackgroundWorkerManager(
        IScheduler scheduler,
        IDynamicBackgroundWorkerHandlerRegistry handlerRegistry)
    {
        Scheduler = scheduler;
        HandlerRegistry = handlerRegistry;
        Logger = NullLogger<QuartzDynamicBackgroundWorkerManager>.Instance;
    }

    public virtual async Task AddAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        DynamicBackgroundWorkerHandler handler,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));
        Check.NotNull(handler, nameof(handler));

        schedule.Validate();

        var jobKey = new JobKey($"DynamicWorker:{workerName}");
        var triggerKey = new TriggerKey($"DynamicWorker:{workerName}");
        var jobDetail = JobBuilder.Create<QuartzDynamicBackgroundWorkerAdapter>()
            .WithIdentity(jobKey)
            .UsingJobData(DynamicWorkerNameKey, workerName)
            .Build();

        var trigger = BuildTrigger(schedule, jobDetail, triggerKey);

        // Register the handler first so it is available the moment the job fires.
        HandlerRegistry.Register(workerName, handler);
        try
        {
            // Use replace=true to avoid TOCTOU race between CheckExists and ScheduleJob.
            await Scheduler.ScheduleJobs(
                new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>
                {
                    { jobDetail, new[] { trigger } }
                },
                replace: true,
                cancellationToken);
        }
        catch
        {
            HandlerRegistry.Unregister(workerName);
            throw;
        }
    }

    public virtual async Task<bool> RemoveAsync(string workerName, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));

        // Always delete the persistent Quartz job regardless of in-memory registry state.
        // This ensures cleanup works correctly after an application restart, when the registry
        // is empty but the Quartz job may still exist in the scheduler store.
        var jobKey = new JobKey($"DynamicWorker:{workerName}");
        var deleted = await Scheduler.DeleteJob(jobKey, cancellationToken);
        var wasRegistered = HandlerRegistry.Unregister(workerName);

        return deleted || wasRegistered;
    }

    public virtual async Task<bool> UpdateScheduleAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));

        schedule.Validate();

        var triggerKey = new TriggerKey($"DynamicWorker:{workerName}");
        var jobKey = new JobKey($"DynamicWorker:{workerName}");
        var jobDetail = JobBuilder.Create<QuartzDynamicBackgroundWorkerAdapter>()
            .WithIdentity(jobKey)
            .UsingJobData(DynamicWorkerNameKey, workerName)
            .Build();

        var trigger = BuildTrigger(schedule, jobDetail, triggerKey);

        // Always attempt to reschedule the persistent job regardless of in-memory registry state.
        // This ensures UpdateScheduleAsync works correctly after an application restart,
        // when the registry is empty but the Quartz job may still exist in the scheduler store.
        // RescheduleJob returns null if the trigger was not found, indicating the job did not exist.
        var result = await Scheduler.RescheduleJob(triggerKey, trigger, cancellationToken);
        return result != null;
    }

    public virtual bool IsRegistered(string workerName)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        return HandlerRegistry.IsRegistered(workerName);
    }

    public virtual Task StopAllAsync(CancellationToken cancellationToken = default)
    {
        HandlerRegistry.Clear();
        return Task.CompletedTask;
    }

    protected virtual ITrigger BuildTrigger(DynamicBackgroundWorkerSchedule schedule, IJobDetail jobDetail, TriggerKey triggerKey)
    {
        var triggerBuilder = TriggerBuilder.Create()
            .ForJob(jobDetail)
            .WithIdentity(triggerKey);

        if (!schedule.CronExpression.IsNullOrWhiteSpace())
        {
            triggerBuilder.WithCronSchedule(schedule.CronExpression);
        }
        else
        {
            triggerBuilder.WithSimpleSchedule(builder =>
                builder.WithInterval(TimeSpan.FromMilliseconds(schedule.Period!.Value)).RepeatForever());
        }

        return triggerBuilder.Build();
    }
}
