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

        public virtual async Task<BackgroundJobInfo> FindAsync(Guid jobId)
        {
            return ObjectMapper.Map<BackgroundJobRecord, BackgroundJobInfo>(
                await BackgroundJobRepository.FindAsync(jobId)
.ConfigureAwait(false));
        }

        public virtual async Task InsertAsync(BackgroundJobInfo jobInfo)
        {
            await BackgroundJobRepository.InsertAsync(
                ObjectMapper.Map<BackgroundJobInfo, BackgroundJobRecord>(jobInfo)
            ).ConfigureAwait(false);
        }

        public virtual async Task<List<BackgroundJobInfo>> GetWaitingJobsAsync(int maxResultCount)
        {
            return ObjectMapper.Map<List<BackgroundJobRecord>, List<BackgroundJobInfo>>(
                await BackgroundJobRepository.GetWaitingListAsync(maxResultCount)
.ConfigureAwait(false));
        }

        public virtual async Task DeleteAsync(Guid jobId)
        {
            await BackgroundJobRepository.DeleteAsync(jobId).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(BackgroundJobInfo jobInfo)
        {
            var backgroundJobRecord = await BackgroundJobRepository.FindAsync(jobInfo.Id).ConfigureAwait(false);
            if (backgroundJobRecord == null)
            {
                return;
            }

            ObjectMapper.Map(jobInfo, backgroundJobRecord);
            await BackgroundJobRepository.UpdateAsync(backgroundJobRecord).ConfigureAwait(false);
        }
    }
}
