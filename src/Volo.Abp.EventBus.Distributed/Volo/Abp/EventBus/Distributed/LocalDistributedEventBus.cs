using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.Distributed
{
    public class LocalDistributedEventBus : IDistributedEventBus, ITransientDependency
    {
        private readonly IEventBus _eventBus;

        public LocalDistributedEventBus(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task PublishAsync<TEvent>(TEvent eventData)
            where TEvent : class
        {
            return _eventBus.TriggerAsync(eventData);
        }

        public Task PublishAsync(Type eventType, object eventData)
        {
            return _eventBus.TriggerAsync(eventType, eventData);
        }
    }
}