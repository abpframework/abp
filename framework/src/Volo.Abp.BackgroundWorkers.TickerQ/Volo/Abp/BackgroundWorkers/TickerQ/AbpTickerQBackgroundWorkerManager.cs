using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TickerQ.Utilities.Entities;
using TickerQ.Utilities.Enums;
using TickerQ.Utilities.Interfaces.Managers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.TickerQ;

namespace Volo.Abp.BackgroundWorkers.TickerQ;

[Dependency(ReplaceServices = true)]
public class AbpTickerQBackgroundWorkerManager : BackgroundWorkerManager, ISingletonDependency
{
    protected AbpTickerQFunctionProvider AbpTickerQFunctionProvider { get; }
    protected AbpTickerQBackgroundWorkersProvider AbpTickerQBackgroundWorkersProvider { get; }
    protected ICronTickerManager<CronTickerEntity> CronTickerManager { get; }
    protected AbpBackgroundWorkersTickerQOptions Options { get; }

    public AbpTickerQBackgroundWorkerManager(
        AbpTickerQFunctionProvider abpTickerQFunctionProvider,
        AbpTickerQBackgroundWorkersProvider abpTickerQBackgroundWorkersProvider,
        ICronTickerManager<CronTickerEntity> cronTickerManager,
        IServiceProvider serviceProvider,
        IDynamicBackgroundWorkerHandlerRegistry dynamicBackgroundWorkerHandlerRegistry,
        IOptions<AbpBackgroundWorkersTickerQOptions> options)
        : base(serviceProvider, dynamicBackgroundWorkerHandlerRegistry)
    {
        AbpTickerQFunctionProvider = abpTickerQFunctionProvider;
        AbpTickerQBackgroundWorkersProvider = abpTickerQBackgroundWorkersProvider;
        CronTickerManager = cronTickerManager;
        Options = options.Value;
    }

    public override async Task AddAsync(IBackgroundWorker worker, CancellationToken cancellationToken = default)
    {
        if (worker is AsyncPeriodicBackgroundWorkerBase or PeriodicBackgroundWorkerBase)
        {
            int? period = null;
            string? cronExpression = null;

            if (worker is AsyncPeriodicBackgroundWorkerBase asyncPeriodicBackgroundWorkerBase)
            {
                period = asyncPeriodicBackgroundWorkerBase.Period;
                cronExpression = asyncPeriodicBackgroundWorkerBase.CronExpression;
            }
            else if (worker is PeriodicBackgroundWorkerBase periodicBackgroundWorkerBase)
            {
                period = periodicBackgroundWorkerBase.Period;
                cronExpression = periodicBackgroundWorkerBase.CronExpression;
            }

            if (period == null && cronExpression.IsNullOrWhiteSpace())
            {
                throw new AbpException($"Both 'Period' and 'CronExpression' are not set for {worker.GetType().FullName}. You must set at least one of them.");
            }

            cronExpression = cronExpression ?? GetCron(period!.Value);
            var name = BackgroundWorkerNameAttribute.GetNameOrNull(worker.GetType()) ?? worker.GetType().FullName;

            var config = Options.GetConfigurationOrNull(ProxyHelper.GetUnProxiedType(worker));
            AbpTickerQFunctionProvider.Functions.TryAdd(name!, (string.Empty, config?.Priority ?? TickerTaskPriority.LongRunning, async (tickerQCancellationToken, serviceProvider, tickerFunctionContext) =>
            {
                var workerInvoker = new AbpTickerQPeriodicBackgroundWorkerInvoker(worker, serviceProvider);
                await workerInvoker.DoWorkAsync(tickerFunctionContext, tickerQCancellationToken);
            }));

            AbpTickerQBackgroundWorkersProvider.BackgroundWorkers.Add(name!, new AbpTickerQCronBackgroundWorker
            {
                Function = name!,
                CronExpression = cronExpression,
                WorkerType = ProxyHelper.GetUnProxiedType(worker)
            });
        }

        await base.AddAsync(worker, cancellationToken);
    }

    public override Task AddAsync(
        string workerName,
        Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default)
    {
        return AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule
            {
                Period = DynamicBackgroundWorkerSchedule.DefaultPeriod
            },
            handler,
            cancellationToken
        );
    }

    public override async Task AddAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));
        Check.NotNull(handler, nameof(handler));

        DynamicBackgroundWorkerHandlerRegistry.Register(workerName, handler);

        var cronExpression = schedule.CronExpression ?? GetCron(schedule.Period ?? DynamicBackgroundWorkerSchedule.DefaultPeriod);
        var functionName = $"DynamicWorker:{workerName}";

        AbpTickerQFunctionProvider.Functions[functionName] =
            (string.Empty, TickerTaskPriority.LongRunning, async (tickerCancellationToken, serviceProvider, _) =>
            {
                var registeredHandler = DynamicBackgroundWorkerHandlerRegistry.Get(workerName);
                if (registeredHandler == null)
                {
                    return;
                }

                await registeredHandler(
                    new DynamicBackgroundWorkerExecutionContext(workerName, serviceProvider),
                    tickerCancellationToken
                );
            });

        AbpTickerQBackgroundWorkersProvider.BackgroundWorkers[functionName] = new AbpTickerQCronBackgroundWorker
        {
            Function = functionName,
            CronExpression = cronExpression,
            WorkerType = typeof(AbpTickerQBackgroundWorkerManager)
        };

        await CronTickerManager.AddAsync(new CronTickerEntity
        {
            Function = functionName,
            Expression = cronExpression
        });
    }

    public override async Task<bool> RemoveAsync(string workerName, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));

        if (!DynamicBackgroundWorkerHandlerRegistry.IsRegistered(workerName))
        {
            return false;
        }

        var functionName = $"DynamicWorker:{workerName}";
        AbpTickerQFunctionProvider.Functions.Remove(functionName);
        AbpTickerQBackgroundWorkersProvider.BackgroundWorkers.Remove(functionName);
        DynamicBackgroundWorkerHandlerRegistry.Unregister(workerName);

        return true;
    }

    public override async Task<bool> UpdateScheduleAsync(string workerName, DynamicBackgroundWorkerSchedule schedule, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));

        if (!DynamicBackgroundWorkerHandlerRegistry.IsRegistered(workerName))
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

    protected virtual string GetCron(int period)
    {
        var time = TimeSpan.FromMilliseconds(period);
        if (time.TotalMinutes < 1)
        {
            // Less than 1 minute — 5-field cron doesn't support seconds, so run every minute
            return "* * * * *";
        }

        if (time.TotalMinutes < 60)
        {
            // Run every N minutes
            var minutes = (int)Math.Round(time.TotalMinutes);
            return $"*/{minutes} * * * *";
        }

        if (time.TotalHours < 24)
        {
            // Run every N hours
            var hours = (int)Math.Round(time.TotalHours);
            return $"0 */{hours} * * *";
        }

        if (time.TotalDays <= 31)
        {
            // Run every N days
            var days = (int)Math.Round(time.TotalDays);
            return $"0 0 */{days} * *";
        }

        throw new AbpException($"Cannot convert period: {period} to cron expression.");
    }
}
