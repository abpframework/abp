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
        var added = false;
        handlerFactories.Locking(factories =>
        {
            if (!factory.IsInFactories(factories))
            {
                factories.Add(factory);
                added = true;
            }
        });

        if (!added)
        {
            return NullDisposable.Instance;
        }

        return new EventHandlerFactoryUnregistrar(this, eventType, factory);
    }

    /// <inheritdoc/>
    public override IDisposable Subscribe(string eventName, IEventHandlerFactory handler)
    {
        var handlerFactories = GetOrCreateAnonymousHandlerFactories(eventName);
        var added = false;
        handlerFactories.Locking(factories =>
        {
            if (!handler.IsInFactories(factories))
            {
                factories.Add(handler);
                added = true;
            }
        });

        if (!added)
        {
            return NullDisposable.Instance;
        }

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
        var anonymousEventData = CreateAnonymousEnvelope(eventName, eventData);
        return TryPublishTypedByEventNameAsync(eventName, anonymousEventData, onUnitOfWorkComplete)
            ?? PublishAnonymousByEventNameAsync(eventName, anonymousEventData, onUnitOfWorkComplete);
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
        if (!TryResolveStoredEventData(outgoingEvent.EventName, outgoingEvent.EventData, out var eventType, out var eventData))
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
        if (!TryResolveStoredEventData(incomingEvent.EventName, incomingEvent.EventData, out var eventType, out var eventData))
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
        return HasAnonymousHandlers(eventName);
    }

    /// <inheritdoc/>
    public override void Unsubscribe(string eventName, IEventHandlerFactory factory)
    {
        if (!AnonymousHandlerFactories.TryGetValue(eventName, out var handlerFactories))
        {
            return;
        }

        handlerFactories.Locking(factories => factories.Remove(factory));
        CleanupAnonymousHandlerFactoriesIfEmpty(eventName, handlerFactories);
    }

    /// <inheritdoc/>
    public override void Unsubscribe(string eventName, IEventHandler handler)
    {
        if (!AnonymousHandlerFactories.TryGetValue(eventName, out var handlerFactories))
        {
            return;
        }

        handlerFactories.Locking(factories =>
        {
            factories.RemoveAll(
                factory =>
                    factory is SingleInstanceHandlerFactory singleFactory &&
                    singleFactory.HandlerInstance == handler
            );
        });

        CleanupAnonymousHandlerFactoriesIfEmpty(eventName, handlerFactories);
    }

    /// <inheritdoc/>
    public override void UnsubscribeAll(string eventName)
    {
        if (!AnonymousHandlerFactories.TryGetValue(eventName, out var handlerFactories))
        {
            return;
        }

        handlerFactories.Locking(factories => factories.Clear());
        CleanupAnonymousHandlerFactoriesIfEmpty(eventName, handlerFactories);
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

    private AnonymousEventData CreateAnonymousEnvelope(string eventName, object eventData)
    {
        return eventData as AnonymousEventData ?? new AnonymousEventData(eventName, eventData);
    }

    private Task? TryPublishTypedByEventNameAsync(string eventName, AnonymousEventData anonymousEventData, bool onUnitOfWorkComplete)
    {
        var eventType = EventTypes.GetOrDefault(eventName);
        if (eventType == null)
        {
            return null;
        }

        var typedEventData = AnonymousEventDataConverter.ConvertToTypedObject(anonymousEventData, eventType);
        return PublishAsync(eventType, typedEventData, onUnitOfWorkComplete);
    }

    private Task PublishAnonymousByEventNameAsync(string eventName, AnonymousEventData anonymousEventData, bool onUnitOfWorkComplete)
    {
        if (!HasAnonymousHandlers(eventName))
        {
            return Task.CompletedTask;
        }

        return PublishAsync(typeof(AnonymousEventData), anonymousEventData, onUnitOfWorkComplete);
    }

    private bool TryResolveStoredEventData(string eventName, byte[] payload, out Type eventType, out object eventData)
    {
        eventType = EventTypes.GetOrDefault(eventName)!;
        if (eventType != null)
        {
            eventData = Serializer.Deserialize(payload, eventType);
            return true;
        }

        if (!HasAnonymousHandlers(eventName))
        {
            eventData = default!;
            eventType = default!;
            return false;
        }

        eventType = typeof(AnonymousEventData);
        eventData = CreateAnonymousEventData(eventName, payload);
        return true;
    }

    private bool HasAnonymousHandlers(string eventName)
    {
        if (!AnonymousHandlerFactories.TryGetValue(eventName, out var handlerFactories))
        {
            return false;
        }

        var hasHandlers = false;
        handlerFactories.Locking(factories => hasHandlers = factories.Count > 0);
        if (!hasHandlers)
        {
            AnonymousHandlerFactories.TryRemove(eventName, out _);
        }

        return hasHandlers;
    }

    private void CleanupAnonymousHandlerFactoriesIfEmpty(string eventName, List<IEventHandlerFactory> handlerFactories)
    {
        var isEmpty = false;
        handlerFactories.Locking(factories => isEmpty = factories.Count == 0);
        if (isEmpty)
        {
            AnonymousHandlerFactories.TryRemove(eventName, out _);
        }
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
