using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.ExceptionHandling;

namespace Volo.Abp.BackgroundWorkers.Hangfire
{
    public abstract class HangfireBackgroundWorkerBase : BackgroundWorkerBase, IHangfireBackgroundWorker
    {
        private IServiceScopeFactory ServiceScopeFactory { get; }

        public string Name { get; set; }

        public string Schedule { get; set; }

        public object Options { get; set; }

        protected HangfireBackgroundWorkerBase(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

        public override async Task StartAsync(CancellationToken cancellationToken = default)
        {
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken = default)
        {
            await base.StopAsync(cancellationToken);
        }


        public async Task RunAsync()
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                try
                {
                    await DoWorkAsync(new HangfireBackgroundWorkerContext(scope.ServiceProvider));
                }
                catch (Exception ex)
                {
                    await scope.ServiceProvider
                        .GetRequiredService<IExceptionNotifier>()
                        .NotifyAsync(new ExceptionNotificationContext(ex));

                    Logger.LogException(ex);
                }
            }
        }

        public abstract Task DoWorkAsync(HangfireBackgroundWorkerContext workerContext);
    }
}