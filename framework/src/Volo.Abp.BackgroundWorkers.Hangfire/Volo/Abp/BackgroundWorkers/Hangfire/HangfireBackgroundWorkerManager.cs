using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers.Hangfire
{
    [Dependency(ReplaceServices = true)]
    public class HangfireBackgroundWorkerManager : IBackgroundWorkerManager, ISingletonDependency
    {
        public IServiceScopeFactory ServiceScopeFactory { get; }

        public HangfireBackgroundWorkerManager(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public virtual Task StopAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public virtual void Add(IBackgroundWorker worker)
        {
            ReScheduleJob(worker);
        }

        protected virtual void ReScheduleJob(IBackgroundWorker worker)
        {
            if (worker is IHangfireBackgroundWorker hangfireWorker)
            {
                DefaultScheduleJob(hangfireWorker);
            }
            else
            {
                var adapterType = typeof(HangfirePeriodicBackgroundWorkerAdapter<>).MakeGenericType(worker.GetType());
                var workerAdapter = Activator.CreateInstance(adapterType, new object[]{ ServiceScopeFactory }) as IHangfireBackgroundWorkerAdapter;
                workerAdapter?.BuildWorker(worker);
                DefaultScheduleJob(workerAdapter);
            }
        }

        private static void DefaultScheduleJob(IHangfireBackgroundWorker hangfireWorker)
        {
            Check.NotNull(hangfireWorker, nameof(hangfireWorker));
            Check.NotNull(hangfireWorker.Schedule, nameof(hangfireWorker.Schedule));
            Check.NotNull(hangfireWorker.Name, nameof(hangfireWorker.Name));

            RecurringJob.RemoveIfExists(hangfireWorker.Name);
            RecurringJob.AddOrUpdate(hangfireWorker.Name, () => hangfireWorker.RunAsync(), hangfireWorker.Schedule);
        }
    }
}