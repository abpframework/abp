using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Collections;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Distributed;

[Dependency(TryRegister = true)]
[ExposeServices(typeof(IDistributedEventBus), typeof(LocalDistributedEventBus))]
public class LocalDistributedEventBus : IDistributedEventBus, ISingletonDependency
{
    private readonly ILocalEventBus _localEventBus;

    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected AbpDistributedEventBusOptions AbpDistributedEventBusOptions { get; }

    public LocalDistributedEventBus(
        ILocalEventBus localEventBus,
        IUnitOfWorkManager unitOfWorkManager,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<AbpDistributedEventBusOptions> distributedEventBusOptions)
    {
        _localEventBus = localEventBus;
        UnitOfWorkManager = unitOfWorkManager;
        ServiceScopeFactory = serviceScopeFactory;
        AbpDistributedEventBusOptions = distributedEventBusOptions.Value;
        Subscribe(distributedEventBusOptions.Value.Handlers);
    }

    public virtual void Subscribe(ITypeList<IEventHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            var interfaces = handler.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(@interface))
                {
                    continue;
                }

                var genericArgs = @interface.GetGenericArguments();
                if (genericArgs.Length == 1)
                {
                    Subscribe(genericArgs[0], new IocEventHandlerFactory(ServiceScopeFactory, handler));
                }
            }
        }
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : class
    {
        return Subscribe(typeof(TEvent), handler);
    }

    public IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
    {
        return _localEventBus.Subscribe(action);
    }

    public IDisposable Subscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class
    {
        return _localEventBus.Subscribe(handler);
    }

    public IDisposable Subscribe<TEvent, THandler>() where TEvent : class where THandler : IEventHandler, new()
    {
        return _localEventBus.Subscribe<TEvent, THandler>();
    }

    public IDisposable Subscribe(Type eventType, IEventHandler handler)
    {
        return _localEventBus.Subscribe(eventType, handler);
    }

    public IDisposable Subscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
    {
        return _localEventBus.Subscribe<TEvent>(factory);
    }

    public IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
        return _localEventBus.Subscribe(eventType, factory);
    }

    public void Unsubscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
    {
        _localEventBus.Unsubscribe(action);
    }

    public void Unsubscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class
    {
        _localEventBus.Unsubscribe(handler);
    }

    public void Unsubscribe(Type eventType, IEventHandler handler)
    {
        _localEventBus.Unsubscribe(eventType, handler);
    }

    public void Unsubscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
    {
        _localEventBus.Unsubscribe<TEvent>(factory);
    }

    public void Unsubscribe(Type eventType, IEventHandlerFactory factory)
    {
        _localEventBus.Unsubscribe(eventType, factory);
    }

    public void UnsubscribeAll<TEvent>() where TEvent : class
    {
        _localEventBus.UnsubscribeAll<TEvent>();
    }

    public void UnsubscribeAll(Type eventType)
    {
        _localEventBus.UnsubscribeAll(eventType);
    }

    public async Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = true)
        where TEvent : class
    {
        await PublishAsync(typeof(TEvent), eventData, onUnitOfWorkComplete);
    }

    public async Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true)
    {
        if (onUnitOfWorkComplete && UnitOfWorkManager.Current != null)
        {
            AddToUnitOfWork(
                UnitOfWorkManager.Current,
                new UnitOfWorkEventRecord(eventType, eventData, EventOrderGenerator.GetNext())
            );
            return;
        }

        await _localEventBus.PublishAsync(eventType, eventData, onUnitOfWorkComplete: false);
    }

    public async Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = true, bool useOutbox = true)
        where TEvent : class
    {
        await PublishAsync(typeof(TEvent), eventData, onUnitOfWorkComplete, useOutbox);
    }

    public async Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true, bool useOutbox = true)
    {
        if (onUnitOfWorkComplete && UnitOfWorkManager.Current != null)
        {
            AddToUnitOfWork(
                UnitOfWorkManager.Current,
                new UnitOfWorkEventRecord(eventType, eventData, EventOrderGenerator.GetNext(), useOutbox)
            );
            return;
        }

        if (useOutbox && UnitOfWorkManager.Current != null)
        {
            UnitOfWorkManager.Current.OnCompleted(async() => {
                await _localEventBus.PublishAsync(eventType, eventData, onUnitOfWorkComplete: false);
            });
            return;
        }

        await _localEventBus.PublishAsync(eventType, eventData, onUnitOfWorkComplete: false);
    }

    protected virtual void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
    {
        unitOfWork.AddOrReplaceDistributedEvent(eventRecord);
    }
}
