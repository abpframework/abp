using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Dapr;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Tracing;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Dapr;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IDistributedEventBus), typeof(DaprDistributedEventBus))]
public class DaprDistributedEventBus : DistributedEventBusBase, ISingletonDependency
{
    protected IDaprSerializer Serializer { get; }
    protected AbpDaprEventBusOptions DaprEventBusOptions { get; }
    protected IAbpDaprClientFactory DaprClientFactory { get; }

    protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
    protected ConcurrentDictionary<string, Type> EventTypes { get; }
    protected ConcurrentDictionary<string, List<IEventHandlerFactory>> AnonymousHandlerFactories { get; }

    public DaprDistributedEventBus(
        IServiceScopeFactory serviceScopeFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IOptions<AbpDistributedEventBusOptions> abpDistributedEventBusOptions,
        IGuidGenerator guidGenerator,
        IClock clock,
        IEventHandlerInvoker eventHandlerInvoker,
        IDaprSerializer serializer,
        IOptions<AbpDaprEventBusOptions> daprEventBusOptions,
        IAbpDaprClientFactory daprClientFactory,
        ILocalEventBus localEventBus,
        ICorrelationIdProvider correlationIdProvider)
        : base(serviceScopeFactory,
            currentTenant,
            unitOfWorkManager,
            abpDistributedEventBusOptions,
            guidGenerator,
            clock,
            eventHandlerInvoker,
            localEventBus,
            correlationIdProvider)
    {
        Serializer = serializer;
        DaprEventBusOptions = daprEventBusOptions.Value;
        DaprClientFactory = daprClientFactory;

        HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        EventTypes = new ConcurrentDictionary<string, Type>();
        AnonymousHandlerFactories = new ConcurrentDictionary<string, List<IEventHandlerFactory>>();
    }

    public void Initialize()
    {
        SubscribeHandlers(AbpDistributedEventBusOptions.Handlers);
    }

    public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
        var handlerFactories = GetOrCreateHandlerFactories(eventType);

        if (factory.IsInFactories(handlerFactories))
        {
            return NullDisposable.Instance;
        }

        handlerFactories.Add(factory);

