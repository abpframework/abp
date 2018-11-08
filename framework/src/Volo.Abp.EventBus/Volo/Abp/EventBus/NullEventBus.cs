using System;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus
{
    public sealed class NullEventBus : IEventBus
    {
        public static NullEventBus Instance { get; } = new NullEventBus();

        private NullEventBus()
        {
            
        }

        public IDisposable Register<TEvent>(Func<TEvent, Task> action) where TEvent : class
        {
            return NullDisposable.Instance;
        }

        public IDisposable Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : class
        {
            return NullDisposable.Instance;
        }

        public IDisposable Register<TEvent, THandler>() where TEvent : class where THandler : IEventHandler, new()
        {
            return NullDisposable.Instance;
        }

        public IDisposable Register(Type eventType, IEventHandler handler)
        {
            return NullDisposable.Instance;
        }

        public IDisposable Register<TEvent>(IEventHandlerFactory factory) where TEvent : class
        {
            return NullDisposable.Instance;
        }

        public IDisposable Register(Type eventType, IEventHandlerFactory factory)
        {
            return NullDisposable.Instance;
        }

        public void Unregister<TEvent>(Func<TEvent, Task> action) where TEvent : class
        {
            
        }

        public void Unregister<TEvent>(IEventHandler<TEvent> handler) where TEvent : class
        {
            
        }

        public void Unregister(Type eventType, IEventHandler handler)
        {
            
        }

        public void Unregister<TEvent>(IEventHandlerFactory factory) where TEvent : class
        {
            
        }

        public void Unregister(Type eventType, IEventHandlerFactory factory)
        {
            
        }

        public void UnregisterAll<TEvent>() where TEvent : class
        {
            
        }

        public void UnregisterAll(Type eventType)
        {
            
        }

        public Task TriggerAsync<TEvent>(TEvent eventData) where TEvent : class
        {
            return Task.CompletedTask;
        }

        public Task TriggerAsync(Type eventType, object eventData)
        {
            return Task.CompletedTask;
        }
    }
}
