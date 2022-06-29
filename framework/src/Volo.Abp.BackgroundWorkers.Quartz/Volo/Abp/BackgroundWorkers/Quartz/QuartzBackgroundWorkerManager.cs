using System;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.BackgroundWorkers.Quartz;

[Dependency(ReplaceServices = true)]
public class QuartzBackgroundWorkerManager : IBackgroundWorkerManager, ISingletonDependency
{
    private readonly IScheduler _scheduler;

    public QuartzBackgroundWorkerManager(IScheduler scheduler)
    {
        _scheduler = scheduler;
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken = default)
    {
        if (_scheduler.IsStarted && _scheduler.InStandbyMode)
        {
            await _scheduler.Start(cancellationToken);
        }
    }

    public virtual async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_scheduler.IsStarted && !_scheduler.InStandbyMode)
        {
            await _scheduler.Standby(cancellationToken);
        }
    }

    public virtual async Task AddAsync(IBackgroundWorker worker, CancellationToken cancellationToken = default)
    {
        await ReScheduleJobAsync(worker, cancellationToken);
    }

    protected virtual async Task ReScheduleJobAsync(IBackgroundWorker worker, CancellationToken cancellationToken = default)
    {
        if (worker is IQuartzBackgroundWorker quartzWork)
        {
            Check.NotNull(quartzWork.Trigger, nameof(quartzWork.Trigger));
            Check.NotNull(quartzWork.JobDetail, nameof(quartzWork.JobDetail));

            if (quartzWork.ScheduleJob != null)
            {
                await quartzWork.ScheduleJob.Invoke(_scheduler);
            }
            else
            {
                await DefaultScheduleJobAsync(quartzWork, cancellationToken);
            }
        }
        else
        {
            var adapterType = typeof(QuartzPeriodicBackgroundWorkerAdapter<>).MakeGenericType(ProxyHelper.GetUnProxiedType(worker));

            var workerAdapter = Activator.CreateInstance(adapterType) as IQuartzBackgroundWorkerAdapter;

            workerAdapter?.BuildWorker(worker);

            if (workerAdapter?.Trigger != null)
            {
                await DefaultScheduleJobAsync(workerAdapter, cancellationToken);
            }
        }
    }

    protected virtual async Task DefaultScheduleJobAsync(IQuartzBackgroundWorker quartzWork, CancellationToken cancellationToken = default)
    {
        if (await _scheduler.CheckExists(quartzWork.JobDetail.Key, cancellationToken))
        {
            await _scheduler.AddJob(quartzWork.JobDetail, true, true, cancellationToken);
            await _scheduler.ResumeJob(quartzWork.JobDetail.Key, cancellationToken);
            await _scheduler.RescheduleJob(quartzWork.Trigger.Key, quartzWork.Trigger, cancellationToken);
        }
        else
        {
            await _scheduler.ScheduleJob(quartzWork.JobDetail, quartzWork.Trigger, cancellationToken);
        }
    }
}
