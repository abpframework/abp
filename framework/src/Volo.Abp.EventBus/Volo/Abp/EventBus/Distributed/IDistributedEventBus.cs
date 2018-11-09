using System;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Distributed
{
    public interface IDistributedEventBus
    {
        Task PublishAsync<TEvent>(TEvent eventData)
            where TEvent : class;

        Task PublishAsync(Type eventType, object eventData);
    }
}
