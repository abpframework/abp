using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.RabbitMQ;

[Dependency(ReplaceServices = true)]
public class RabbitMqBackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    protected IJobQueueManager JobQueueManager { get; }

    public RabbitMqBackgroundJobManager(IJobQueueManager jobQueueManager)
    {
        JobQueueManager = jobQueueManager;
    }

    public virtual async Task<string> EnqueueAsync<TArgs>(
        TArgs args,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null)
    {
        var jobQueue = await JobQueueManager.GetAsync<TArgs>();
        return (await jobQueue.EnqueueAsync(args, priority, delay))!;
    }

    public virtual async Task<string> EnqueueAsync(
        string jobName,
        object args,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null)
    {
        var jobQueue = await JobQueueManager.GetAsync(jobName);
        return (await jobQueue.EnqueueAsync(args, priority, delay))!;
    }
}
