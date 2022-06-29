using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers;

/// <summary>
/// Used to manage background workers.
/// </summary>
public interface IBackgroundWorkerManager : IRunnable
{
    /// <summary>
    /// Adds a new worker. Starts the worker immediately if <see cref="IBackgroundWorkerManager"/> has started.
    /// </summary>
    /// <param name="worker">
    /// The worker. It should be resolved from IOC.
    /// </param>
    /// <param name="cancellationToken"></param>
    Task AddAsync(IBackgroundWorker worker, CancellationToken cancellationToken = default);
}
