using System;
using System.Reflection;
using System.Threading.Tasks;
using Quartz;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers.Quartz
{
    public class QuartzPeriodicBackgroundWorkerAdapter<TWorker> : QuartzBackgroundWorkerBase,
        IQuartzBackgroundWorkerAdapter
        where TWorker : IBackgroundWorker
    {
        public QuartzPeriodicBackgroundWorkerAdapter()
        {
            AutoRegister = false;
        }

        public void BuildWorker(IBackgroundWorker worker)
        {
            int? period;
            var workerType = worker.GetType();

            if (worker is AsyncPeriodicBackgroundWorkerBase || worker is PeriodicBackgroundWorkerBase)
            {
                if (typeof(TWorker) != worker.GetType())
                {
                    throw new ArgumentException($"{nameof(worker)} type is different from the generic type");
                }

                var timer = (AbpTimer) worker.GetType().GetProperty("Timer", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(worker);
                period = timer?.Period;
            }
            else
            {
                return;
            }

            if (period == null)
            {
                return;
            }

            JobDetail = JobBuilder
                .Create<QuartzPeriodicBackgroundWorkerAdapter<TWorker>>()
                .WithIdentity(workerType.FullName)
                .Build();
            Trigger = TriggerBuilder.Create()
                .WithIdentity(workerType.FullName)
                .WithSimpleSchedule(builder => builder.WithInterval(TimeSpan.FromMilliseconds(period.Value)).RepeatForever())
                .Build();
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            var worker = (IBackgroundWorker) ServiceProvider.GetService(typeof(TWorker));
            var workerContext = new PeriodicBackgroundWorkerContext(ServiceProvider);

            switch (worker)
            {
                case AsyncPeriodicBackgroundWorkerBase asyncWorker:
                {
                    var doWorkAsyncMethod = asyncWorker.GetType()
                        .GetMethod("DoWorkAsync", BindingFlags.Instance | BindingFlags.NonPublic);

                    if (doWorkAsyncMethod != null)
                    {
                        await (Task) doWorkAsyncMethod.Invoke(asyncWorker, new object[] {workerContext});
                    }

                    break;
                }
                case PeriodicBackgroundWorkerBase syncWorker:
                {
                    var doWorkMethod = syncWorker.GetType()
                        .GetMethod("DoWork", BindingFlags.Instance | BindingFlags.NonPublic);

                    if (doWorkMethod != null)
                    {
                        doWorkMethod.Invoke(syncWorker, new object[] {workerContext});
                    }

                    break;
                }
            }
        }
    }
}
