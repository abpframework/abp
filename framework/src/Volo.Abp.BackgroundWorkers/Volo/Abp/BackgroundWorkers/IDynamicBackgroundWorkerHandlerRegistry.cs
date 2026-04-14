using System.Collections.Generic;

namespace Volo.Abp.BackgroundWorkers;

public interface IDynamicBackgroundWorkerHandlerRegistry
{
    void Register(string workerName, DynamicBackgroundWorkerHandler handler);

    bool Unregister(string workerName);

    bool IsRegistered(string workerName);

    DynamicBackgroundWorkerHandler? Get(string workerName);

    IReadOnlyCollection<string> GetAllNames();

    void Clear();
}
