using System.Collections.Generic;

namespace Volo.Abp.BackgroundJobs;

public interface IDynamicBackgroundJobHandlerRegistry
{
    void Register(string jobName, DynamicBackgroundJobHandler handler);

    bool Unregister(string jobName);

    bool IsRegistered(string jobName);

    DynamicBackgroundJobHandler? Get(string jobName);

    IReadOnlyCollection<string> GetAllNames();

    void Clear();
}
