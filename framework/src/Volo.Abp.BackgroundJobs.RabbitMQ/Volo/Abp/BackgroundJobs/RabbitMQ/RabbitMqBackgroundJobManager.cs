using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    [Dependency(ReplaceServices = true)]
    public class RabbitMqBackgroundJobManager : IBackgroundJobManager, ITransientDependency
    {
        private readonly IJobQueueManager _jobQueueManager;

        public RabbitMqBackgroundJobManager(IJobQueueManager jobQueueManager)
        {
            _jobQueueManager = jobQueueManager;
        }

        public virtual async Task<string> EnqueueAsync<TArgs>(
            TArgs args, 
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            var jobQueue = await _jobQueueManager.GetAsync<TArgs>();
            return await jobQueue.EnqueueAsync(args, priority, delay);
        }

        public virtual async Task<string> EnqueueAsync<TArgs>(
            TArgs args,
            DateTime executionTime,
            BackgroundJobPriority priority = BackgroundJobPriority.Normal)
        {
            var jobQueue = await _jobQueueManager.GetAsync<TArgs>();
            return await jobQueue.EnqueueAsync(args, executionTime, priority);
        }
    }
}
