using System;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.States;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Hangfire;

namespace Volo.Abp.BackgroundJobs.Hangfire;

[Dependency(ReplaceServices = true)]
public class HangfireBackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    protected IOptions<AbpBackgroundJobOptions> BackgroundJobOptions { get; }
    protected IOptions<AbpHangfireOptions> HangfireOptions { get; }

    public HangfireBackgroundJobManager(IOptions<AbpBackgroundJobOptions> backgroundJobOptions, IOptions<AbpHangfireOptions> hangfireOptions)
    {
        BackgroundJobOptions = backgroundJobOptions;
        HangfireOptions = hangfireOptions;
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

    protected virtual string GetQueueName(Type argsType)
    {
        var queueAttribute = BackgroundJobOptions.Value.GetJob(argsType).JobType.GetCustomAttribute<QueueAttribute>();
        return queueAttribute != null ? HangfireOptions.Value.DefaultQueuePrefix + queueAttribute.Queue : HangfireOptions.Value.DefaultQueue;
    }
}
