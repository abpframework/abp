using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs
{
    /// <summary>
    /// Defines interface of a background job.
    /// </summary>
    public interface IAsyncBackgroundJob<in TArgs>
    {
        /// <summary>
        /// Executes the job with the <paramref name="args"/>.
        /// </summary>
        /// <param name="args">Job arguments.</param>
        Task ExecuteAsync(TArgs args);
    }
}
