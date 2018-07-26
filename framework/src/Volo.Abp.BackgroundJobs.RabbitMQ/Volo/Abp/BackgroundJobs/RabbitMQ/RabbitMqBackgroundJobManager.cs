using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.RabbitMQ;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public class RabbitMqBackgroundJobManager : IBackgroundJobManager, ITransientDependency
    {
        private readonly IJobQueueManager _jobQueueManager;

        public RabbitMqBackgroundJobManager(IJobQueueManager jobQueueManager)
        {
            _jobQueueManager = jobQueueManager;
        }

        public Task<string> EnqueueAsync<TArgs>(
            TArgs args, 
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            var jobQueue = _jobQueueManager.Get<TArgs>();
            return jobQueue.EnqueueAsync(args, priority, delay);
        }
    }
}
