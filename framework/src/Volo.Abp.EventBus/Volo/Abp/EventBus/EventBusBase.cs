using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Collections;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus;

public abstract class EventBusBase : IEventBus
{
    protected IServiceScopeFactory ServiceScopeFactory { get; }

    protected ICurrentTenant CurrentTenant { get; }

    protected IUnitOfWorkManager UnitOfWorkManager { get; }

    protected EventBusBase(
        IServiceScopeFactory serviceScopeFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager)
    {
        ServiceScopeFactory = serviceScopeFactory;
        CurrentTenant = currentTenant;
        UnitOfWorkManager = unitOfWorkManager;
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
    {
        return Subscribe(typeof(TEvent), new ActionEventHandler<TEvent>(action));
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe<TEvent, THandler>()
        where TEvent : class
        where THandler : IEventHandler, new()
    {
        return Subscribe(typeof(TEvent), new TransientEventHandlerFactory<THandler>());
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe(Type eventType, IEventHandler handler)
    {
        return Subscribe(eventType, new SingleInstanceHandlerFactory(handler));
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
    {
        return Subscribe(typeof(TEvent), factory);
    }

    public abstract IDisposable Subscribe(Type eventType, IEventHandlerFactory factory);

    public abstract void Unsubscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class;

    /// <inheritdoc/>
    public virtual void Unsubscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class
    {
        Unsubscribe(typeof(TEvent), handler);
    }

    public abstract void Unsubscribe(Type eventType, IEventHandler handler);

    /// <inheritdoc/>
    public virtual void Unsubscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
    {
        Unsubscribe(typeof(TEvent), factory);
    }

    public abstract void Unsubscribe(Type eventType, IEventHandlerFactory factory);

    /// <inheritdoc/>
    public virtual void UnsubscribeAll<TEvent>() where TEvent : class
    {
        UnsubscribeAll(typeof(TEvent));
    }

    /// <inheritdoc/>
    public abstract void UnsubscribeAll(Type eventType);

    /// <inheritdoc/>
    public Task PublishAsync<TEvent>(TEvent eventData, bool onUnitOfWorkComplete = true)
        where TEvent : class
    {
        return PublishAsync(typeof(TEvent), eventData, onUnitOfWorkComplete);
    }

    /// <inheritdoc/>
    public virtual async Task PublishAsync(
        Type eventType,
        object eventData,
        bool onUnitOfWorkComplete = true)
    {
        if (onUnitOfWorkComplete && UnitOfWorkManager.Current != null)
        {
            AddToUnitOfWork(
                UnitOfWorkManager.Current,
                new UnitOfWorkEventRecord(eventType, eventData, EventOrderGenerator.GetNext())
            );
            return;
        }

        await PublishToEventBusAsync(eventType, eventData);
    }

    protected abstract Task PublishToEventBusAsync(Type eventType, object eventData);

    protected abstract void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord);

    public virtual async Task TriggerHandlersAsync(Type eventType, object eventData)
    {
        var exceptions = new List<Exception>();

        await TriggerHandlersAsync(eventType, eventData, exceptions);

        if (exceptions.Any())
        {
            ThrowOriginalExceptions(eventType, exceptions);
        }
    }

    protected virtual async Task TriggerHandlersAsync(Type eventType, object eventData, List<Exception> exceptions, InboxConfig inboxConfig = null)
    {
        await new SynchronizationContextRemover();

        foreach (var handlerFactories in GetHandlerFactories(eventType))
        {
            foreach (var handlerFactory in handlerFactories.EventHandlerFactories)
            {
                await TriggerHandlerAsync(handlerFactory, handlerFactories.EventType, eventData, exceptions, inboxConfig);
            }
        }

        //Implements generic argument inheritance. See IEventDataWithInheritableGenericArgument
        if (eventType.GetTypeInfo().IsGenericType &&
            eventType.GetGenericArguments().Length == 1 &&
            typeof(IEventDataWithInheritableGenericArgument).IsAssignableFrom(eventType))
        {
            var genericArg = eventType.GetGenericArguments()[0];
            var baseArg = genericArg.GetTypeInfo().BaseType;
            if (baseArg != null)
            {
                var baseEventType = eventType.GetGenericTypeDefinition().MakeGenericType(baseArg);
                var constructorArgs = ((IEventDataWithInheritableGenericArgument)eventData).GetConstructorArgs();
                var baseEventData = Activator.CreateInstance(baseEventType, constructorArgs);
                await PublishToEventBusAsync(baseEventType, baseEventData);
            }
        }
    }

    protected void ThrowOriginalExceptions(Type eventType, List<Exception> exceptions)
    {
        if (exceptions.Count == 1)
        {
            exceptions[0].ReThrow();
        }

        throw new AggregateException(
            "More than one error has occurred while triggering the event: " + eventType,
            exceptions
        );
    }

    protected virtual void SubscribeHandlers(ITypeList<IEventHandler> handlers)
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

    protected abstract IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType);

    protected virtual async Task TriggerHandlerAsync(IEventHandlerFactory asyncHandlerFactory, Type eventType,
        object eventData, List<Exception> exceptions, InboxConfig inboxConfig = null)
    {
        using (var eventHandlerWrapper = asyncHandlerFactory.GetHandler())
        {
            try
            {
                var handlerType = eventHandlerWrapper.EventHandler.GetType();

                if (inboxConfig?.HandlerSelector != null &&
                    !inboxConfig.HandlerSelector(handlerType))
                {
                    return;
                }

                using (CurrentTenant.Change(GetEventDataTenantId(eventData)))
                {
                    if (ReflectionHelper.IsAssignableToGenericType(handlerType, typeof(ILocalEventHandler<>)))
                    {
                        var method = typeof(ILocalEventHandler<>)
                            .MakeGenericType(eventType)
                            .GetMethod(
                                nameof(ILocalEventHandler<object>.HandleEventAsync),
                                new[] { eventType }
                            );

                        await ((Task)method.Invoke(eventHandlerWrapper.EventHandler, new[] { eventData }));
                    }
                    else if (ReflectionHelper.IsAssignableToGenericType(handlerType, typeof(IDistributedEventHandler<>)))
                    {
                        var method = typeof(IDistributedEventHandler<>)
                            .MakeGenericType(eventType)
                            .GetMethod(
                                nameof(IDistributedEventHandler<object>.HandleEventAsync),
                                new[] { eventType }
                            );

                        await ((Task)method.Invoke(eventHandlerWrapper.EventHandler, new[] { eventData }));
                    }
                    else
                    {
                        throw new AbpException("The object instance is not an event handler. Object type: " + handlerType.AssemblyQualifiedName);
                    }
                }
            }
            catch (TargetInvocationException ex)
            {
                exceptions.Add(ex.InnerException);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }
    }

    protected virtual Guid? GetEventDataTenantId(object eventData)
    {
        return eventData switch
        {
            IMultiTenant multiTenantEventData => multiTenantEventData.TenantId,
            IEventDataMayHaveTenantId eventDataMayHaveTenantId when eventDataMayHaveTenantId.IsMultiTenant(out var tenantId) => tenantId,
            _ => CurrentTenant.Id
        };
    }

    protected class EventTypeWithEventHandlerFactories
    {
        public Type EventType { get; }

        public List<IEventHandlerFactory> EventHandlerFactories { get; }

        public EventTypeWithEventHandlerFactories(Type eventType, List<IEventHandlerFactory> eventHandlerFactories)
        {
            EventType = eventType;
            EventHandlerFactories = eventHandlerFactories;
        }
    }

    // Reference from
    // https://blogs.msdn.microsoft.com/benwilli/2017/02/09/an-alternative-to-configureawaitfalse-everywhere/
    protected struct SynchronizationContextRemover : INotifyCompletion
    {
        public bool IsCompleted
        {
            get { return SynchronizationContext.Current == null; }
        }

        public void OnCompleted(Action continuation)
        {
            var prevContext = SynchronizationContext.Current;
            try
            {
                SynchronizationContext.SetSynchronizationContext(null);
                continuation();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(prevContext);
            }
        }

        public SynchronizationContextRemover GetAwaiter()
        {
            return this;
        }

        public void GetResult()
        {
        }
    }
}
