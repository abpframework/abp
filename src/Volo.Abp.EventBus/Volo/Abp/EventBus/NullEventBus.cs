using System;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Factories;
using Volo.Abp.EventBus.Handlers;

namespace Volo.Abp.EventBus
{
    public sealed class NullEventBus : IEventBus
    {
        public static NullEventBus Instance { get; } = new NullEventBus();

        private NullEventBus()
        {
            
        }

        public IDisposable Register<TEventData>(Action<TEventData> action) where TEventData : class
        {
            return NullDisposable.Instance;
        }

        public IDisposable AsyncRegister<TEventData>(Func<TEventData, Task> action) where TEventData : class
        {
            return NullDisposable.Instance;
        }

        public IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : class
        {
            return NullDisposable.Instance;
        }

        public IDisposable AsyncRegister<TEventData>(IAsyncEventHandler<TEventData> handler) where TEventData : class
        {
            return NullDisposable.Instance;
        }

        public IDisposable Register<TEventData, THandler>() where TEventData : class where THandler : IEventHandler, new()
        {
            return NullDisposable.Instance;
        }

        public IDisposable Register(Type eventType, IEventHandler handler)
        {
            return NullDisposable.Instance;
        }

        public IDisposable Register<TEventData>(IEventHandlerFactory factory) where TEventData : class
        {
            return NullDisposable.Instance;
        }

        public IDisposable Register(Type eventType, IEventHandlerFactory factory)
        {
            return NullDisposable.Instance;
        }

        public void Unregister<TEventData>(Action<TEventData> action) where TEventData : class
        {

        }

        public void AsyncUnregister<TEventData>(Func<TEventData, Task> action) where TEventData : class
        {
            
        }

        public void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : class
        {
            
        }

        public void AsyncUnregister<TEventData>(IAsyncEventHandler<TEventData> handler) where TEventData : class
        {
            
        }

        public void Unregister(Type eventType, IEventHandler handler)
        {
            
        }

        public void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : class
        {
            
        }

        public void Unregister(Type eventType, IEventHandlerFactory factory)
        {
            
        }

        public void UnregisterAll<TEventData>() where TEventData : class
        {
            
        }

        public void UnregisterAll(Type eventType)
        {
            
        }

        public void Trigger<TEventData>(TEventData eventData) where TEventData : class
        {
            
        }

        public void Trigger(Type eventType, object eventData)
        {
            
        }

        public Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : class
        {
            return Task.CompletedTask;
        }

        public Task TriggerAsync(Type eventType, object eventData)
        {
            return Task.CompletedTask;
        }
    }
}
