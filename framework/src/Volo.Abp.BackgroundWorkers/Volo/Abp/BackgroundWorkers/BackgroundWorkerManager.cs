using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

    /// <summary>
    /// Initializes a new instance of the <see cref="BackgroundWorkerManager"/> class.
    /// </summary>
    public BackgroundWorkerManager()
    {
        _backgroundWorkers = new List<IBackgroundWorker>();
    }

    public virtual async Task AddAsync(IBackgroundWorker worker)
    {
        _backgroundWorkers.Add(worker);

        if (IsRunning)
        {
            await worker.StartAsync();
        }
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
