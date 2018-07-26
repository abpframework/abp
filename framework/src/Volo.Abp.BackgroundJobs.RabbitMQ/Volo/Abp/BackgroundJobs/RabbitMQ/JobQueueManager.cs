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
        protected BackgroundJobOptions Options { get; }

        public JobQueueManager(
            IOptions<BackgroundJobOptions> options,
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

            foreach (var item in Options.JobTypes)
            {
                var jobName = item.Key;
                var jobType = item.Value;
                var argsType = BackgroundJobArgsHelper.GetJobArgsType(jobType);

                var jobQueue = (IRunnable)ServiceProvider.GetRequiredService(typeof(IJobQueue<>).MakeGenericType(argsType));
                await jobQueue.StartAsync(cancellationToken);
                JobQueues[jobName] = jobQueue;
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
            var jobName = BackgroundJobNameAttribute.GetName(typeof(TArgs));

            if (!Options.JobTypes.ContainsKey(jobName))
            {
                throw new AbpException("No job registered");
            }

            return (IJobQueue<TArgs>)JobQueues.GetOrAdd(jobName, _ =>
            {
                var jobQueue = (IRunnable)ServiceProvider.GetRequiredService(typeof(IJobQueue<>).MakeGenericType(typeof(TArgs)));
                jobQueue.Start();
                return jobQueue;
            });
        }
    }
}
