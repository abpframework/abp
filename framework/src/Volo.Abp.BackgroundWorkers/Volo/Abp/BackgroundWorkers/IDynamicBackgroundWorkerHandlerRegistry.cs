using System;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundWorkers;

public interface IDynamicBackgroundWorkerHandlerRegistry
{
    void Register(string workerName, Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task> handler);

    bool Unregister(string workerName);

    bool IsRegistered(string workerName);

    Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task>? Get(string workerName);
}
