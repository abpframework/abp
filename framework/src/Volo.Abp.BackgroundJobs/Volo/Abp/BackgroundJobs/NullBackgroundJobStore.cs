using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs
{
    /// <summary>
    /// Null pattern implementation of <see cref="IBackgroundJobStore"/>.
    /// It's used if <see cref="IBackgroundJobStore"/> is not implemented by actual persistent store
    /// and job execution is not enabled (<see cref="IBackgroundJobOptions.IsJobExecutionEnabled"/>) for the application.
    /// </summary>
    public class NullBackgroundJobStore : IBackgroundJobStore
    {
        public Task<BackgroundJobInfo> GetAsync(Guid jobId)
        {
            return Task.FromResult((BackgroundJobInfo)null);
        }

        public Task InsertAsync(BackgroundJobInfo jobInfo)
        {
            return Task.CompletedTask;
        }

        public Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(int maxResultCount)
        {
            return Task.FromResult(new List<BackgroundJobInfo>());
        }

        public Task DeleteAsync(BackgroundJobInfo jobInfo)
        {
            return Task.CompletedTask;
        }

        public Task UpdateAsync(BackgroundJobInfo jobInfo)
        {
            return Task.CompletedTask;
        }
    }
}