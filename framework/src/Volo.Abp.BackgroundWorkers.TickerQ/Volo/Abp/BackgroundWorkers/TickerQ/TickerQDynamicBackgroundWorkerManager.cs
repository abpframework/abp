using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers.TickerQ;

[Dependency(ReplaceServices = true)]
public class TickerQDynamicBackgroundWorkerManager : IDynamicBackgroundWorkerManager, ISingletonDependency
{
    public virtual Task AddAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        DynamicBackgroundWorkerHandler handler,
        CancellationToken cancellationToken = default)
    {
        throw new AbpException(
            "TickerQ does not support dynamic background worker registration at runtime. " +
            "TickerQ uses FrozenDictionary for function registration, which requires all functions to be registered before the application starts. " +
            "Please use Hangfire or Quartz provider for dynamic background workers.");
    }

    public virtual Task<bool> RemoveAsync(string workerName, CancellationToken cancellationToken = default)
    {
        throw new AbpException(
            "TickerQ does not support dynamic background worker registration at runtime. " +
            "Please use Hangfire or Quartz provider for dynamic background workers.");
    }

    public virtual Task<bool> UpdateScheduleAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        CancellationToken cancellationToken = default)
    {
        throw new AbpException(
            "TickerQ does not support dynamic background worker registration at runtime. " +
            "Please use Hangfire or Quartz provider for dynamic background workers.");
    }

    public virtual bool IsRegistered(string workerName)
    {
        // TickerQ does not support runtime registration, so there are never any registered workers.
        return false;
    }

    public virtual Task StopAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
