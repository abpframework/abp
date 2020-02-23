using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers.Quartz
{
    public class QuartzBackgroundWorkerManager : IBackgroundWorkerManager, ITransientDependency
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
            if (worker is IQuartzBackgroundWorker quartzWork)
            {
                AsyncHelper.RunSync(() => _scheduler.ScheduleJob(quartzWork.JobDetail, quartzWork.Trigger));
            }
        }

    }
}
