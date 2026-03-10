using System;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs;

public interface IAnonymousJobHandlerRegistry
{
    void Register(string jobName, Func<AnonymousJobExecutionContext, System.Threading.CancellationToken, Task> handler);

    void Register(string jobName, Action<AnonymousJobExecutionContext, System.Threading.CancellationToken> handler);

    bool Unregister(string jobName);

    bool IsRegistered(string jobName);

    Func<AnonymousJobExecutionContext, System.Threading.CancellationToken, Task>? Get(string jobName);
}
