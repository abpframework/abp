using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
    protected AbpBackgroundJobOptions BackgroundJobOptions { get; }

    protected IJsonSerializer JsonSerializer { get; }

    protected IAnonymousJobHandlerRegistry AnonymousJobHandlerRegistry { get; }
    public ILogger<QuartzBackgroundJobManager> Logger { get; set; }

    public QuartzBackgroundJobManager(
        IScheduler scheduler,
        IOptions<AbpBackgroundJobQuartzOptions> options,
        IOptions<AbpBackgroundJobOptions> backgroundJobOptions,
        IJsonSerializer jsonSerializer,
        IAnonymousJobHandlerRegistry anonymousJobHandlerRegistry)
    {
        Scheduler = scheduler;
        JsonSerializer = jsonSerializer;
        Options = options.Value;
        BackgroundJobOptions = backgroundJobOptions.Value;
        AnonymousJobHandlerRegistry = anonymousJobHandlerRegistry;
        Logger = NullLogger<QuartzBackgroundJobManager>.Instance;
    }

    public virtual async Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null)
    {
        return await ReEnqueueAsync(args, Options.RetryCount, Options.RetryIntervalMillisecond, priority, delay);
    }

    public virtual async Task<string> EnqueueAsync(string jobName, object args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
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

        return await ReEnqueueAsync(jobName, args, Options.RetryCount, Options.RetryIntervalMillisecond, priority, delay);
    }

    protected virtual bool ShouldWrapAsAnonymousJob(string jobName)
    {
        return jobName != AnonymousJobArgs.JobNameConstant &&
               AnonymousJobHandlerRegistry.IsRegistered(jobName) &&
               BackgroundJobOptions.GetJobOrNull(jobName) == null;
    }

    public virtual async Task<string> ReEnqueueAsync<TArgs>(TArgs args, int retryCount, int retryIntervalMillisecond,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
    {
        var jobDataMap = new JobDataMap
            {
                {nameof(TArgs), JsonSerializer.Serialize(args!)},
                {JobDataPrefix+ nameof(Options.RetryCount), retryCount.ToString()},
                {JobDataPrefix+ nameof(Options.RetryIntervalMillisecond), retryIntervalMillisecond.ToString()},
                {JobDataPrefix+ RetryIndex, "0"}
            };

        var jobDetail = JobBuilder.Create<QuartzJobExecutionAdapter<TArgs>>().RequestRecovery().SetJobData(jobDataMap).Build();
        var trigger = !delay.HasValue ? TriggerBuilder.Create().StartNow().Build() : TriggerBuilder.Create().StartAt(new DateTimeOffset(DateTime.Now.Add(delay.Value))).Build();
        await Scheduler.ScheduleJob(jobDetail, trigger);
        return jobDetail.Key.ToString();
    }

    public virtual async Task<string> ReEnqueueAsync(string jobName, object args, int retryCount, int retryIntervalMillisecond,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
    {
        var jobDataMap = new JobDataMap
            {
                {QuartzJobExecutionAdapter.JobNameKey, jobName},
                {QuartzJobExecutionAdapter.JobArgsKey, JsonSerializer.Serialize(args)},
                {JobDataPrefix + nameof(Options.RetryCount), retryCount.ToString()},
                {JobDataPrefix + nameof(Options.RetryIntervalMillisecond), retryIntervalMillisecond.ToString()},
                {JobDataPrefix + RetryIndex, "0"}
            };

        var jobDetail = JobBuilder.Create<QuartzJobExecutionAdapter>().RequestRecovery().SetJobData(jobDataMap).Build();
        var trigger = !delay.HasValue ? TriggerBuilder.Create().StartNow().Build() : TriggerBuilder.Create().StartAt(new DateTimeOffset(DateTime.Now.Add(delay.Value))).Build();
        await Scheduler.ScheduleJob(jobDetail, trigger);
        return jobDetail.Key.ToString();
    }
}
