using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TickerQ.Utilities.Entities;
using TickerQ.Utilities.Enums;
using TickerQ.Utilities.Interfaces.Managers;
using Volo.Abp.DependencyInjection;
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

                await registeredHandler(
                    new DynamicBackgroundWorkerExecutionContext(workerName, serviceProvider),
                    tickerCancellationToken);
            }, 0);

        AbpTickerQBackgroundWorkersProvider.BackgroundWorkers[functionName] = new AbpTickerQCronBackgroundWorker
        {
            Function = functionName,
            CronExpression = cronExpression,
            WorkerType = typeof(TickerQDynamicBackgroundWorkerManager)
        };

        await CronTickerManager.AddAsync(new CronTickerEntity
        {
            Function = functionName,
            Expression = cronExpression
        });

        HandlerRegistry.Register(workerName, handler);
    }

    public virtual Task<bool> RemoveAsync(string workerName, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));

        if (!HandlerRegistry.IsRegistered(workerName))
        {
            return Task.FromResult(false);
        }

        var functionName = $"DynamicWorker:{workerName}";
        AbpTickerQFunctionProvider.Functions.Remove(functionName);
        AbpTickerQBackgroundWorkersProvider.BackgroundWorkers.Remove(functionName);
        HandlerRegistry.Unregister(workerName);

        return Task.FromResult(true);
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

        await CronTickerManager.AddAsync(new CronTickerEntity
        {
            Function = functionName,
            Expression = cronExpression
        });

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
