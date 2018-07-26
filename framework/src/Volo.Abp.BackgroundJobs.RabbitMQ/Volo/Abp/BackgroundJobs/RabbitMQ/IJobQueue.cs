using System;
using System.Threading.Tasks;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public interface IJobQueue<TArgs> : IRunnable, IDisposable
    {
        Task<string> EnqueueAsync(
            TArgs args,
            BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null
        );
    }
}