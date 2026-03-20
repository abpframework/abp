using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TickerQ.Utilities.Entities;
using TickerQ.Utilities.Enums;
using TickerQ.Utilities.Interfaces.Managers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.TickerQ;

namespace Volo.Abp.BackgroundWorkers.TickerQ;

[Dependency(ReplaceServices = true)]
public class TickerQDynamicBackgroundWorkerManager : IDynamicBackgroundWorkerManager, ISingletonDependency
{
    protected AbpTickerQFunctionProvider AbpTickerQFunctionProvider { get; }
    protected AbpTickerQBackgroundWorkersProvider AbpTickerQBackgroundWorkersProvider { get; }
    protected ICronTickerManager<CronTickerEntity> CronTickerManager { get; }
    protected IDynamicBackgroundWorkerHandlerRegistry HandlerRegistry { get; }
    public ILogger<TickerQDynamicBackgroundWorkerManager> Logger { get; set; }

    private readonly ConcurrentDictionary<string, Guid> _cronTickerIds;

    public TickerQDynamicBackgroundWorkerManager(
        AbpTickerQFunctionProvider abpTickerQFunctionProvider,
        AbpTickerQBackgroundWorkersProvider abpTickerQBackgroundWorkersProvider,
        ICronTickerManager<CronTickerEntity> cronTickerManager,
        IDynamicBackgroundWorkerHandlerRegistry handlerRegistry)
    {
        AbpTickerQFunctionProvider = abpTickerQFunctionProvider;
        AbpTickerQBackgroundWorkersProvider = abpTickerQBackgroundWorkersProvider;
        CronTickerManager = cronTickerManager;
        HandlerRegistry = handlerRegistry;
        Logger = NullLogger<TickerQDynamicBackgroundWorkerManager>.Instance;
        _cronTickerIds = new ConcurrentDictionary<string, Guid>();
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

        // If replacing an existing worker, delete the old cron entry first
        if (_cronTickerIds.TryRemove(workerName, out var existingId))
        {
            await CronTickerManager.DeleteAsync(existingId, cancellationToken);
        }

        var cronExpression = schedule.CronExpression ?? GetCron(schedule.Period ?? DynamicBackgroundWorkerSchedule.DefaultPeriod);
        var functionName = $"DynamicWorker:{workerName}";

        AbpTickerQFunctionProvider.Functions[functionName] =
            (string.Empty, TickerTaskPriority.LongRunning, async (tickerCancellationToken, serviceProvider, _) =>
            {
                var registeredHandler = HandlerRegistry.Get(workerName);
                if (registeredHandler == null)
                {
                    return;
                }

                try
                {
                    await registeredHandler(
                        new DynamicBackgroundWorkerExecutionContext(workerName, serviceProvider),
                        tickerCancellationToken);
                }
                catch (Exception ex)
                {
                    await serviceProvider.GetRequiredService<IExceptionNotifier>()
                        .NotifyAsync(new ExceptionNotificationContext(ex));

                    throw;
                }
            }, 0);

        AbpTickerQBackgroundWorkersProvider.BackgroundWorkers[functionName] = new AbpTickerQCronBackgroundWorker
        {
            Function = functionName,
            CronExpression = cronExpression,
            WorkerType = typeof(TickerQDynamicBackgroundWorkerManager)
        };

        var result = await CronTickerManager.AddAsync(new CronTickerEntity
        {
            Function = functionName,
            Expression = cronExpression
        }, cancellationToken);

        if (result.IsSucceeded && result.Result != null)
        {
            _cronTickerIds[workerName] = result.Result.Id;
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

        var functionName = $"DynamicWorker:{workerName}";
        AbpTickerQFunctionProvider.Functions.Remove(functionName);
        AbpTickerQBackgroundWorkersProvider.BackgroundWorkers.Remove(functionName);
        HandlerRegistry.Unregister(workerName);

        if (_cronTickerIds.TryRemove(workerName, out var cronTickerId))
        {
            await CronTickerManager.DeleteAsync(cronTickerId, cancellationToken);
        }

        return true;
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

        var cronExpression = schedule.CronExpression ?? GetCron(schedule.Period ?? DynamicBackgroundWorkerSchedule.DefaultPeriod);
        var functionName = $"DynamicWorker:{workerName}";

        if (AbpTickerQBackgroundWorkersProvider.BackgroundWorkers.TryGetValue(functionName, out var existingWorker))
        {
            existingWorker.CronExpression = cronExpression;
        }

        // Delete old entry and create new one with updated expression
        if (_cronTickerIds.TryRemove(workerName, out var oldCronTickerId))
        {
            await CronTickerManager.DeleteAsync(oldCronTickerId, cancellationToken);
        }

        var result = await CronTickerManager.AddAsync(new CronTickerEntity
        {
            Function = functionName,
            Expression = cronExpression
        }, cancellationToken);

        if (result.IsSucceeded && result.Result != null)
        {
            _cronTickerIds[workerName] = result.Result.Id;
        }

        return true;
    }

    public virtual bool IsRegistered(string workerName)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        return HandlerRegistry.IsRegistered(workerName);
    }

    protected virtual string GetCron(int period)
    {
        var time = TimeSpan.FromMilliseconds(period);
        if (time.TotalMinutes < 1)
        {
            Logger.LogWarning(
                "TickerQ does not support sub-minute intervals. Period {Period}ms will be rounded up to every minute.",
                period);
            return "* * * * *";
        }

        if (time.TotalMinutes < 60)
        {
            var minutes = (int)Math.Round(time.TotalMinutes);
            return $"*/{minutes} * * * *";
        }

        if (time.TotalHours < 24)
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
