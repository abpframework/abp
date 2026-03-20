using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers;

public class DefaultDynamicBackgroundWorkerManager : IDynamicBackgroundWorkerManager, ISingletonDependency, IDisposable
{
    protected IServiceProvider ServiceProvider { get; }
    public ILogger<DefaultDynamicBackgroundWorkerManager> Logger { get; set; }

    private readonly ConcurrentDictionary<string, InMemoryDynamicBackgroundWorker> _dynamicWorkers;
    private bool _isDisposed;

    public DefaultDynamicBackgroundWorkerManager(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Logger = NullLogger<DefaultDynamicBackgroundWorkerManager>.Instance;
        _dynamicWorkers = new ConcurrentDictionary<string, InMemoryDynamicBackgroundWorker>();
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

        if (schedule.Period == null)
        {
            throw new AbpException(
                $"The default in-memory background worker manager does not support CronExpression without Period for dynamic worker '{workerName}'. " +
                "Please set Period, or use a scheduler-backed provider (Hangfire, Quartz, TickerQ).");
        }

        if (_dynamicWorkers.TryRemove(workerName, out var existingWorker))
        {
            await existingWorker.StopAsync(cancellationToken);
            Logger.LogInformation("Replaced existing dynamic worker: {WorkerName}", workerName);
        }

        var worker = CreateDynamicWorker(workerName, schedule, handler);
        _dynamicWorkers[workerName] = worker;

        await worker.StartAsync(cancellationToken);
    }

    public virtual async Task<bool> RemoveAsync(string workerName, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));

        if (!_dynamicWorkers.TryRemove(workerName, out var worker))
        {
            return false;
        }

        await worker.StopAsync(cancellationToken);
        return true;
    }

    public virtual Task<bool> UpdateScheduleAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));

        schedule.Validate();

        if (schedule.Period == null)
        {
            throw new AbpException(
                $"The default in-memory background worker manager does not support CronExpression without Period for dynamic worker '{workerName}'. " +
                "Please set Period, or use a scheduler-backed provider (Hangfire, Quartz, TickerQ).");
        }

        if (!_dynamicWorkers.TryGetValue(workerName, out var worker))
        {
            return Task.FromResult(false);
        }

        worker.UpdateSchedule(schedule);
        return Task.FromResult(true);
    }

    public virtual bool IsRegistered(string workerName)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        return _dynamicWorkers.ContainsKey(workerName);
    }

    public virtual void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        foreach (var kvp in _dynamicWorkers)
        {
            try
            {
                kvp.Value.StopAsync(CancellationToken.None).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        _dynamicWorkers.Clear();
    }

    protected virtual InMemoryDynamicBackgroundWorker CreateDynamicWorker(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task> handler)
    {
        var timer = ServiceProvider.GetRequiredService<AbpAsyncTimer>();
        var serviceScopeFactory = ServiceProvider.GetRequiredService<IServiceScopeFactory>();

        var worker = new InMemoryDynamicBackgroundWorker(
            workerName, schedule, handler, timer, serviceScopeFactory);

        worker.ServiceProvider = ServiceProvider;
        worker.LazyServiceProvider = ServiceProvider.GetRequiredService<IAbpLazyServiceProvider>();

        return worker;
    }
}
