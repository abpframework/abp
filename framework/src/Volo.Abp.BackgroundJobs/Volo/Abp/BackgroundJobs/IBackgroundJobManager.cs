using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;

namespace Volo.Abp.BackgroundJobs
{
    //TODO: Create a non-generic EnqueueAsync extension method to IBackgroundJobManager which takes types as input parameters rather than generic parameters.
    /// <summary>
    /// Defines interface of a job manager.
    /// </summary>
    public interface IBackgroundJobManager : IBackgroundWorker
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