using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Dapr;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
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

    protected ConcurrentDictionary<(string eventName, Type eventType), List<IEventHandlerFactory>> HandlerFactories { get; }
    protected ConcurrentDictionary<string, Type> EventTypes { get; }

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

        HandlerFactories = new ConcurrentDictionary<(string, Type), List<IEventHandlerFactory>>();
        EventTypes = new ConcurrentDictionary<string, Type>();
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

    public override IDisposable Subscribe(string eventName, Type payloadType, IEventHandlerFactory factory)
    {
        var handlerFactories = GetOrCreateHandlerFactories(eventName, payloadType);

        if (factory.IsInFactories(handlerFactories))
        {
            return NullDisposable.Instance;
        }

        handlerFactories.Add(factory);

        return new NamedEventHandlerFactoryUnregistrar(this, eventName, payloadType, factory);
    }

    public override void Unsubscribe(string eventName, Type payloadType, IEventHandlerFactory factory)
    {
        GetOrCreateHandlerFactories(eventName, payloadType)
            .Locking(factories => factories.Remove(factory));
    }

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

    protected async override Task PublishToEventBusAsync(Type eventType, object eventData)
    {
        await PublishToDaprAsync(eventType, eventData, null, CorrelationIdProvider.Get());
    }

    protected override async Task PublishToEventBusByNameAsync(string eventName, object eventData)
    {
        await PublishToDaprAsync(eventName, eventData, null, CorrelationIdProvider.Get());
    }

    protected override void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
    {
        unitOfWork.AddOrReplaceDistributedEvent(eventRecord);
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
    {
        var eventName = EventNameAttribute.GetNameOrDefault(eventType);
        return GetHandlerFactories(eventName, eventType);
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(string eventName, Type eventType)
    {
        return HandlerFactories
            .Where(hf => hf.Key.eventName == eventName && ShouldTriggerEventForHandler(eventType, hf.Key.eventType))
            .Select(hf => new EventTypeWithEventHandlerFactories(hf.Key.eventType, hf.Value))
            .ToArray();
    }

    public async override Task PublishFromOutboxAsync(OutgoingEventInfo outgoingEvent, OutboxConfig outboxConfig)
    {
        var eventType = GetEventType(outgoingEvent.EventName);
        if (eventType == null)
        {
            return;
        }

        await PublishToDaprAsync(outgoingEvent.EventName, Serializer.Deserialize(outgoingEvent.EventData, eventType), outgoingEvent.Id, outgoingEvent.GetCorrelationId());

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
        var eventName = EventNameAttribute.GetNameOrDefault(eventType);

        if (await AddToInboxAsync(messageId, eventName, eventType, eventData, correlationId))
        {
            return;
        }

        using (CorrelationIdProvider.Change(correlationId))
        {
            await TriggerHandlersDirectAsync(eventName, eventType, eventData);
        }
    }

    public async override Task ProcessFromInboxAsync(IncomingEventInfo incomingEvent, InboxConfig inboxConfig)
    {
        var eventType = GetEventType(incomingEvent.EventName);
        if (eventType == null)
        {
            return;
        }

        var eventData = Serializer.Deserialize(incomingEvent.EventData, eventType);
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

    protected override Task OnAddToOutboxAsync(string eventName, Type eventType, object eventData)
    {
        EventTypes.GetOrAdd(eventName, eventType);
        return base.OnAddToOutboxAsync(eventName, eventType, eventData);
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

    public Type? GetEventType(string eventName)
    {
        return EventTypes.GetOrDefault(eventName);
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
