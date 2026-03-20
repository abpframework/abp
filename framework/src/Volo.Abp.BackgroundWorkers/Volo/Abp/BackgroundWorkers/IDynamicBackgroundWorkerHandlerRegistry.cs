namespace Volo.Abp.BackgroundWorkers;

public interface IDynamicBackgroundWorkerHandlerRegistry
{
    void Register(string workerName, DynamicBackgroundWorkerHandler handler);

    bool Unregister(string workerName);

    bool IsRegistered(string workerName);

    DynamicBackgroundWorkerHandler? Get(string workerName);
}
