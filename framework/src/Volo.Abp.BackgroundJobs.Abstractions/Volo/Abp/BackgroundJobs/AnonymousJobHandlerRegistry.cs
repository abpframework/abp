using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs;

public class AnonymousJobHandlerRegistry : IAnonymousJobHandlerRegistry, ISingletonDependency
{
    private readonly ConcurrentDictionary<string, Func<string, IServiceProvider, CancellationToken, Task>> _handlers = new();
    private readonly AbpBackgroundJobOptions _options;

    public AnonymousJobHandlerRegistry(IOptions<AbpBackgroundJobOptions> options)
    {
        _options = options.Value;
    }

    public virtual void Register(string jobName, Func<string, IServiceProvider, CancellationToken, Task> handler)
    {
        Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        Check.NotNull(handler, nameof(handler));

        _handlers[jobName] = handler;
    }

    public virtual void Register(string jobName, Action<string, IServiceProvider, CancellationToken> handler)
    {
        Register(jobName, (jsonData, sp, ct) =>
        {
            handler(jsonData, sp, ct);
            return Task.CompletedTask;
        });
    }

    public virtual bool Unregister(string jobName)
    {
        return _handlers.TryRemove(jobName, out _);
    }

    public virtual bool IsRegistered(string jobName)
    {
        return _handlers.ContainsKey(jobName) || _options.IsAnonymousJobRegistered(jobName);
    }

    public virtual Func<string, IServiceProvider, CancellationToken, Task>? Get(string jobName)
    {
        if (_handlers.TryGetValue(jobName, out var handler))
        {
            return handler;
        }

        return _options.TryGetAnonymousHandler(jobName, out handler) ? handler : null;
    }
}
