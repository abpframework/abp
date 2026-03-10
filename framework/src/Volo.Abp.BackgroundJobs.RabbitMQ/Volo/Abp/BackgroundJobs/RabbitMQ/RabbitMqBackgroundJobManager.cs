using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.BackgroundJobs.RabbitMQ;

[Dependency(ReplaceServices = true)]
public class RabbitMqBackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    protected IJobQueueManager JobQueueManager { get; }
    protected IAnonymousJobHandlerRegistry AnonymousJobHandlerRegistry { get; }
    protected IJsonSerializer JsonSerializer { get; }

    public RabbitMqBackgroundJobManager(IJobQueueManager jobQueueManager, IAnonymousJobHandlerRegistry anonymousJobHandlerRegistry, IJsonSerializer jsonSerializer)
    {
        JobQueueManager = jobQueueManager;
        AnonymousJobHandlerRegistry = anonymousJobHandlerRegistry;
        JsonSerializer = jsonSerializer;
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
        if (ShouldWrapAsAnonymousJob(jobName))
        {
            var jsonData = JsonSerializer.Serialize(args);
            var anonymousArgs = new AnonymousJobArgs(jobName, jsonData);
            return await EnqueueAsync(AnonymousJobArgs.JobNameConstant, anonymousArgs, priority, delay);
        }

        var jobQueue = await JobQueueManager.GetAsync(jobName);
        return (await jobQueue.EnqueueAsync(args, priority, delay))!;
    }

    protected virtual bool ShouldWrapAsAnonymousJob(string jobName)
    {
        return jobName != AnonymousJobArgs.JobNameConstant && AnonymousJobHandlerRegistry.IsRegistered(jobName);
    }
}
