using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.BackgroundJobs.RabbitMQ;

[Dependency(ReplaceServices = true)]
public class RabbitMqBackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    protected IJobQueueManager JobQueueManager { get; }
    protected IAnonymousJobHandlerRegistry AnonymousJobHandlerRegistry { get; }
    protected AbpBackgroundJobOptions BackgroundJobOptions { get; }
    protected IJsonSerializer JsonSerializer { get; }
    public ILogger<RabbitMqBackgroundJobManager> Logger { get; set; }

    public RabbitMqBackgroundJobManager(
        IJobQueueManager jobQueueManager,
        IAnonymousJobHandlerRegistry anonymousJobHandlerRegistry,
        IOptions<AbpBackgroundJobOptions> backgroundJobOptions,
        IJsonSerializer jsonSerializer)
    {
        JobQueueManager = jobQueueManager;
        AnonymousJobHandlerRegistry = anonymousJobHandlerRegistry;
        BackgroundJobOptions = backgroundJobOptions.Value;
        JsonSerializer = jsonSerializer;
        Logger = NullLogger<RabbitMqBackgroundJobManager>.Instance;
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
            Logger.LogInformation(
                "Wrapping job into anonymous transport. TransportJobName: {TransportJobName}, EffectiveJobName: {EffectiveJobName}",
                AnonymousJobArgs.JobNameConstant,
                jobName
            );
            var jsonData = JsonSerializer.Serialize(args);
            var anonymousArgs = new AnonymousJobArgs(jobName, jsonData);
            return await EnqueueAsync(AnonymousJobArgs.JobNameConstant, anonymousArgs, priority, delay);
        }

        var jobQueue = await JobQueueManager.GetAsync(jobName);
        return (await jobQueue.EnqueueAsync(args, priority, delay))!;
    }

    protected virtual bool ShouldWrapAsAnonymousJob(string jobName)
    {
        return jobName != AnonymousJobArgs.JobNameConstant &&
               AnonymousJobHandlerRegistry.IsRegistered(jobName) &&
               BackgroundJobOptions.GetJobOrNull(jobName) == null;
    }
}
