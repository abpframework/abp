using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Quartz;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers.Quartz;

[Dependency(ReplaceServices = true)]
public class QuartzDynamicBackgroundWorkerManager : IDynamicBackgroundWorkerManager, ISingletonDependency
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
        Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task> handler,
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

        if (await Scheduler.CheckExists(jobDetail.Key, cancellationToken))
        {
            await Scheduler.AddJob(jobDetail, true, true, cancellationToken);
            await Scheduler.ResumeJob(jobDetail.Key, cancellationToken);
            await Scheduler.RescheduleJob(trigger.Key, trigger, cancellationToken);
        }
        else
        {
            await Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
        }

        HandlerRegistry.Register(workerName, handler);
    }

    public virtual async Task<bool> RemoveAsync(string workerName, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));

        if (!HandlerRegistry.IsRegistered(workerName))
        {
            return false;
        }

        var jobKey = new JobKey($"DynamicWorker:{workerName}");
        var deleted = await Scheduler.DeleteJob(jobKey, cancellationToken);
        if (deleted)
        {
            HandlerRegistry.Unregister(workerName);
        }

        return deleted;
    }

    public virtual async Task<bool> UpdateScheduleAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));

        schedule.Validate();

        if (!HandlerRegistry.IsRegistered(workerName))
        {
            return false;
        }

        var triggerKey = new TriggerKey($"DynamicWorker:{workerName}");
        var jobKey = new JobKey($"DynamicWorker:{workerName}");

        var triggerBuilder = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .ForJob(jobKey);

        if (!schedule.CronExpression.IsNullOrWhiteSpace())
        {
            triggerBuilder.WithCronSchedule(schedule.CronExpression);
        }
        else
        {
            triggerBuilder.WithSimpleSchedule(builder =>
                builder.WithInterval(TimeSpan.FromMilliseconds(schedule.Period!.Value)).RepeatForever());
        }

        var result = await Scheduler.RescheduleJob(triggerKey, triggerBuilder.Build(), cancellationToken);
        return result != null;
    }

    public virtual bool IsRegistered(string workerName)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        return HandlerRegistry.IsRegistered(workerName);
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
