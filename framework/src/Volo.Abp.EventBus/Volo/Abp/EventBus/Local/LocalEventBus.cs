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

    protected ConcurrentDictionary<(string eventName, Type eventType), List<IEventHandlerFactory>> HandlerFactories { get; }

    protected ConcurrentDictionary<string, Type> EventTypes { get; }

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

        HandlerFactories = new ConcurrentDictionary<(string, Type), List<IEventHandlerFactory>>();
        EventTypes = new ConcurrentDictionary<string, Type>();
        SubscribeHandlers(Options.Handlers);
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe<TEvent>(ILocalEventHandler<TEvent> handler) where TEvent : class
    {
        return Subscribe(typeof(TEvent), handler);
    }

    /// <inheritdoc/>
    public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
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
    public virtual IDisposable Subscribe<TPayload>(string eventName, ILocalEventHandler<TPayload> handler)
        where TPayload : class
    {
        return Subscribe(eventName, typeof(TPayload), new SingleInstanceHandlerFactory(handler));
    }

    /// <inheritdoc/>
    public override IDisposable Subscribe(string eventName, Type payloadType, IEventHandlerFactory factory)
    {
        GetOrCreateHandlerFactories(eventName, payloadType)
            .Locking(factories =>
            {
                if (!factory.IsInFactories(factories))
                {
                    factories.Add(factory);
                }
            });

        return new NamedEventHandlerFactoryUnregistrar(this, eventName, payloadType, factory);
    }

    /// <inheritdoc/>
    public override void Unsubscribe(string eventName, Type payloadType, IEventHandlerFactory factory)
    {
        GetOrCreateHandlerFactories(eventName, payloadType)
            .Locking(factories => factories.Remove(factory));
    }

    /// <inheritdoc/>
    public override void UnsubscribeAll(string eventName)
    {
        var keysToRemove = HandlerFactories.Keys.Where(k => k.eventName == eventName).ToList();
        foreach (var key in keysToRemove)
        {
            if (HandlerFactories.TryGetValue(key, out var factories))
            {
                factories.Locking(list => list.Clear());
            }
        }
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
    public override void UnsubscribeAll(Type eventType)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
    }

    protected override async Task PublishToEventBusAsync(Type eventType, object eventData)
    {
        await PublishAsync(new LocalEventMessage(Guid.NewGuid(), eventData, eventType));
    }

    protected override async Task PublishToEventBusByNameAsync(string eventName, object eventData)
    {
        var eventType = EventTypes.GetOrDefault(eventName);
        if (eventType == null)
        {
            return;
        }

        var convertedData = ConvertPayloadToType(eventData, eventType);
        await TriggerHandlersAsync(eventName, eventType, convertedData);
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

    public virtual List<EventTypeWithEventHandlerFactories> GetEventHandlerFactories(string eventName, Type eventType)
    {
        return GetHandlerFactories(eventName, eventType).ToList();
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
    {
        var handlerFactoryList = new List<Tuple<IEventHandlerFactory, Type, int>>();
        foreach (var handlerFactory in HandlerFactories.Where(hf =>
                     hf.Key.eventName == EventNameAttribute.GetNameOrDefault(hf.Key.eventType) &&
                     ShouldTriggerEventForHandler(eventType, hf.Key.eventType)))
        {
            foreach (var factory in handlerFactory.Value)
            {
                handlerFactoryList.Add(new Tuple<IEventHandlerFactory, Type, int>(
                    factory,
                    handlerFactory.Key.eventType,
                    GetHandlerOrder(factory)));
            }
        }

        return handlerFactoryList.OrderBy(x => x.Item3).Select(x => new EventTypeWithEventHandlerFactories(x.Item2, new List<IEventHandlerFactory> {x.Item1})).ToArray();
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(string eventName, Type eventType)
    {
        var handlerFactoryList = new List<Tuple<IEventHandlerFactory, Type, int>>();
        foreach (var handlerFactory in HandlerFactories.Where(hf =>
                     hf.Key.eventName == eventName && ShouldTriggerEventForHandler(eventType, hf.Key.eventType)))
        {
            foreach (var factory in handlerFactory.Value)
            {
                handlerFactoryList.Add(new Tuple<IEventHandlerFactory, Type, int>(
                    factory,
                    handlerFactory.Key.eventType,
                    GetHandlerOrder(factory)));
            }
        }

        return handlerFactoryList.OrderBy(x => x.Item3).Select(x => new EventTypeWithEventHandlerFactories(x.Item2, new List<IEventHandlerFactory> {x.Item1})).ToArray();
    }

    private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
    {
        var eventName = EventNameAttribute.GetNameOrDefault(eventType);
        return GetOrCreateHandlerFactories(eventName, eventType);
    }

    private List<IEventHandlerFactory> GetOrCreateHandlerFactories(string eventName, Type eventType)
    {
        var existingType = EventTypes.GetOrDefault(eventName);
        if (existingType != null && existingType != eventType)
        {
            throw new AbpException(
                $"Event name '{eventName}' is already mapped to type '{existingType.FullName}'. " +
                $"Cannot register a different type '{eventType.FullName}' for the same event name.");
        }

        return HandlerFactories.GetOrAdd(
            (eventName, eventType),
            _ =>
            {
                EventTypes.GetOrAdd(eventName, eventType);
                return new List<IEventHandlerFactory>();
            }
        );
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

    private static int GetHandlerOrder(IEventHandlerFactory factory)
    {
        var handlerType = GetHandlerType(factory);
        return ReflectionHelper
                   .GetAttributesOfMemberOrDeclaringType<LocalEventHandlerOrderAttribute>(handlerType)
                   .FirstOrDefault()
                   ?.Order ?? 0;
    }

    private static Type GetHandlerType(IEventHandlerFactory factory)
    {
        switch (factory)
        {
            case SingleInstanceHandlerFactory singleInstanceHandlerFactory:
                return singleInstanceHandlerFactory.HandlerInstance.GetType();
            case IocEventHandlerFactory iocEventHandlerFactory:
                return iocEventHandlerFactory.HandlerType;
            case TransientEventHandlerFactory transientEventHandlerFactory:
                return transientEventHandlerFactory.HandlerType;
            default:
            {
                using var eventHandlerWrapper = factory.GetHandler();
                return eventHandlerWrapper.EventHandler.GetType();
            }
        }
    }
}
