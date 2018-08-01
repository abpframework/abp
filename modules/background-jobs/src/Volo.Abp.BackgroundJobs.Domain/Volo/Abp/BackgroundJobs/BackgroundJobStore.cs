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
        protected IObjectMapper ObjectMapper { get; }

        public BackgroundJobStore(
            IBackgroundJobRepository backgroundJobRepository,
            IObjectMapper objectMapper)
        {
            ObjectMapper = objectMapper;
            BackgroundJobRepository = backgroundJobRepository;
        }

        public async Task<BackgroundJobInfo> FindAsync(Guid jobId)
        {
            return ObjectMapper.Map<BackgroundJobRecord, BackgroundJobInfo>(
                await BackgroundJobRepository.FindAsync(jobId)
            );
        }

        public async Task InsertAsync(BackgroundJobInfo jobInfo)
        {
            await BackgroundJobRepository.InsertAsync(
                ObjectMapper.Map<BackgroundJobInfo, BackgroundJobRecord>(jobInfo)
            );
        }

        public async Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(int maxResultCount)
        {
            return ObjectMapper.Map<List<BackgroundJobRecord>, List<BackgroundJobInfo>>(
                await BackgroundJobRepository.GetWaitingListAsync(maxResultCount)
            );
        }

        public async Task DeleteAsync(Guid jobId)
        {
            await BackgroundJobRepository.DeleteAsync(jobId);
        }

        public async Task UpdateAsync(BackgroundJobInfo jobInfo)
        {
            await BackgroundJobRepository.UpdateAsync(
                ObjectMapper.Map<BackgroundJobInfo, BackgroundJobRecord>(jobInfo)
            );
        }
    }
}
