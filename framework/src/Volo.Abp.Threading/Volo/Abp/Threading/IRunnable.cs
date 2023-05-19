using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Threading;

/// <summary>
/// Interface to start/stop self threaded services.
/// </summary>
public interface IRunnable
{
    /// <summary>
    /// Starts the service.
    /// </summary>
    Task StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Stops the service.
    /// </summary>
    Task StopAsync(CancellationToken cancellationToken = default);
}
