using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.Distributed.RabbitMq
{
    public class RabbitMqDistributedEventBus : IDistributedEventBus, ITransientDependency
    {
        public Task PublishAsync<TEvent>(TEvent eventData)
            where TEvent : class
        {
            throw new NotImplementedException();
        }

        public Task PublishAsync(Type eventType, object eventData)
        {
            throw new NotImplementedException();
        }
    }
}