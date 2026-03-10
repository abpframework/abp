using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.BackgroundJobs;

public interface IAnonymousJobHandlerRegistry
{
    void Register(string jobName, Func<string, IServiceProvider, System.Threading.CancellationToken, Task> handler);

    void Register(string jobName, Action<string, IServiceProvider, System.Threading.CancellationToken> handler);

    bool Unregister(string jobName);

    bool IsRegistered(string jobName);

    Func<string, IServiceProvider, System.Threading.CancellationToken, Task>? Get(string jobName);
}
