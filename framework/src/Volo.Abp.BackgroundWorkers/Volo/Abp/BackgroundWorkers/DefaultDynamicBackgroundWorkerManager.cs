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

public class DefaultDynamicBackgroundWorkerManager :
    IDynamicBackgroundWorkerManager,
    ISupportsRuntimeRegistration,
    ISingletonDependency
{
    protected IServiceProvider ServiceProvider { get; }
    public ILogger<DefaultDynamicBackgroundWorkerManager> Logger { get; set; }

    private readonly ConcurrentDictionary<string, InMemoryDynamicBackgroundWorker> _dynamicWorkers;
    private readonly SemaphoreSlim _semaphore;
    private volatile bool _isDisposed;

    public DefaultDynamicBackgroundWorkerManager(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Logger = NullLogger<DefaultDynamicBackgroundWorkerManager>.Instance;
        _dynamicWorkers = new ConcurrentDictionary<string, InMemoryDynamicBackgroundWorker>();
        _semaphore = new SemaphoreSlim(1, 1);
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

        if (!schedule.CronExpression.IsNullOrWhiteSpace())
        {
            throw new AbpException(
                $"The default in-memory background worker manager does not support CronExpression for dynamic worker '{workerName}'. " +
                "Please clear CronExpression and use Period-based scheduling, or use a scheduler-backed provider (Hangfire or Quartz).");
        }

        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(DefaultDynamicBackgroundWorkerManager));
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
        finally
        {
            _semaphore.Release();
        }
    }

    public virtual async Task<bool> RemoveAsync(string workerName, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));

        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            if (!_dynamicWorkers.TryRemove(workerName, out var worker))
            {
                return false;
            }

            await worker.StopAsync(cancellationToken);
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public virtual async Task<bool> UpdateScheduleAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));

        schedule.Validate();

        if (!schedule.CronExpression.IsNullOrWhiteSpace())
        {
            throw new AbpException(
                $"The default in-memory background worker manager does not support CronExpression for dynamic worker '{workerName}'. " +
                "Please clear CronExpression and use Period-based scheduling, or use a scheduler-backed provider (Hangfire or Quartz).");
        }

        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(DefaultDynamicBackgroundWorkerManager));
            }

            if (!_dynamicWorkers.TryGetValue(workerName, out var worker))
            {
                return false;
            }

            worker.UpdateSchedule(schedule);
            return true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public virtual bool IsRegistered(string workerName)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        return _dynamicWorkers.ContainsKey(workerName);
    }

    public virtual async Task StopAllAsync(CancellationToken cancellationToken = default)
    {
        if (_isDisposed)
        {
            return;
        }

        await _semaphore.WaitAsync(CancellationToken.None);
        try
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
                    await kvp.Value.StopAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }

            _dynamicWorkers.Clear();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    protected virtual InMemoryDynamicBackgroundWorker CreateDynamicWorker(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        DynamicBackgroundWorkerHandler handler)
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
