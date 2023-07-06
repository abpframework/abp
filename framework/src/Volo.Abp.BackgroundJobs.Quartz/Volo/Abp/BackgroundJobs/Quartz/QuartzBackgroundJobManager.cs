using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.BackgroundJobs.Quartz;

[Dependency(ReplaceServices = true)]
public class QuartzBackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    public const string JobDataPrefix = "Abp";
    public const string RetryIndex = "RetryIndex";

    protected IScheduler Scheduler { get; }

    protected AbpBackgroundJobQuartzOptions Options { get; }

    protected IJsonSerializer JsonSerializer { get; }

    public QuartzBackgroundJobManager(IScheduler scheduler, IOptions<AbpBackgroundJobQuartzOptions> options, IJsonSerializer jsonSerializer)
    {
        Scheduler = scheduler;
        JsonSerializer = jsonSerializer;
        Options = options.Value;
    }

    public virtual async Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null)
    {
        var executionTime = delay.HasValue ? DateTime.Now.Add(delay.Value) : (DateTime?) null;
            
        return await ReEnqueueAsync(args, Options.RetryCount, Options.RetryIntervalMillisecond, priority, executionTime);
    }

    public virtual async Task<string> EnqueueAsync<TArgs>(TArgs args, DateTime executionTime, BackgroundJobPriority priority = BackgroundJobPriority.Normal)
    {
        return await ReEnqueueAsync(args, Options.RetryCount, Options.RetryIntervalMillisecond, priority, executionTime);
    }

    public virtual async Task<string> ReEnqueueAsync<TArgs>(TArgs args, int retryCount, int retryIntervalMillisecond,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal, DateTime? executionTime = null)
    {
        var jobDataMap = new JobDataMap
        {
            {nameof(TArgs), JsonSerializer.Serialize(args)},
            {JobDataPrefix+ nameof(Options.RetryCount), retryCount.ToString()},
            {JobDataPrefix+ nameof(Options.RetryIntervalMillisecond), retryIntervalMillisecond.ToString()},
            {JobDataPrefix+ RetryIndex, "0"}
        };

        var jobDetail = JobBuilder.Create<QuartzJobExecutionAdapter<TArgs>>().RequestRecovery().SetJobData(jobDataMap).Build();
        var trigger = !executionTime.HasValue ? TriggerBuilder.Create().StartNow().Build() : TriggerBuilder.Create().StartAt(new DateTimeOffset(executionTime.Value)).Build();
        await Scheduler.ScheduleJob(jobDetail, trigger);
        return jobDetail.Key.ToString();
    }
}