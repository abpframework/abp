using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Quartz;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.Quartz
{
    [Dependency(ReplaceServices = true)]
    public class QuartzBackgroundJobManager : IBackgroundJobManager, ITransientDependency
    {
        public const string JobDataPrefix = "Abp";
        public const string RetryIndex = "RetryIndex";

        protected IScheduler Scheduler { get; }

        protected AbpBackgroundJobQuartzOptions Options { get; }

        public QuartzBackgroundJobManager(IScheduler scheduler, IOptions<AbpBackgroundJobQuartzOptions> options)
        {
            Scheduler = scheduler;
            Options = options.Value;
        }

        public virtual async Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            return await ReEnqueueAsync(args, Options.RetryCount, Options.RetryIntervalMillisecond, priority, delay);
        }

        public virtual async Task<string> ReEnqueueAsync<TArgs>(TArgs args, int retryCount, int retryIntervalMillisecond,
            BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
        {
            var jobDataMap = new JobDataMap
            {
                {nameof(TArgs), args},
                {JobDataPrefix+ nameof(Options.RetryCount), retryCount},
                {JobDataPrefix+ nameof(Options.RetryIntervalMillisecond), retryIntervalMillisecond},
                {JobDataPrefix+ RetryIndex, 0}
            };

            var jobDetail = JobBuilder.Create<QuartzJobExecutionAdapter<TArgs>>().RequestRecovery().SetJobData(jobDataMap).Build();
            var trigger = !delay.HasValue ? TriggerBuilder.Create().StartNow().Build() : TriggerBuilder.Create().StartAt(new DateTimeOffset(DateTime.Now.Add(delay.Value))).Build();
            await Scheduler.ScheduleJob(jobDetail, trigger);
            return jobDetail.Key.ToString();
        }
    }
}
