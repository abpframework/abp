using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs
{
    /// <summary>
    /// Defines interface to store/get background jobs.
    /// </summary>
    public interface IBackgroundJobStore
    {
        /// <summary>
        /// Gets a BackgroundJobInfo based on the given jobId.
        /// </summary>
        /// <param name="jobId">The Job Unique Identifier.</param>
        /// <returns>The BackgroundJobInfo object.</returns>
        Task<BackgroundJobInfo> FindAsync(Guid jobId);

        /// <summary>
        /// Inserts a background job.
        /// </summary>
        /// <param name="jobInfo">Job information.</param>
        Task InsertAsync(BackgroundJobInfo jobInfo);

        /// <summary>
        /// Gets waiting jobs. It should get jobs based on these:
        /// Conditions: !IsAbandoned And NextTryTime &lt;= Clock.Now.
        /// Order by: Priority DESC, TryCount ASC, NextTryTime ASC.
        /// Maximum result: <paramref name="maxResultCount"/>.
        /// </summary>
        /// <param name="maxResultCount">Maximum result count.</param>
        Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(int maxResultCount);

        /// <summary>
        /// Deletes a job.
        /// </summary>
        /// <param name="jobId">The Job Unique Identifier.</param>
        Task DeleteAsync(Guid jobId);

        /// <summary>
        /// Updates a job.
        /// </summary>
        /// <param name="jobInfo">Job information.</param>
        Task UpdateAsync(BackgroundJobInfo jobInfo);
    }
}