        return new EventHandlerFactoryUnregistrar(this, eventType, factory);
    }

    /// <inheritdoc/>
    public override IDisposable Subscribe(string eventName, IEventHandlerFactory handler)
    {
        var handlerFactories = GetOrCreateAnonymousHandlerFactories(eventName);

        if (handler.IsInFactories(handlerFactories))
        {
            return NullDisposable.Instance;
        }

        handlerFactories.Add(handler);

        return new AnonymousEventHandlerFactoryUnregistrar(this, eventName, handler);
    }

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

    public override void Unsubscribe(Type eventType, IEventHandler handler)
    {
        GetOrCreateHandlerFactories(eventType)
            .Locking(factories =>
            {
                factories.RemoveAll(
                    factory =>
                        factory is SingleInstanceHandlerFactory &&
                        (factory as SingleInstanceHandlerFactory)!.HandlerInstance == handler
                );
            });
    }

    public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
    }

    public override void UnsubscribeAll(Type eventType)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
    }

    /// <inheritdoc/>
    public override Task PublishAsync(string eventName, object eventData, bool onUnitOfWorkComplete = true)
    {
        var eventType = EventTypes.GetOrDefault(eventName);
        var anonymousEventData = eventData as AnonymousEventData ?? new AnonymousEventData(eventName, eventData);

        if (eventType != null)
        {
            return PublishAsync(eventType, anonymousEventData.ConvertToTypedObject(eventType), onUnitOfWorkComplete);
        }

        if (AnonymousHandlerFactories.ContainsKey(eventName))
        {
            return PublishAsync(typeof(AnonymousEventData), anonymousEventData, onUnitOfWorkComplete);
        }

        throw new AbpException($"Unknown event name: {eventName}");
    }

    protected async override Task PublishToEventBusAsync(Type eventType, object eventData)
    {
        var (eventName, resolvedData) = ResolveEventForPublishing(eventType, eventData);
        await PublishToDaprAsync(eventName, resolvedData, null, CorrelationIdProvider.Get());
    }

    protected override void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
    {
        unitOfWork.AddOrReplaceDistributedEvent(eventRecord);
    }

    public async override Task PublishFromOutboxAsync(OutgoingEventInfo outgoingEvent, OutboxConfig outboxConfig)
    {
        var eventType = EventTypes.GetOrDefault(outgoingEvent.EventName);
        object eventData;

        if (eventType != null)
        {
            eventData = Serializer.Deserialize(outgoingEvent.EventData, eventType);
        }
        else if (AnonymousHandlerFactories.ContainsKey(outgoingEvent.EventName))
        {
            eventData = Serializer.Deserialize(outgoingEvent.EventData, typeof(object));
        }
        else
        {
            return;
        }

        await PublishToDaprAsync(outgoingEvent.EventName, eventData, outgoingEvent.Id, outgoingEvent.GetCorrelationId());

        using (CorrelationIdProvider.Change(outgoingEvent.GetCorrelationId()))
        {
            await TriggerDistributedEventSentAsync(new DistributedEventSent()
            {
                Source = DistributedEventSource.Outbox,
                EventName = outgoingEvent.EventName,
                EventData = outgoingEvent.EventData
            });
        }
    }

    public async override Task PublishManyFromOutboxAsync(IEnumerable<OutgoingEventInfo> outgoingEvents, OutboxConfig outboxConfig)
    {
        foreach (var outgoingEvent in outgoingEvents)
        {
           await PublishFromOutboxAsync(outgoingEvent, outboxConfig);
        }
    }

    public virtual async Task TriggerHandlersAsync(Type eventType, object eventData, string? messageId = null, string? correlationId = null)
    {
        if (await AddToInboxAsync(messageId, GetEventName(eventType, eventData), eventType, eventData, correlationId))
        {
            return;
        }

        using (CorrelationIdProvider.Change(correlationId))
        {
            await TriggerHandlersDirectAsync(eventType, eventData);
        }
    }

    public async override Task ProcessFromInboxAsync(IncomingEventInfo incomingEvent, InboxConfig inboxConfig)
    {
        var eventType = EventTypes.GetOrDefault(incomingEvent.EventName);
        object eventData;

        if (eventType != null)
        {
            eventData = Serializer.Deserialize(incomingEvent.EventData, eventType);
        }
        else if (AnonymousHandlerFactories.ContainsKey(incomingEvent.EventName))
        {
            eventData = new AnonymousEventData(incomingEvent.EventName, Serializer.Deserialize(incomingEvent.EventData, typeof(object)));
            eventType = typeof(AnonymousEventData);
        }
        else
        {
            return;
        }

        var exceptions = new List<Exception>();
        using (CorrelationIdProvider.Change(incomingEvent.GetCorrelationId()))
        {
            await TriggerHandlersFromInboxAsync(eventType, eventData, exceptions, inboxConfig);
        }
        if (exceptions.Any())
        {
            ThrowOriginalExceptions(eventType, exceptions);
        }
    }

    protected override byte[] Serialize(object eventData)
    {
        return Serializer.Serialize(eventData);
    }

    protected virtual async Task PublishToDaprAsync(Type eventType, object eventData, Guid? messageId = null, string? correlationId = null)
    {
        await PublishToDaprAsync(EventNameAttribute.GetNameOrDefault(eventType), eventData, messageId, correlationId);
    }

    protected virtual async Task PublishToDaprAsync(string eventName, object eventData, Guid? messageId = null, string? correlationId = null)
    {
        var client = await DaprClientFactory.CreateAsync();
        var data = new AbpDaprEventData(DaprEventBusOptions.PubSubName, eventName, (messageId ?? GuidGenerator.Create()).ToString("N"), Serializer.SerializeToString(eventData), correlationId);
        await client.PublishEventAsync(pubsubName: DaprEventBusOptions.PubSubName, topicName: eventName, data: data);
    }

    protected override Task OnAddToOutboxAsync(string eventName, Type eventType, object eventData)
    {
        if (typeof(AnonymousEventData) != eventType)
        {
            EventTypes.GetOrAdd(eventName, eventType);
        }
        return base.OnAddToOutboxAsync(eventName, eventType, eventData);
    }

    private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
    {
        return HandlerFactories.GetOrAdd(
            eventType,
            type =>
            {
                var eventName = EventNameAttribute.GetNameOrDefault(type);
                EventTypes.GetOrAdd(eventName, eventType);
                return new List<IEventHandlerFactory>();
            }
        );
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
    {
        var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();
        var eventNames = EventTypes.Where(x => ShouldTriggerEventForHandler(eventType, x.Value)).Select(x => x.Key).ToList();

        foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
        {
            handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
        }

        foreach (var handlerFactory in AnonymousHandlerFactories.Where(aehf => eventNames.Contains(aehf.Key)))
        {
            handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(typeof(AnonymousEventData), handlerFactory.Value));
        }

        return handlerFactoryList.ToArray();
    }

    protected override Type? GetEventTypeByEventName(string eventName)
    {
        return EventTypes.GetOrDefault(eventName);
    }

    public Type? GetEventType(string eventName)
    {
        return EventTypes.GetOrDefault(eventName);
    }
    
    public bool IsAnonymousEvent(string eventName)
    {
        return AnonymousHandlerFactories.ContainsKey(eventName);
    }

    /// <inheritdoc/>
    public override void Unsubscribe(string eventName, IEventHandlerFactory factory)
    {
        GetOrCreateAnonymousHandlerFactories(eventName).Locking(factories => factories.Remove(factory));
    }

    /// <inheritdoc/>
    public override void Unsubscribe(string eventName, IEventHandler handler)
    {
        GetOrCreateAnonymousHandlerFactories(eventName)
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
    public override void UnsubscribeAll(string eventName)
    {
        GetOrCreateAnonymousHandlerFactories(eventName).Locking(factories => factories.Clear());
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetAnonymousHandlerFactories(string eventName)
    {
        var result = new List<EventTypeWithEventHandlerFactories>();

        var eventType = GetEventTypeByEventName(eventName);
        if (eventType != null)
        {
            result.AddRange(GetHandlerFactories(eventType));
        }

        foreach (var handlerFactory in AnonymousHandlerFactories.Where(hf => hf.Key == eventName))
        {
            result.Add(new EventTypeWithEventHandlerFactories(typeof(AnonymousEventData), handlerFactory.Value));
        }

        return result;
    }

    private List<IEventHandlerFactory> GetOrCreateAnonymousHandlerFactories(string eventName)
    {
        return AnonymousHandlerFactories.GetOrAdd(eventName, _ => new List<IEventHandlerFactory>());
    }

    private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
    {
        //Should trigger same type
        if (handlerEventType == targetEventType)
        {
            return true;
        }

        //TODO: Support inheritance? But it does not support on subscription to RabbitMq!
        //Should trigger for inherited types
        if (handlerEventType.IsAssignableFrom(targetEventType))
        {
            return true;
        }

        return false;
    }
}
