using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers.Quartz
{
    [Dependency(ReplaceServices = true)]
    public class QuartzBackgroundWorkerManager : IBackgroundWorkerManager, ISingletonDependency
    {
        private readonly IScheduler _scheduler;

        public QuartzBackgroundWorkerManager(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            await _scheduler.ResumeAll(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            if (!_scheduler.IsShutdown)
            {
                await _scheduler.PauseAll(cancellationToken);
            }
        }

        public void Add(IBackgroundWorker worker)
        {
            AsyncHelper.RunSync(() => ReScheduleJobAsync(worker));
        }

        private async Task ReScheduleJobAsync(IBackgroundWorker worker)
        {
            if (worker is IQuartzBackgroundWorker quartzWork)
            {
                Check.NotNull(quartzWork.Trigger, nameof(quartzWork.Trigger));
                Check.NotNull(quartzWork.JobDetail, nameof(quartzWork.JobDetail));
                
                if (await _scheduler.CheckExists(quartzWork.JobDetail.Key))
                {
                    await _scheduler.AddJob(quartzWork.JobDetail, true);
                    await _scheduler.ResumeJob(quartzWork.JobDetail.Key);
                    await _scheduler.RescheduleJob(quartzWork.Trigger.Key, quartzWork.Trigger);
                }
                else
                {
                    await _scheduler.ScheduleJob(quartzWork.JobDetail, quartzWork.Trigger);
                }
            }
        }
    }
}