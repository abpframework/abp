using System.Threading.Tasks;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public interface IJobQueueManager : IRunnable
    {
        Task<IJobQueue<TArgs>> GetAsync<TArgs>();
    }
}