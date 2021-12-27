using System;
using System.Threading.Tasks;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.RabbitMQ;

public interface IJobQueue<in TArgs> : IRunnable, IDisposable
{
    Task<string> EnqueueAsync(
        TArgs args,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null
    );
        
    Task<string> EnqueueAsync(
        TArgs args,
        DateTime executionTime,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal
    );
}