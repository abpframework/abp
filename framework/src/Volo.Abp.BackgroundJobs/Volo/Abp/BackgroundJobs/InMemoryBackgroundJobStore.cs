using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs
{
    public class InMemoryBackgroundJobStore : IBackgroundJobStore, ISingletonDependency
    {
        private readonly ConcurrentDictionary<Guid, BackgroundJobInfo> _jobs;

        protected IClock Clock { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryBackgroundJobStore"/> class.
        /// </summary>
        public InMemoryBackgroundJobStore(IClock clock)
        {
            Clock = clock;
            _jobs = new ConcurrentDictionary<Guid, BackgroundJobInfo>();
        }

        public BackgroundJobInfo Find(Guid jobId)
        {
            return _jobs.GetOrDefault(jobId);
        }

        public virtual Task<BackgroundJobInfo> FindAsync(Guid jobId)
        {
            return Task.FromResult(_jobs.GetOrDefault(jobId));
        }

        public void Insert(BackgroundJobInfo jobInfo)
        {
            _jobs[jobInfo.Id] = jobInfo;
        }

        public virtual Task InsertAsync(BackgroundJobInfo jobInfo)
        {
            _jobs[jobInfo.Id] = jobInfo;

            return Task.FromResult(0);
        }

        public List<BackgroundJobInfo> GetWaitingJobs(int maxResultCount)
        {
            return _jobs.Values
                .Where(t => !t.IsAbandoned && t.NextTryTime <= Clock.Now)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.TryCount)
                .ThenBy(t => t.NextTryTime)
                .Take(maxResultCount)
                .ToList();
        }

        public virtual Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(int maxResultCount)
        {
            var waitingJobs = _jobs.Values
                .Where(t => !t.IsAbandoned && t.NextTryTime <= Clock.Now)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.TryCount)
                .ThenBy(t => t.NextTryTime)
                .Take(maxResultCount)
                .ToList();

            return Task.FromResult(waitingJobs);
        }

        public void Delete(Guid jobId)
        {
            _jobs.TryRemove(jobId, out _);
        }

        public virtual Task DeleteAsync(Guid jobId)
        {
            _jobs.TryRemove(jobId, out _);

            return Task.FromResult(0);
        }

        public void Update(BackgroundJobInfo jobInfo)
        {
            if (jobInfo.IsAbandoned)
            {
                DeleteAsync(jobInfo.Id);
            }
        }

        public virtual Task UpdateAsync(BackgroundJobInfo jobInfo)
        {
            if (jobInfo.IsAbandoned)
            {
                return DeleteAsync(jobInfo.Id);
            }

            return Task.FromResult(0);
        }
    }
}