using System;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.BackgroundWorkers.Quartz;

[Dependency(ReplaceServices = true)]
public class QuartzBackgroundWorkerManager : BackgroundWorkerManager, ISingletonDependency
{
    protected IScheduler Scheduler { get; }

    public QuartzBackgroundWorkerManager(IScheduler scheduler)
    {
        Scheduler = scheduler;
    }

    public async override Task StartAsync(CancellationToken cancellationToken = default)
    {
        if (Scheduler.IsStarted && Scheduler.InStandbyMode)
        {
            await Scheduler.Start(cancellationToken);
        }

        await base.StartAsync(cancellationToken);
    }

    public async override Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (Scheduler.IsStarted && !Scheduler.InStandbyMode)
        {
            await Scheduler.Standby(cancellationToken);
        }

        await base.StopAsync(cancellationToken);
    }

    public async override Task AddAsync(IBackgroundWorker worker, CancellationToken cancellationToken = default)
    {
        await ReScheduleJobAsync(worker, cancellationToken);
    }

    protected virtual async Task ReScheduleJobAsync(IBackgroundWorker worker, CancellationToken cancellationToken = default)
    {
        switch (worker)
        {
            case IQuartzBackgroundWorker quartzWork:
            {
                Check.NotNull(quartzWork.Trigger, nameof(quartzWork.Trigger));
                Check.NotNull(quartzWork.JobDetail, nameof(quartzWork.JobDetail));

                if (quartzWork.ScheduleJob != null)
                {
                    await quartzWork.ScheduleJob.Invoke(Scheduler);
                }
                else
                {
                    await DefaultScheduleJobAsync(quartzWork, cancellationToken);
                }

                break;
            }
            case AsyncPeriodicBackgroundWorkerBase or PeriodicBackgroundWorkerBase:
            {
                var adapterType = typeof(QuartzPeriodicBackgroundWorkerAdapter<>).MakeGenericType(ProxyHelper.GetUnProxiedType(worker));

                var workerAdapter = Activator.CreateInstance(adapterType) as IQuartzBackgroundWorkerAdapter;

                workerAdapter?.BuildWorker(worker);

                if (workerAdapter?.Trigger != null)
                {
                    await DefaultScheduleJobAsync(workerAdapter, cancellationToken);
                }

                break;
            }
            default:
                await base.AddAsync(worker, cancellationToken);
                break;
        }
    }

    protected virtual async Task DefaultScheduleJobAsync(IQuartzBackgroundWorker quartzWork, CancellationToken cancellationToken = default)
    {
        if (await Scheduler.CheckExists(quartzWork.JobDetail.Key, cancellationToken))
        {
            await Scheduler.AddJob(quartzWork.JobDetail, true, true, cancellationToken);
            await Scheduler.ResumeJob(quartzWork.JobDetail.Key, cancellationToken);
            await Scheduler.RescheduleJob(quartzWork.Trigger.Key, quartzWork.Trigger, cancellationToken);
        }
        else
        {
            await Scheduler.ScheduleJob(quartzWork.JobDetail, quartzWork.Trigger, cancellationToken);
        }
    }
}
