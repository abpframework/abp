using System;
using System.Threading.Tasks;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs
{
    /// <summary>
    /// Some extension methods for <see cref="IBackgroundJobManager"/>.
    /// </summary>
    public static class BackgroundJobManagerExtensions
    {
        /// <summary>
        /// Enqueues a job to be executed.
        /// </summary>
        /// <typeparam name="TArgs">Type of the arguments of job.</typeparam>
        /// <param name="backgroundJobManager">Background job manager reference</param>
        /// <param name="args">Job arguments.</param>
        /// <param name="priority">Job priority.</param>
        /// <param name="delay">Job delay (wait duration before first try).</param>
        public static void Enqueue<TArgs>(this IBackgroundJobManager backgroundJobManager, TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
        {
            AsyncHelper.RunSync(() => backgroundJobManager.EnqueueAsync<TArgs>(args, priority, delay));
        }

        /// <summary>
        /// Enqueues a job to be executed.
        /// </summary>
        /// <param name="backgroundJobManager">Background job manager reference</param>
        /// <param name="jobName">Job name.</param>
        /// <param name="args">Job arguments.</param>
        /// <param name="priority">Job priority.</param>
        /// <param name="delay">Job delay (wait duration before first try).</param>
        /// <returns>Unique identifier of a background job.</returns>
        public static Guid EnqueueAsync(
            this IBackgroundJobManager backgroundJobManager, 
            string jobName, 
            object args,
            BackgroundJobPriority priority = BackgroundJobPriority.Normal, 
            TimeSpan? delay = null)
        {
            return AsyncHelper.RunSync(() => backgroundJobManager.EnqueueAsync(jobName, args, priority, delay));
        }
    }
}
