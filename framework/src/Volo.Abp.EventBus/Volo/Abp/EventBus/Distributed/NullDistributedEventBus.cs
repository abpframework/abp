using System;
using System.Threading.Tasks;

namespace Volo.Abp.EventBus.Distributed;

public sealed class NullDistributedEventBus : IDistributedEventBus
{
    public static NullDistributedEventBus Instance { get; } = new NullDistributedEventBus();

    private NullDistributedEventBus()
    {

    }

    public IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
    {
        return NullDisposable.Instance;
    }

    public IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : class
    {
        return NullDisposable.Instance;
    }

    public IDisposable Subscribe<TEvent, THandler>() where TEvent : class where THandler : IEventHandler, new()
    {
        return NullDisposable.Instance;
    }

    public IDisposable Subscribe(Type eventType, IEventHandler handler)
    {
        return NullDisposable.Instance;
    }

    public IDisposable Subscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
    {
        return NullDisposable.Instance;
    }

    public IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
        return NullDisposable.Instance;
    }

    public void Unsubscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
    {

    }

    public void Unsubscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class
    {

    }

    public void Unsubscribe(Type eventType, IEventHandler handler)
    {

    }

    public void Unsubscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
    {

    }

    public void Unsubscribe(Type eventType, IEventHandlerFactory factory)
    {

    }

    public void UnsubscribeAll<TEvent>() where TEvent : class
    {

    }

    public void UnsubscribeAll(Type eventType)
    {

    }

    public Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = true) where TEvent : class
    {
        return Task.CompletedTask;
    }

    public Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true)
    {
        return Task.CompletedTask;
    }

    public Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = true, bool useOutbox = true) where TEvent : class
    {
        return Task.CompletedTask;
    }

    public Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true, bool useOutbox = true)
    {
        return Task.CompletedTask;
    }
}
