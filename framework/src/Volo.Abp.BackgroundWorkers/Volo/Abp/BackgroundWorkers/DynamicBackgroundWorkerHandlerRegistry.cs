using System.Collections.Concurrent;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers;

public class DynamicBackgroundWorkerHandlerRegistry : IDynamicBackgroundWorkerHandlerRegistry, ISingletonDependency
{
    protected ConcurrentDictionary<string, DynamicBackgroundWorkerHandler> Handlers { get; }

    public DynamicBackgroundWorkerHandlerRegistry()
    {
        Handlers = new ConcurrentDictionary<string, DynamicBackgroundWorkerHandler>();
    }

    public virtual void Register(string workerName, DynamicBackgroundWorkerHandler handler)
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

    public virtual DynamicBackgroundWorkerHandler? Get(string workerName)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        return Handlers.TryGetValue(workerName, out var handler) ? handler : null;
    }
}
