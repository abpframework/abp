using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace Volo.Abp.BackgroundJobs
{
    public class BackgroundJobStore : IBackgroundJobStore, ITransientDependency
    {
        protected IBackgroundJobRepository BackgroundJobRepository { get; }

        protected IObjectMapper<AbpBackgroundJobsDomainModule> ObjectMapper { get; }

        public BackgroundJobStore(
            IBackgroundJobRepository backgroundJobRepository,
            IObjectMapper<AbpBackgroundJobsDomainModule> objectMapper)
        {
            ObjectMapper = objectMapper;
            BackgroundJobRepository = backgroundJobRepository;
        }

        public BackgroundJobInfo Find(Guid jobId)
        {
            return ObjectMapper.Map<BackgroundJobRecord, BackgroundJobInfo>(
                BackgroundJobRepository.Find(jobId)
            );
        }

        public virtual async Task<BackgroundJobInfo> FindAsync(Guid jobId)
        {
            return ObjectMapper.Map<BackgroundJobRecord, BackgroundJobInfo>(
                await BackgroundJobRepository.FindAsync(jobId)
            );
        }

        public void Insert(BackgroundJobInfo jobInfo)
        {
            BackgroundJobRepository.Insert(
                ObjectMapper.Map<BackgroundJobInfo, BackgroundJobRecord>(jobInfo)
            );
        }

        public virtual async Task InsertAsync(BackgroundJobInfo jobInfo)
        {
            await BackgroundJobRepository.InsertAsync(
                ObjectMapper.Map<BackgroundJobInfo, BackgroundJobRecord>(jobInfo)
            );
        }

        public List<BackgroundJobInfo> GetWaitingJobs(int maxResultCount)
        {
            return ObjectMapper.Map<List<BackgroundJobRecord>, List<BackgroundJobInfo>>(
                BackgroundJobRepository.GetWaitingList(maxResultCount)
            );
        }

        public virtual async Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(int maxResultCount)
        {
            return ObjectMapper.Map<List<BackgroundJobRecord>, List<BackgroundJobInfo>>(
                await BackgroundJobRepository.GetWaitingListAsync(maxResultCount)
            );
        }

        public void Delete(Guid jobId)
        {
            BackgroundJobRepository.Delete(jobId);
        }

        public virtual async Task DeleteAsync(Guid jobId)
        {
            await BackgroundJobRepository.DeleteAsync(jobId);
        }

        public void Update(BackgroundJobInfo jobInfo)
        {
            BackgroundJobRepository.Update(
                ObjectMapper.Map<BackgroundJobInfo, BackgroundJobRecord>(jobInfo)
            );
        }

        public virtual async Task UpdateAsync(BackgroundJobInfo jobInfo)
        {
            await BackgroundJobRepository.UpdateAsync(
                ObjectMapper.Map<BackgroundJobInfo, BackgroundJobRecord>(jobInfo)
            );
        }
    }
}
