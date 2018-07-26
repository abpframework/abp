using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public interface IJobQueueManager : IRunnable
    {
        IJobQueue<TArgs> Get<TArgs>();
    }
}