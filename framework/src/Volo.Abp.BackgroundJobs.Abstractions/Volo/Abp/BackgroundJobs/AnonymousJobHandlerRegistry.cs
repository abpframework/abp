using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs;

public class AnonymousJobHandlerRegistry : IAnonymousJobHandlerRegistry, ISingletonDependency
{
    private readonly ConcurrentDictionary<string, Func<AnonymousJobExecutionContext, CancellationToken, Task>> _handlers = new();
    private readonly AbpBackgroundJobOptions _options;

    public AnonymousJobHandlerRegistry(IOptions<AbpBackgroundJobOptions> options)
    {
        _options = options.Value;
    }

    public virtual void Register(string jobName, Func<AnonymousJobExecutionContext, CancellationToken, Task> handler)
    {
        Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        Check.NotNull(handler, nameof(handler));

        _handlers[jobName] = handler;
    }

    public virtual void Register(string jobName, Action<AnonymousJobExecutionContext, CancellationToken> handler)
    {
        Check.NotNull(handler, nameof(handler));

        Register(jobName, (context, ct) =>
        {
            handler(context, ct);
            return Task.CompletedTask;
        });
    }

    public virtual bool Unregister(string jobName)
    {
        Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        return _handlers.TryRemove(jobName, out _);
    }

    public virtual bool IsRegistered(string jobName)
    {
        Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        return _handlers.ContainsKey(jobName) || _options.IsAnonymousJobRegistered(jobName);
    }

    public virtual Func<AnonymousJobExecutionContext, CancellationToken, Task>? Get(string jobName)
    {
        Check.NotNullOrWhiteSpace(jobName, nameof(jobName));

        if (_handlers.TryGetValue(jobName, out var handler))
        {
            return handler;
        }

        return _options.TryGetAnonymousHandler(jobName, out handler) ? handler : null;
    }
}
