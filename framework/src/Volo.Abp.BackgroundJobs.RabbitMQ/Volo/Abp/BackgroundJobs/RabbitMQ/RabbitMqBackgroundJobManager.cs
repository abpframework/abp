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

        public Task<string> EnqueueAsync<TArgs>(
            TArgs args, 
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            return _jobQueueManager
                .Get<TArgs>()
                .EnqueueAsync(args, priority, delay);
        }
    }
}
