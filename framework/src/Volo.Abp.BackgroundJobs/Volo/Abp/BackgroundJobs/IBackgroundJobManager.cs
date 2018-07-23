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
        /// <typeparam name="TJob">Type of the job.</typeparam>
        /// <typeparam name="TArgs">Type of the arguments of job.</typeparam>
        /// <param name="args">Job arguments.</param>
        /// <param name="priority">Job priority.</param>
        /// <param name="delay">Job delay (wait duration before first try).</param>
        /// <returns>Unique identifier of a background job.</returns>
        Task<Guid> EnqueueAsync<TJob, TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
            where TJob : IBackgroundJob<TArgs>;

        /// <summary>
        /// Deletes a job with the specified jobId.
        /// </summary>
        /// <param name="jobId">The Job Unique Identifier.</param>
        /// <returns><c>True</c> on a successfull state transition, <c>false</c> otherwise.</returns>
        Task<bool> DeleteAsync(Guid jobId);
    }
}