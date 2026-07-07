using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs;

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
    /// Conditions: ApplicationName is applicationName And !IsAbandoned And CompletionTime == null And NextTryTime &lt;= Clock.Now.
    /// Order by: Priority DESC, TryCount ASC, NextTryTime ASC.
    /// Maximum result: <paramref name="maxResultCount"/>.
    /// </summary>
    /// <param name="applicationName">Application name.</param>
    /// <param name="maxResultCount">Maximum result count.</param>
    Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(string? applicationName, int maxResultCount);

    /// <summary>
    /// Gets waiting jobs (see <see cref="GetWaitingJobsAsync(string, int)"/>), additionally filtered by job name
    /// according to <paramref name="jobNameFilter"/>.
    /// </summary>
    /// <param name="applicationName">Application name.</param>
    /// <param name="maxResultCount">Maximum result count.</param>
    /// <param name="jobNameFilter">Job name filter. When null, no job name filter is applied.</param>
    Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(
        string? applicationName,
        int maxResultCount,
        BackgroundJobNameFilter? jobNameFilter);

    /// <summary>
    /// Deletes a job.
    /// </summary>
    /// <param name="jobId">The Job Unique Identifier.</param>
    Task DeleteAsync(Guid jobId);

    /// <summary>
    /// Deletes successfully completed jobs (<see cref="BackgroundJobInfo.CompletionTime"/> is set) of the given
    /// application that completed before <paramref name="completedBefore"/>. Used by the retention cleanup.
    /// </summary>
    /// <returns>The number of deleted jobs.</returns>
    Task<int> DeleteAsync(
        string? applicationName,
        DateTime completedBefore,
        int maxResultCount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a job.
    /// </summary>
    /// <param name="jobInfo">Job information.</param>
    Task UpdateAsync(BackgroundJobInfo jobInfo);
}
