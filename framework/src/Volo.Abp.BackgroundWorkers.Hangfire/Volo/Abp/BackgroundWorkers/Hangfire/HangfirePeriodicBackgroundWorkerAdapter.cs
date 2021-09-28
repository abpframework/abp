using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers.Hangfire
{
    public class HangfirePeriodicBackgroundWorkerAdapter<TWorker> : HangfireBackgroundWorkerBase,
        IHangfireBackgroundWorkerAdapter
        where TWorker : IBackgroundWorker
    {
        private readonly MethodInfo _doWorkAsyncMethod;
        private readonly MethodInfo _doWorkMethod;

        public HangfirePeriodicBackgroundWorkerAdapter(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            _doWorkAsyncMethod = typeof(TWorker).GetMethod("DoWorkAsync", BindingFlags.Instance | BindingFlags.NonPublic);
            _doWorkMethod = typeof(TWorker).GetMethod("DoWork", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public void BuildWorker(IBackgroundWorker worker)
        {
            int? period;
            if (worker is AsyncPeriodicBackgroundWorkerBase || worker is PeriodicBackgroundWorkerBase)
            {
                if (typeof(TWorker) != worker.GetType())
                {
                    throw new ArgumentException($"{nameof(worker)} type is different from the generic type");
                }

                var timer = (AbpAsyncTimer)worker.GetType()
                    .GetProperty("Timer", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?.GetValue(worker);

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

            var minutes = Math.Round(period.Value / 6000d);
            Schedule = $"*/{minutes} * * * *";
            Name = typeof(TWorker).Name;
        }

        public override async Task DoWorkAsync(HangfireBackgroundWorkerContext workerContext)
        {
            var worker = (IBackgroundWorker)workerContext.ServiceProvider.GetService(typeof(TWorker));
            switch (worker)
            {
                case AsyncPeriodicBackgroundWorkerBase asyncWorker:
                {
                    if (_doWorkAsyncMethod != null)
                    {
                        await (Task)_doWorkAsyncMethod.Invoke(asyncWorker, new object[] { workerContext });
                    }

                    break;
                }
                case PeriodicBackgroundWorkerBase syncWorker:
                {
                    _doWorkMethod?.Invoke(syncWorker, new object[] { workerContext });

                    break;
                }
            }
        }
    }
}