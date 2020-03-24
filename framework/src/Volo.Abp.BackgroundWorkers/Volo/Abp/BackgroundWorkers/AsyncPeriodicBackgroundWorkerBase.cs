using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers
{
    public abstract class AsyncPeriodicBackgroundWorkerBase : BackgroundWorkerBase
    {
        protected IServiceScopeFactory ServiceScopeFactory { get; }
        protected AbpTimer Timer { get; }

        protected AsyncPeriodicBackgroundWorkerBase(
            AbpTimer timer,
            IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Timer = timer;
            Timer.Elapsed += Timer_Elapsed;
        }

        public override async Task StartAsync(CancellationToken cancellationToken = default)
        {
            await base.StartAsync(cancellationToken);
            Timer.Start(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken = default)
        {
            Timer.Stop(cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        private void Timer_Elapsed(object sender, System.EventArgs e)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                try
                {
                    AsyncHelper.RunSync(
                        () => DoWorkAsync(new PeriodicBackgroundWorkerContext(scope.ServiceProvider))
                    );
                }
                catch (Exception ex)
                {
                    AsyncHelper.RunSync(
                        () => scope.ServiceProvider
                            .GetRequiredService<IExceptionNotifier>()
                            .NotifyAsync(new ExceptionNotificationContext(ex))
                    );

                    Logger.LogException(ex);
                }
            }
        }

        protected abstract Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext);
    }
}