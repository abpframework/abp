using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.Distributed.RabbitMq
{
    public class RabbitMqDistributedEventBus : IDistributedEventBus, ITransientDependency
    {
        public IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : class
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe<TEvent, THandler>() where TEvent : class where THandler : IEventHandler, new()
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(Type eventType, IEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            throw new NotImplementedException();
        }

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