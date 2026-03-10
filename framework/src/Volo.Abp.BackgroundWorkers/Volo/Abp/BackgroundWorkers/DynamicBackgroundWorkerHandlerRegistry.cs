using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers;

public class DynamicBackgroundWorkerHandlerRegistry : IDynamicBackgroundWorkerHandlerRegistry, ISingletonDependency
{
    protected ConcurrentDictionary<string, Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task>> Handlers { get; }

    public DynamicBackgroundWorkerHandlerRegistry()
    {
        Handlers = new ConcurrentDictionary<string, Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task>>();
    }

    public virtual void Register(string workerName, Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task> handler)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(handler, nameof(handler));

        Handlers[workerName] = handler;
    }

    public virtual bool Unregister(string workerName)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        return Handlers.TryRemove(workerName, out _);
    }

    public virtual bool IsRegistered(string workerName)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        return Handlers.ContainsKey(workerName);
    }

    public virtual Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task>? Get(string workerName)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        return Handlers.TryGetValue(workerName, out var handler) ? handler : null;
    }
}
