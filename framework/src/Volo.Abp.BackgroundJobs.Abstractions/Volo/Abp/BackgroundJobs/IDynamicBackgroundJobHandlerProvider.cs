using System;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs;

public interface IDynamicBackgroundJobHandlerProvider
{
    void Register(string jobName, Func<DynamicBackgroundJobContext, Task> handler);

    void Register(string jobName, Action<DynamicBackgroundJobContext> handler);

    bool Unregister(string jobName);

    bool IsRegistered(string jobName);
}
