using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Hangfire;

namespace Volo.Abp.BackgroundWorkers.Hangfire;

[Dependency(ReplaceServices = true)]
public class HangfireDynamicBackgroundWorkerManager :
    IDynamicBackgroundWorkerManager,
    ISupportsRuntimeRegistration,
    ISupportsCronScheduling,
    ISingletonDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected IDynamicBackgroundWorkerHandlerRegistry HandlerRegistry { get; }
    public ILogger<HangfireDynamicBackgroundWorkerManager> Logger { get; set; }

    public HangfireDynamicBackgroundWorkerManager(
        IServiceProvider serviceProvider,
        IDynamicBackgroundWorkerHandlerRegistry handlerRegistry)
    {
        ServiceProvider = serviceProvider;
        HandlerRegistry = handlerRegistry;
        Logger = NullLogger<HangfireDynamicBackgroundWorkerManager>.Instance;
    }

    public virtual Task AddAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        DynamicBackgroundWorkerHandler handler,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));
        Check.NotNull(handler, nameof(handler));

        schedule.Validate();

        var cronExpression = schedule.CronExpression;
        if (cronExpression.IsNullOrWhiteSpace())
        {
            var period = schedule.Period ?? DynamicBackgroundWorkerSchedule.DefaultPeriod;
            cronExpression = GetCron(period);
        }

        // Register the handler first so it is available the moment the recurring job fires.
        HandlerRegistry.Register(workerName, handler);
        try
        {
            ScheduleRecurringJob(workerName, cronExpression, cancellationToken);
        }
        catch
        {
            HandlerRegistry.Unregister(workerName);
            throw;
        }

        return Task.CompletedTask;
    }

    public virtual Task<bool> RemoveAsync(string workerName, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));

        // Always remove the persistent recurring job regardless of in-memory registry state.
        // This ensures cleanup works correctly after an application restart, when the registry
        // is empty but the Hangfire recurring job may still exist in the database.
        var recurringJobId = $"DynamicWorker:{workerName}";
        RecurringJob.RemoveIfExists(recurringJobId);
        var wasRegistered = HandlerRegistry.Unregister(workerName);

        return Task.FromResult(wasRegistered);
    }

    public virtual Task<bool> UpdateScheduleAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));

        schedule.Validate();

        var cronExpression = schedule.CronExpression;
        if (cronExpression.IsNullOrWhiteSpace())
        {
            var period = schedule.Period ?? DynamicBackgroundWorkerSchedule.DefaultPeriod;
            cronExpression = GetCron(period);
        }

        // Always update the persistent recurring job regardless of in-memory registry state.
        // This ensures UpdateScheduleAsync works correctly after an application restart,
        // when the registry is empty but the Hangfire recurring job may still exist in the database.
        ScheduleRecurringJob(workerName, cronExpression, cancellationToken);

        return Task.FromResult(true);
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

    protected virtual void ScheduleRecurringJob(string workerName, string cronExpression, CancellationToken cancellationToken)
    {
        var abpHangfireOptions = ServiceProvider.GetRequiredService<IOptions<AbpHangfireOptions>>().Value;
        var queueName = abpHangfireOptions.DefaultQueue;
        var recurringJobId = $"DynamicWorker:{workerName}";

        if (!JobStorage.Current.HasFeature(JobStorageFeatures.JobQueueProperty))
        {
            Logger.LogWarning(
                "Current storage doesn't support specifying queues ({QueueName}) directly for a specific job. Please use the QueueAttribute instead.",
                queueName);

            RecurringJob.AddOrUpdate<HangfireDynamicBackgroundWorkerAdapter>(
                recurringJobId,
                adapter => adapter.DoWorkAsync(workerName, CancellationToken.None),
                cronExpression,
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Utc
                });
        }
        else
        {
            RecurringJob.AddOrUpdate<HangfireDynamicBackgroundWorkerAdapter>(
                recurringJobId,
                queueName,
                adapter => adapter.DoWorkAsync(workerName, CancellationToken.None),
                cronExpression,
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Utc
                });
        }
    }

    protected virtual string GetCron(int period)
    {
        var time = TimeSpan.FromMilliseconds(period);
        string cron;

        if (time.TotalSeconds <= 59)
        {
            var seconds = Math.Max(1, (int)Math.Round(time.TotalSeconds));
            cron = $"*/{seconds} * * * * *";
        }
        else if (time.TotalMinutes <= 59)
        {
            var minutes = Math.Max(1, (int)Math.Round(time.TotalMinutes));
            cron = $"*/{minutes} * * * *";
        }
        else if (time.TotalHours <= 23)
        {
            var hours = Math.Max(1, (int)Math.Round(time.TotalHours));
            cron = $"0 */{hours} * * *";
        }
        else if (time.TotalDays <= 31)
        {
            var days = Math.Max(1, (int)Math.Round(time.TotalDays));
            cron = $"0 0 0 1/{days} * *";
        }
        else
        {
            throw new AbpException($"Cannot convert period: {period} to cron expression.");
        }

        return cron;
    }
}
