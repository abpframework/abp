using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers.Quartz
{
    [Dependency(ReplaceServices = true)]
    public class QuartzBackgroundWorkerManager : IBackgroundWorkerManager, ISingletonDependency
    {
        private readonly IScheduler _scheduler;
        private readonly AbpBackgroundWorkerOptions _options;

        public QuartzBackgroundWorkerManager(IScheduler scheduler, IOptions<AbpBackgroundWorkerOptions> options)
        {
            _scheduler = scheduler;
            _options = options.Value;
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
            if (_options.IsEnabled && worker is IQuartzBackgroundWorker quartzWork)
            {
                Check.NotNull(quartzWork.Trigger, nameof(quartzWork.Trigger));
                Check.NotNull(quartzWork.JobDetail, nameof(quartzWork.JobDetail));

                if (AsyncHelper.RunSync(() => _scheduler.CheckExists(quartzWork.JobDetail.Key)))
                {
                    AsyncHelper.RunSync(() => _scheduler.DeleteJob(quartzWork.JobDetail.Key));
                }

                AsyncHelper.RunSync(() => _scheduler.ScheduleJob(quartzWork.JobDetail, quartzWork.Trigger));
            }
        }
    }
}