using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public class JobQueueManager : IJobQueueManager, ISingletonDependency
    {
        protected ConcurrentDictionary<string, IRunnable> JobQueues { get; }

        protected IServiceProvider ServiceProvider { get; }

        protected AbpBackgroundJobOptions Options { get; }

        public JobQueueManager(
            IOptions<AbpBackgroundJobOptions> options,
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Options = options.Value;
            JobQueues = new ConcurrentDictionary<string, IRunnable>();
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            if (!Options.IsJobExecutionEnabled)
            {
                return;
            }

            foreach (var jobConfiguration in Options.GetJobs())
            {
                var jobQueue = (IRunnable)ServiceProvider.GetRequiredService(typeof(IJobQueue<>).MakeGenericType(jobConfiguration.ArgsType));
                await jobQueue.StartAsync(cancellationToken);
                JobQueues[jobConfiguration.JobName] = jobQueue;
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            foreach (var jobQueue in JobQueues.Values)
            {
                await jobQueue.StopAsync(cancellationToken);
            }

            JobQueues.Clear();
        }

        public IJobQueue<TArgs> Get<TArgs>()
        {
            var jobConfiguration = Options.GetJob(typeof(TArgs));

            return (IJobQueue<TArgs>)JobQueues.GetOrAdd(jobConfiguration.JobName, _ =>
            {
                var jobQueue = (IRunnable) ServiceProvider
                    .GetRequiredService(typeof(IJobQueue<>)
                        .MakeGenericType(typeof(TArgs)));

                AsyncHelper.RunSync(() => jobQueue.StartAsync());

                return jobQueue;
            });
        }
    }
}
