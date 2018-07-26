using System;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    public interface IJobQueue<TArgs> : IDisposable
    {
        Task<string> Enqueue(TArgs args);

        Task StartAsync();
    }
}