using System;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.States;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.Hangfire;

[Dependency(ReplaceServices = true)]
public class HangfireBackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    protected AbpBackgroundJobOptions Options { get; }
    
    public HangfireBackgroundJobManager(IOptions<AbpBackgroundJobOptions> options)
    {
        Options = options.Value;
    }
    
    public virtual Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null)
    {
        return Task.FromResult(delay.HasValue
            ? BackgroundJob.Schedule<HangfireJobExecutionAdapter<TArgs>>(
                adapter => adapter.ExecuteAsync(GetQueueName(typeof(TArgs)),args),
                delay.Value
            )
            : BackgroundJob.Enqueue<HangfireJobExecutionAdapter<TArgs>>(
                adapter => adapter.ExecuteAsync(GetQueueName(typeof(TArgs)) ,args)
            ));
    }

    protected virtual string GetQueueName(Type argsType)
    {
        var queueName = EnqueuedState.DefaultQueue;
        var queueAttribute = Options.GetJob(argsType).JobType.GetCustomAttribute<QueueAttribute>();
        if (queueAttribute != null)
        {
            queueName = queueAttribute.Queue;
        }

        return queueName;
    }
}
