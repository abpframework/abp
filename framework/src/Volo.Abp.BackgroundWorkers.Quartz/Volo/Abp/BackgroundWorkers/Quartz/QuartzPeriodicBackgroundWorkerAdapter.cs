using System;
using System.Reflection;
using System.Threading.Tasks;
using Quartz;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers.Quartz
{
    [DisallowConcurrentExecution]
    public class QuartzPeriodicBackgroundWorkerAdapter<TWorker> : QuartzBackgroundWorkerBase,
        IQuartzBackgroundWorkerAdapter
        where TWorker : IBackgroundWorker
    {
        private readonly MethodInfo _doWorkAsyncMethod;
        private readonly MethodInfo _doWorkMethod;

        public QuartzPeriodicBackgroundWorkerAdapter()
        {
            AutoRegister = false;

            _doWorkAsyncMethod = typeof(TWorker).GetMethod("DoWorkAsync", BindingFlags.Instance | BindingFlags.NonPublic);
            _doWorkMethod = typeof(TWorker).GetMethod("DoWork", BindingFlags.Instance | BindingFlags.NonPublic);

        }

        public void BuildWorker(IBackgroundWorker worker)
        {
            int? period;
            var workerType = worker.GetType();

            if (worker is AsyncPeriodicBackgroundWorkerBase or PeriodicBackgroundWorkerBase)
            {
                if (typeof(TWorker) != worker.GetType())
                {
                    throw new ArgumentException($"{nameof(worker)} type is different from the generic type");
                }

                var timer = worker.GetType().GetProperty("Timer", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(worker);

                if (worker is AsyncPeriodicBackgroundWorkerBase)
                {
                    period = ((AbpAsyncTimer)timer)?.Period;
                }
                else
                {
                    period = ((AbpTimer)timer)?.Period;
                }
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
                    if (_doWorkAsyncMethod != null)
                    {
                        await (Task) _doWorkAsyncMethod.Invoke(asyncWorker, new object[] {workerContext});
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
}
