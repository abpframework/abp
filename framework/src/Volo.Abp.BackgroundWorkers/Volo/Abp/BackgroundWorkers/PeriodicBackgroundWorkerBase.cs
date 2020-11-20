using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers
{
    /// <summary>
    /// Extends <see cref="BackgroundWorkerBase"/> to add a periodic running Timer.
    /// </summary>
    public abstract class PeriodicBackgroundWorkerBase : BackgroundWorkerBase
    {
        protected IServiceScopeFactory ServiceScopeFactory { get; }
        protected AbpTimer Timer { get; }

        protected PeriodicBackgroundWorkerBase(
            AbpTimer timer,
            IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
            Timer = timer;
            Timer.Elapsed += Timer_Elapsed;
        }

        public async override Task StartAsync(CancellationToken cancellationToken = default)
        {
            await base.StartAsync(cancellationToken);
            Timer.Start(cancellationToken);
        }

        public async override Task StopAsync(CancellationToken cancellationToken = default)
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

                    DoWork(new PeriodicBackgroundWorkerContext(scope.ServiceProvider));
                }
                catch (Exception ex)
                {
                    scope.ServiceProvider
                        .GetRequiredService<IExceptionNotifier>()
                        .NotifyAsync(new ExceptionNotificationContext(ex));

                    Logger.LogException(ex);
                }
            }
        }

        /// <summary>
        /// Periodic works should be done by implementing this method.
        /// </summary>
        protected abstract void DoWork(PeriodicBackgroundWorkerContext workerContext);
    }
}
