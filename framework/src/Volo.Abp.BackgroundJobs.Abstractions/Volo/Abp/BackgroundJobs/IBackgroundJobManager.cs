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
        Task<string> EnqueueAsync<TArgs>(
            TArgs args,
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null
        );

        /// <summary>
        /// Enqueues a job to be executed.
        /// </summary>
        /// <typeparam name="TArgs">Type of the arguments of job.</typeparam>
        /// <param name="args">Job arguments.</param>
        /// <param name="executionTime">Time to execute the job.</param>
        /// <param name="priority">Job priority.</param>
        /// <returns>Unique identifier of a background job.</returns>
        Task<string> EnqueueAsync<TArgs>(
            TArgs args,
            DateTime executionTime,
            BackgroundJobPriority priority = BackgroundJobPriority.Normal
        );
    }
}