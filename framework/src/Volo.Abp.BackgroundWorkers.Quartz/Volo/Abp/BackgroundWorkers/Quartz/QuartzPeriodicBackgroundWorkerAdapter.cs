using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Volo.Abp.BackgroundWorkers.Quartz;

[DisallowConcurrentExecution]
public class QuartzPeriodicBackgroundWorkerAdapter<TWorker> : QuartzBackgroundWorkerBase,
    IQuartzBackgroundWorkerAdapter
    where TWorker : IBackgroundWorker
{
    private readonly MethodInfo? _doWorkAsyncMethod;
    private readonly MethodInfo? _doWorkMethod;

    public QuartzPeriodicBackgroundWorkerAdapter()
    {
        AutoRegister = false;

        _doWorkAsyncMethod = typeof(TWorker).GetMethod("DoWorkAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        _doWorkMethod = typeof(TWorker).GetMethod("DoWork", BindingFlags.Instance | BindingFlags.NonPublic);

    }

    public virtual void BuildWorker(IBackgroundWorker worker)
    {
        int? period = null;
        string? CronExpression = null;

        if (worker is AsyncPeriodicBackgroundWorkerBase asyncPeriodicBackgroundWorkerBase)
        {
            period = asyncPeriodicBackgroundWorkerBase.Period;
            CronExpression = asyncPeriodicBackgroundWorkerBase.CronExpression;
        }
        else if (worker is PeriodicBackgroundWorkerBase periodicBackgroundWorkerBase)
        {
            period = periodicBackgroundWorkerBase.Period;
            CronExpression = periodicBackgroundWorkerBase.CronExpression;
        }

        if (period == null && CronExpression.IsNullOrWhiteSpace())
        {
            return;
        }

        JobDetail = JobBuilder
            .Create<QuartzPeriodicBackgroundWorkerAdapter<TWorker>>()
            .WithIdentity(BackgroundWorkerNameAttribute.GetName<TWorker>())
            .Build();

        var triggerBuilder = TriggerBuilder.Create()
            .ForJob(JobDetail)
            .WithIdentity(BackgroundWorkerNameAttribute.GetName<TWorker>());

        if (!CronExpression.IsNullOrWhiteSpace())
        {
            triggerBuilder.WithCronSchedule(CronExpression);
        }
        else
        {
            triggerBuilder.WithSimpleSchedule(builder => builder.WithInterval(TimeSpan.FromMilliseconds(period!.Value)).RepeatForever());
        }

        Trigger = triggerBuilder.Build();
    }

    public async override Task Execute(IJobExecutionContext context)
    {
        var worker = (IBackgroundWorker) ServiceProvider.GetRequiredService(typeof(TWorker));
        var workerContext = new PeriodicBackgroundWorkerContext(ServiceProvider, context.CancellationToken);

        switch (worker)
        {
            case AsyncPeriodicBackgroundWorkerBase asyncWorker:
            {
                if (_doWorkAsyncMethod != null)
                {
                    await (Task) (_doWorkAsyncMethod.Invoke(asyncWorker, new object[] {workerContext})!);
                }

                break;
            }
            case PeriodicBackgroundWorkerBase syncWorker:
            {
                _doWorkMethod?.Invoke(syncWorker, new object[] {workerContext});

                break;
            }
        }
    }
}
