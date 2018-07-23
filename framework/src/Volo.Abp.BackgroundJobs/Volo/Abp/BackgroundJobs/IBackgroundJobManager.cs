using System;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs
{
    /// <summary>
    /// Defines interface of a job manager.
    /// </summary>
    public interface IBackgroundJobManager
    {
        /// <summary>
        /// Enqueues a job to be executed.
        /// </summary>
        /// <typeparam name="TArgs">Type of the arguments of job.</typeparam>
        /// <param name="args">Job arguments.</param>
        /// <param name="priority">Job priority.</param>
        /// <param name="delay">Job delay (wait duration before first try).</param>
        /// <returns>Unique identifier of a background job.</returns>
        Task<Guid> EnqueueAsync<TArgs>(
            TArgs args,
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null
        );

        /// <summary>
        /// Enqueues a job to be executed.
        /// </summary>
        /// <param name="jobName">Job name.</param>
        /// <param name="args">Job arguments.</param>
        /// <param name="priority">Job priority.</param>
        /// <param name="delay">Job delay (wait duration before first try).</param>
        /// <returns>Unique identifier of a background job.</returns>
        Task<Guid> EnqueueAsync(string jobName, object args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null);
    }
}