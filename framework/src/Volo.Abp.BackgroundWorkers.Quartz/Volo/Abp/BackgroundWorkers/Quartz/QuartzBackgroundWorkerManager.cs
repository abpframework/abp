using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.BackgroundWorkers.Quartz;

[Dependency(ReplaceServices = true)]
public class QuartzBackgroundWorkerManager : BackgroundWorkerManager, ISingletonDependency
{
    public const string DynamicWorkerNameKey = "AbpDynamicWorkerName";

    protected IScheduler Scheduler { get; }

    public QuartzBackgroundWorkerManager(
        IScheduler scheduler,
        IServiceProvider serviceProvider,
        IDynamicBackgroundWorkerHandlerRegistry dynamicBackgroundWorkerHandlerRegistry)
        : base(serviceProvider, dynamicBackgroundWorkerHandlerRegistry)
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

    public override Task AddAsync(
        string workerName,
        Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default)
    {
        return AddAsync(
            workerName,
            new DynamicBackgroundWorkerSchedule
            {
                Period = DynamicBackgroundWorkerSchedule.DefaultPeriod
            },
            handler,
            cancellationToken
        );
    }

    public override async Task AddAsync(
        string workerName,
        DynamicBackgroundWorkerSchedule schedule,
        Func<DynamicBackgroundWorkerExecutionContext, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));
        Check.NotNull(handler, nameof(handler));

        schedule.Validate();

        if (schedule.Period == null && schedule.CronExpression.IsNullOrWhiteSpace())
        {
            throw new AbpException($"Both 'Period' and 'CronExpression' are not set for dynamic worker {workerName}. You must set at least one of them.");
        }

        var jobKey = new JobKey($"DynamicWorker:{workerName}");
        var triggerKey = new TriggerKey($"DynamicWorker:{workerName}");
        var jobDetail = JobBuilder.Create<QuartzDynamicBackgroundWorkerAdapter>()
            .WithIdentity(jobKey)
            .UsingJobData(DynamicWorkerNameKey, workerName)
            .Build();

        var triggerBuilder = TriggerBuilder.Create()
            .ForJob(jobDetail)
            .WithIdentity(triggerKey);

        if (!schedule.CronExpression.IsNullOrWhiteSpace())
        {
            triggerBuilder.WithCronSchedule(schedule.CronExpression);
        }
        else
        {
            triggerBuilder.WithSimpleSchedule(builder =>
                builder.WithInterval(TimeSpan.FromMilliseconds(schedule.Period!.Value)).RepeatForever());
        }

        var trigger = triggerBuilder.Build();

        if (await Scheduler.CheckExists(jobDetail.Key, cancellationToken))
        {
            await Scheduler.AddJob(jobDetail, true, true, cancellationToken);
            await Scheduler.ResumeJob(jobDetail.Key, cancellationToken);
            await Scheduler.RescheduleJob(trigger.Key, trigger, cancellationToken);
        }
        else
        {
            await Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken);
        }

        DynamicBackgroundWorkerHandlerRegistry.Register(workerName, handler);
    }

    public override async Task<bool> RemoveAsync(string workerName, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));

        if (!DynamicBackgroundWorkerHandlerRegistry.IsRegistered(workerName))
        {
            return false;
        }

        var jobKey = new JobKey($"DynamicWorker:{workerName}");
        var deleted = await Scheduler.DeleteJob(jobKey, cancellationToken);
        if (deleted)
        {
            DynamicBackgroundWorkerHandlerRegistry.Unregister(workerName);
        }

        return deleted;
    }

    public override async Task<bool> UpdateScheduleAsync(string workerName, DynamicBackgroundWorkerSchedule schedule, CancellationToken cancellationToken = default)
    {
        Check.NotNullOrWhiteSpace(workerName, nameof(workerName));
        Check.NotNull(schedule, nameof(schedule));

        schedule.Validate();

        if (!DynamicBackgroundWorkerHandlerRegistry.IsRegistered(workerName))
        {
            return false;
        }

        if (schedule.Period == null && schedule.CronExpression.IsNullOrWhiteSpace())
        {
            throw new AbpException($"Both 'Period' and 'CronExpression' are not set for dynamic worker {workerName}. You must set at least one of them.");
        }

        var triggerKey = new TriggerKey($"DynamicWorker:{workerName}");
        var triggerBuilder = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .ForJob(new JobKey($"DynamicWorker:{workerName}"));

        if (!schedule.CronExpression.IsNullOrWhiteSpace())
        {
            triggerBuilder.WithCronSchedule(schedule.CronExpression);
        }
        else
        {
            triggerBuilder.WithSimpleSchedule(builder =>
                builder.WithInterval(TimeSpan.FromMilliseconds(schedule.Period!.Value)).RepeatForever());
        }

        var result = await Scheduler.RescheduleJob(triggerKey, triggerBuilder.Build(), cancellationToken);

        return result != null;
    }
}
