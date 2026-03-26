using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Local;

/// <summary>
/// Implements EventBus as Singleton pattern.
/// </summary>
[ExposeServices(typeof(ILocalEventBus), typeof(LocalEventBus))]
public class LocalEventBus : EventBusBase, ILocalEventBus, ISingletonDependency
{
    /// <summary>
    /// Reference to the Logger.
    /// </summary>
    public ILogger<LocalEventBus> Logger { get; set; }

    protected AbpLocalEventBusOptions Options { get; }

    protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }

    protected ConcurrentDictionary<string, Type> EventTypes { get; }

    protected ConcurrentDictionary<string, List<IEventHandlerFactory>> DynamicEventHandlerFactories { get; }

    public LocalEventBus(
        IOptions<AbpLocalEventBusOptions> options,
        IServiceScopeFactory serviceScopeFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IEventHandlerInvoker eventHandlerInvoker)
        : base(serviceScopeFactory, currentTenant, unitOfWorkManager, eventHandlerInvoker)
    {
        Options = options.Value;
        Logger = NullLogger<LocalEventBus>.Instance;

        HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        EventTypes = new ConcurrentDictionary<string, Type>();
        DynamicEventHandlerFactories = new ConcurrentDictionary<string, List<IEventHandlerFactory>>();
        SubscribeHandlers(Options.Handlers);
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class
    {
        return Subscribe(typeof(TEvent), handler);
    }

    /// <inheritdoc/>
    public override IDisposable Subscribe(string eventName, IEventHandlerFactory handler)
    {
        GetOrCreateDynamicHandlerFactories(eventName).Locking(factories =>
        {
            if (!handler.IsInFactories(factories))
            {
                factories.Add(handler);
            }
        });

        return new DynamicEventHandlerFactoryUnregistrar(this, eventName, handler);
    }

    /// <inheritdoc/>
    public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
        EventTypes.GetOrAdd(EventNameAttribute.GetNameOrDefault(eventType), eventType);

        GetOrCreateHandlerFactories(eventType)
            .Locking(factories =>
                {
                    if (!factory.IsInFactories(factories))
                    {
                        factories.Add(factory);
                    }
                }
            );

        return new EventHandlerFactoryUnregistrar(this, eventType, factory);
    }

    /// <inheritdoc/>
    public override void Unsubscribe<TEvent>(Func<TEvent, Task> action)
    {
        Check.NotNull(action, nameof(action));

        GetOrCreateHandlerFactories(typeof(TEvent))
            .Locking(factories =>
            {
                factories.RemoveAll(
                    factory =>
                    {
                        var singleInstanceFactory = factory as SingleInstanceHandlerFactory;
                        if (singleInstanceFactory == null)
                        {
                            return false;
                        }

                        var actionHandler = singleInstanceFactory.HandlerInstance as ActionEventHandler<TEvent>;
                        if (actionHandler == null)
                        {
                            return false;
                        }

                        return actionHandler.Action == action;
                    });
            });
    }

    /// <inheritdoc/>
    public override void Unsubscribe(Type eventType, IEventHandler handler)
    {
        GetOrCreateHandlerFactories(eventType)
            .Locking(factories =>
            {
                factories.RemoveAll(
                    factory =>
                        factory is SingleInstanceHandlerFactory &&
                        ((factory as SingleInstanceHandlerFactory)!).HandlerInstance == handler
                );
            });
    }

    /// <inheritdoc/>
    public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
    }

    /// <inheritdoc/>
    public override void Unsubscribe(string eventName, IEventHandlerFactory factory)
    {
        GetOrCreateDynamicHandlerFactories(eventName).Locking(factories => factories.Remove(factory));
    }

    /// <inheritdoc/>
    public override void Unsubscribe(string eventName, IEventHandler handler)
    {
        GetOrCreateDynamicHandlerFactories(eventName)
            .Locking(factories =>
            {
                factories.RemoveAll(
                    factory =>
                        factory is SingleInstanceHandlerFactory singleFactory &&
                        singleFactory.HandlerInstance == handler
                );
            });
    }

    /// <inheritdoc/>
    public override void UnsubscribeAll(Type eventType)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
    }

    /// <inheritdoc/>
    public override void UnsubscribeAll(string eventName)
    {
        GetOrCreateDynamicHandlerFactories(eventName).Locking(factories => factories.Clear());
    }

    /// <inheritdoc/>
    public override Task PublishAsync(string eventName, object eventData, bool onUnitOfWorkComplete = true)
    {
        var eventType = EventTypes.GetOrDefault(eventName);

        var dynamicEventData = eventData as DynamicEventData ?? new DynamicEventData(eventName, eventData);

        if (eventType != null)
        {
            return PublishAsync(eventType, ConvertDynamicEventData(dynamicEventData.Data, eventType), onUnitOfWorkComplete);
        }

        return PublishAsync(typeof(DynamicEventData), dynamicEventData, onUnitOfWorkComplete);
    }

    protected override async Task PublishToEventBusAsync(Type eventType, object eventData)
    {
        await PublishAsync(new LocalEventMessage(Guid.NewGuid(), eventData, eventType));
    }

    protected override void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
    {
        unitOfWork.AddOrReplaceLocalEvent(eventRecord);
    }

    public virtual async Task PublishAsync(LocalEventMessage localEventMessage)
    {
        await TriggerHandlersAsync(localEventMessage.EventType, localEventMessage.EventData);
    }

    public virtual List<EventTypeWithEventHandlerFactories> GetEventHandlerFactories(Type eventType)
    {
        return GetHandlerFactories(eventType).ToList();
    }

    /// <inheritdoc/>
    public virtual List<EventTypeWithEventHandlerFactories> GetDynamicEventHandlerFactories(string eventName)
    {
        return GetDynamicHandlerFactories(eventName).ToList();
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
    {
        var handlerFactoryList = new List<Tuple<IEventHandlerFactory, Type, int>>();
        var eventNames = EventTypes.Where(x => ShouldTriggerEventForHandler(eventType, x.Value)).Select(x => x.Key).ToList();

        foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
        {
            foreach (var factory in handlerFactory.Value)
            {
                handlerFactoryList.Add(new Tuple<IEventHandlerFactory, Type, int>(
                    factory,
                    handlerFactory.Key,
                    ReflectionHelper.GetAttributesOfMemberOrDeclaringType<LocalEventHandlerOrderAttribute>(factory.GetHandler().EventHandler.GetType()).FirstOrDefault()?.Order ?? 0));
            }
        }

        foreach (var handlerFactory in DynamicEventHandlerFactories.Where(aehf => eventNames.Contains(aehf.Key)))
        {
            foreach (var factory in handlerFactory.Value)
            {
                handlerFactoryList.Add(new Tuple<IEventHandlerFactory, Type, int>(
                    factory,
                    typeof(DynamicEventData),
                    ReflectionHelper.GetAttributesOfMemberOrDeclaringType<LocalEventHandlerOrderAttribute>(factory.GetHandler().EventHandler.GetType()).FirstOrDefault()?.Order ?? 0));
            }
        }

        return handlerFactoryList.OrderBy(x => x.Item3).Select(x => new EventTypeWithEventHandlerFactories(x.Item2, new List<IEventHandlerFactory> {x.Item1})).ToArray();
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetDynamicHandlerFactories(string eventName)
    {
        var eventType = EventTypes.GetOrDefault(eventName);
        if (eventType != null)
        {
            return GetHandlerFactories(eventType);
        }

        var handlerFactoryList = new List<Tuple<IEventHandlerFactory, Type, int>>();

        foreach (var handlerFactory in DynamicEventHandlerFactories.Where(aehf => aehf.Key == eventName))
        {
            foreach (var factory in handlerFactory.Value)
            {
                using var handler = factory.GetHandler();
                var handlerType = handler.EventHandler.GetType();
                handlerFactoryList.Add(new Tuple<IEventHandlerFactory, Type, int>(
                    factory,
                    typeof(DynamicEventData),
                    ReflectionHelper
                        .GetAttributesOfMemberOrDeclaringType<LocalEventHandlerOrderAttribute>(handlerType)
                        .FirstOrDefault()?.Order ?? 0));
            }
        }

        return handlerFactoryList.OrderBy(x => x.Item3).Select(x =>
            new EventTypeWithEventHandlerFactories(x.Item2, new List<IEventHandlerFactory> { x.Item1 })).ToArray();
    }

    protected override Type? GetEventTypeByEventName(string eventName)
    {
        return EventTypes.GetOrDefault(eventName);
    }

    private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
    {
        return HandlerFactories.GetOrAdd(eventType, (type) => new List<IEventHandlerFactory>());
    }

    private List<IEventHandlerFactory> GetOrCreateDynamicHandlerFactories(string eventName)
    {
        return DynamicEventHandlerFactories.GetOrAdd(eventName, (name) => new List<IEventHandlerFactory>());
    }

    private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
    {
        if (handlerEventType == targetEventType)
        {
            return true;
        }

        if (handlerEventType.IsAssignableFrom(targetEventType))
        {
            return true;
        }

        return false;
    }
}
