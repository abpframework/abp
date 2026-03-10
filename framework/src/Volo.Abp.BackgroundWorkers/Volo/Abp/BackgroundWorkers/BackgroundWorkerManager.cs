using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers;

/// <summary>
/// Implements <see cref="IBackgroundWorkerManager"/>.
/// </summary>
public class BackgroundWorkerManager : IBackgroundWorkerManager, ISingletonDependency, IDisposable
{
    protected bool IsRunning { get; private set; }

    private bool _isDisposed;

    private readonly List<IBackgroundWorker> _backgroundWorkers;
    private readonly Dictionary<string, InMemoryDynamicBackgroundWorker> _dynamicWorkers;
    protected IServiceProvider ServiceProvider { get; }
    protected IDynamicBackgroundWorkerHandlerRegistry DynamicBackgroundWorkerHandlerRegistry { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundWorkerManager"/> class.
    /// </summary>
    public BackgroundWorkerManager(
        IServiceProvider serviceProvider,
        IDynamicBackgroundWorkerHandlerRegistry dynamicBackgroundWorkerHandlerRegistry)
    {
        _backgroundWorkers = new List<IBackgroundWorker>();
        _dynamicWorkers = new Dictionary<string, InMemoryDynamicBackgroundWorker>();
        ServiceProvider = serviceProvider;
        DynamicBackgroundWorkerHandlerRegistry = dynamicBackgroundWorkerHandlerRegistry;
    }

    public virtual async Task AddAsync(IBackgroundWorker worker, CancellationToken cancellationToken = default)
    {
        _backgroundWorkers.Add(worker);

        if (IsRunning)
        {
            await worker.StartAsync(cancellationToken);
        }
    }

    public virtual Task AddAsync(
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

    public virtual async Task AddAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));
        Check.NotNull(handler, nameof(handler));

        DynamicBackgroundWorkerHandlerRegistry.Register(workerName, handler);

        if (schedule.Period == null && !string.IsNullOrWhiteSpace(schedule.CronExpression))
        {
            throw new AbpException("Default background worker manager does not support cron expression without period.");
        }

        var timer = ServiceProvider.GetRequiredService<AbpAsyncTimer>();
        var serviceScopeFactory = ServiceProvider.GetRequiredService<IServiceScopeFactory>();
        var worker = new InMemoryDynamicBackgroundWorker(
            workerName,
            schedule,
            timer,
            serviceScopeFactory,
            DynamicBackgroundWorkerHandlerRegistry
        );
        worker.ServiceProvider = ServiceProvider;
        worker.LazyServiceProvider = ServiceProvider.GetRequiredService<IAbpLazyServiceProvider>();

        _dynamicWorkers[workerName] = worker;

        await AddAsync(worker, cancellationToken);
    }

    public virtual async Task<bool> RemoveAsync(string workerName, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));

        if (!_dynamicWorkers.TryGetValue(workerName, out var worker))
        {
            return false;
        }

        await worker.StopAsync(cancellationToken);
        _backgroundWorkers.Remove(worker);
        _dynamicWorkers.Remove(workerName);
        DynamicBackgroundWorkerHandlerRegistry.Unregister(workerName);

        return true;
    }

    public virtual async Task<bool> UpdateScheduleAsync(string workerName, DynamicBackgroundWorkerSchedule schedule, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));

        if (!_dynamicWorkers.TryGetValue(workerName, out var oldWorker))
        {
            return false;
        }

        if (schedule.Period == null && !string.IsNullOrWhiteSpace(schedule.CronExpression))
        {
            throw new AbpException("Default background worker manager does not support cron expression without period.");
        }

        var handler = DynamicBackgroundWorkerHandlerRegistry.Get(workerName);
        if (handler == null)
        {
            return false;
        }

        await oldWorker.StopAsync(cancellationToken);
        _backgroundWorkers.Remove(oldWorker);
        _dynamicWorkers.Remove(workerName);

        var timer = ServiceProvider.GetRequiredService<AbpAsyncTimer>();
        var serviceScopeFactory = ServiceProvider.GetRequiredService<IServiceScopeFactory>();
        var newWorker = new InMemoryDynamicBackgroundWorker(
            workerName,
            schedule,
            timer,
            serviceScopeFactory,
            DynamicBackgroundWorkerHandlerRegistry
        );
        newWorker.ServiceProvider = ServiceProvider;
        newWorker.LazyServiceProvider = ServiceProvider.GetRequiredService<IAbpLazyServiceProvider>();

        _dynamicWorkers[workerName] = newWorker;

        await AddAsync(newWorker, cancellationToken);

        return true;
    }

    public virtual void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        //TODO: ???
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken = default)
    {
        IsRunning = true;

        foreach (var worker in _backgroundWorkers)
        {
            await worker.StartAsync(cancellationToken);
        }
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken = default)
    {
        IsRunning = false;

        foreach (var worker in _backgroundWorkers)
        {
            await worker.StopAsync(cancellationToken);
        }
    }
}
