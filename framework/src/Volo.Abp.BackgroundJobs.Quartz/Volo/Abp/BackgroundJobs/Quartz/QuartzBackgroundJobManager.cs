using System;
using System.Threading.Tasks;
using Quartz;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.Quartz
{
    [Dependency(ReplaceServices = true)]
    public class QuartzBackgroundJobManager : IBackgroundJobManager, ITransientDependency
    {
        private readonly IScheduler _scheduler;

        public QuartzBackgroundJobManager(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public async Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            var jobDetail = JobBuilder.Create<QuartzJobExecutionAdapter<TArgs>>().SetJobData(new JobDataMap { { nameof(TArgs), args } }).Build();
            var trigger = !delay.HasValue ? TriggerBuilder.Create().StartNow().Build() : TriggerBuilder.Create().StartAt(new DateTimeOffset(DateTime.Now.Add(delay.Value))).Build();
            await _scheduler.ScheduleJob(jobDetail, trigger);
            return jobDetail.Key.ToString();
        }
    }
}
