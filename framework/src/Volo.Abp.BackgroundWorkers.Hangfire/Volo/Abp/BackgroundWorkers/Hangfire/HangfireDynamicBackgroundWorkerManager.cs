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
public class HangfireDynamicBackgroundWorkerManager : IDynamicBackgroundWorkerManager, ISingletonDependency
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
        Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task> handler,
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

        ScheduleRecurringJob(workerName, cronExpression, cancellationToken);
        HandlerRegistry.Register(workerName, handler);

        return Task.CompletedTask;
    }

    public virtual Task<bool> RemoveAsync(string workerName, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));

        if (!HandlerRegistry.IsRegistered(workerName))
        {
            return Task.FromResult(false);
        }

        var recurringJobId = $"DynamicWorker:{workerName}";
        RecurringJob.RemoveIfExists(recurringJobId);
        HandlerRegistry.Unregister(workerName);

        return Task.FromResult(true);
    }

    public virtual Task<bool> UpdateScheduleAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));

        schedule.Validate();

        if (!HandlerRegistry.IsRegistered(workerName))
        {
            return Task.FromResult(false);
        }

        var cronExpression = schedule.CronExpression;
        if (cronExpression.IsNullOrWhiteSpace())
        {
            var period = schedule.Period ?? DynamicBackgroundWorkerSchedule.DefaultPeriod;
            cronExpression = GetCron(period);
        }

        ScheduleRecurringJob(workerName, cronExpression, cancellationToken);

        return Task.FromResult(true);
    }

    public virtual bool IsRegistered(string workerName)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        return HandlerRegistry.IsRegistered(workerName);
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

        if (time.TotalSeconds <= 59)
        {
            var seconds = (int)Math.Round(time.TotalSeconds);
            return $"*/{seconds} * * * * *";
        }

        if (time.TotalMinutes <= 59)
        {
            var minutes = (int)Math.Round(time.TotalMinutes);
            return $"*/{minutes} * * * *";
        }

        if (time.TotalHours <= 23)
        {
            var hours = (int)Math.Round(time.TotalHours);
            return $"0 */{hours} * * *";
        }

        if (time.TotalDays <= 31)
        {
            var days = (int)Math.Round(time.TotalDays);
            return $"0 0 */{days} * *";
        }

        throw new AbpException($"Cannot convert period: {period} to cron expression.");
    }
}
