using System.Threading;

namespace Volo.Abp.BackgroundJobs;

/// <summary>
/// Defines interface of a background job.
/// </summary>
public interface IBackgroundJob<in TArgs>
{
    /// <summary>
    /// Executes the job with the <paramref name="args"/>.
    /// </summary>
    /// <param name="args">Job arguments.</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    void Execute(TArgs args, CancellationToken cancellationToken = default);
}
