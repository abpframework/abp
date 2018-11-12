using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus.Distributed
{
    [Dependency(TryRegister = true)]
    public class LocalDistributedEventBus : IDistributedEventBus, ITransientDependency
    {
        private readonly IEventBus _eventBus;

        public LocalDistributedEventBus(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
        {
            return _eventBus.Register(action);
        }

        public IDisposable Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : class
        {
            return _eventBus.Register(handler);
        }

        public IDisposable Subscribe<TEvent, THandler>() where TEvent : class where THandler : IEventHandler, new()
        {
            return _eventBus.Register<TEvent, THandler>();
        }

        public IDisposable Subscribe(Type eventType, IEventHandler handler)
        {
            return _eventBus.Register(eventType, handler);
        }

        public IDisposable Subscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
        {
            return _eventBus.Register<TEvent>(factory);
        }

        public IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            return _eventBus.Register(eventType, factory);
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