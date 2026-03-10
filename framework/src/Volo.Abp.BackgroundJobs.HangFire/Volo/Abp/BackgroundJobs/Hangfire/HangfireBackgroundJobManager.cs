using System;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.States;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Hangfire;
using Volo.Abp.Json;

namespace Volo.Abp.BackgroundJobs.Hangfire;

[Dependency(ReplaceServices = true)]
public class HangfireBackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    protected IOptions<AbpBackgroundJobOptions> BackgroundJobOptions { get; }
    protected IOptions<AbpHangfireOptions> HangfireOptions { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected IAnonymousJobHandlerRegistry AnonymousJobHandlerRegistry { get; }
    public ILogger<HangfireBackgroundJobManager> Logger { get; set; }

    public HangfireBackgroundJobManager(
        IOptions<AbpBackgroundJobOptions> backgroundJobOptions,
        IOptions<AbpHangfireOptions> hangfireOptions,
        IJsonSerializer jsonSerializer,
        IAnonymousJobHandlerRegistry anonymousJobHandlerRegistry)
    {
        BackgroundJobOptions = backgroundJobOptions;
        HangfireOptions = hangfireOptions;
        JsonSerializer = jsonSerializer;
        AnonymousJobHandlerRegistry = anonymousJobHandlerRegistry;
        Logger = NullLogger<HangfireBackgroundJobManager>.Instance;
    }

    public virtual Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null)
    {
        return Task.FromResult(delay.HasValue
            ? BackgroundJob.Schedule<HangfireJobExecutionAdapter<TArgs>>(
                adapter => adapter.ExecuteAsync(GetQueueName(typeof(TArgs)), args, default),
                delay.Value
            )
            : BackgroundJob.Enqueue<HangfireJobExecutionAdapter<TArgs>>(
                adapter => adapter.ExecuteAsync(GetQueueName(typeof(TArgs)), args, default)
            ));
    }

    public virtual Task<string> EnqueueAsync(string jobName, object args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
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
            return EnqueueAsync(AnonymousJobArgs.JobNameConstant, anonymousArgs, priority, delay);
        }

        var serializedArgs = JsonSerializer.Serialize(args);
        var queueName = GetQueueName(jobName);

        return Task.FromResult(delay.HasValue
            ? BackgroundJob.Schedule<HangfireJobExecutionAdapter>(
                adapter => adapter.ExecuteAsync(queueName, jobName, serializedArgs, default),
                delay.Value
            )
            : BackgroundJob.Enqueue<HangfireJobExecutionAdapter>(
                adapter => adapter.ExecuteAsync(queueName, jobName, serializedArgs, default)
            ));
    }

    protected virtual bool ShouldWrapAsAnonymousJob(string jobName)
    {
        return jobName != AnonymousJobArgs.JobNameConstant && AnonymousJobHandlerRegistry.IsRegistered(jobName);
    }

    protected virtual string GetQueueName(Type argsType)
    {
        return GetQueueName(BackgroundJobOptions.Value.GetJob(argsType));
    }

    protected virtual string GetQueueName(string jobName)
    {
        return GetQueueName(BackgroundJobOptions.Value.GetJob(jobName));
    }

    protected virtual string GetQueueName(BackgroundJobConfiguration jobConfiguration)
    {
        var queueAttribute = jobConfiguration.JobType.GetCustomAttribute<QueueAttribute>();
        return queueAttribute != null ? HangfireOptions.Value.DefaultQueuePrefix + queueAttribute.Queue : HangfireOptions.Value.DefaultQueue;
    }
}
