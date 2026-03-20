using System.Collections.Concurrent;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs;

public class DynamicBackgroundJobHandlerRegistry : IDynamicBackgroundJobHandlerRegistry, ISingletonDependency
{
    protected ConcurrentDictionary<string, DynamicBackgroundJobHandler> Handlers { get; }

    public DynamicBackgroundJobHandlerRegistry()
    {
        Handlers = new ConcurrentDictionary<string, DynamicBackgroundJobHandler>();
    }

    public virtual void Register(string jobName, DynamicBackgroundJobHandler handler)
    {
        Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        Check.NotNull(handler, nameof(handler));

        Handlers[jobName] = handler;
    }

    public virtual bool Unregister(string jobName)
    {
        Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        return Handlers.TryRemove(jobName, out _);
    }

    public virtual bool IsRegistered(string jobName)
    {
        Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        return Handlers.ContainsKey(jobName);
    }

    public virtual DynamicBackgroundJobHandler? Get(string jobName)
    {
        Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        return Handlers.TryGetValue(jobName, out var handler) ? handler : null;
    }
}
