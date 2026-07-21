using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundWorkers;

public static class DynamicBackgroundWorkerManagerExtensions
{
    /// <summary>
    /// Adds a dynamic worker with the default schedule (<see cref="DynamicBackgroundWorkerSchedule.DefaultPeriod"/>).
    /// </summary>
    public static Task AddAsync(
        this IDynamicBackgroundWorkerManager manager,
        string workerName,
        DynamicBackgroundWorkerHandler handler,
        CancellationToken cancellationToken = default)
    {
        return manager.AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule
            {
                Period = DynamicBackgroundWorkerSchedule.DefaultPeriod
            },
            handler,
            cancellationToken);
    }
}
