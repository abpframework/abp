using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rebus.Bus;
using Rebus.Messages;
using Rebus.Pipeline;
using Rebus.Transport;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Tracing;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Rebus;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IDistributedEventBus), typeof(RebusDistributedEventBus))]
public class RebusDistributedEventBus : DistributedEventBusBase, ISingletonDependency
{
    protected IBus Rebus { get; }
    protected IRebusSerializer Serializer { get; }

    //TODO: Accessing to the List<IEventHandlerFactory> may not be thread-safe!
    protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
    protected ConcurrentDictionary<string, Type> EventTypes { get; }
    protected ConcurrentDictionary<string, List<IEventHandlerFactory>> DynamicHandlerFactories { get; }
    protected AbpRebusEventBusOptions AbpRebusEventBusOptions { get; }

    public RebusDistributedEventBus(
        IServiceScopeFactory serviceScopeFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IBus rebus,
        IOptions<AbpDistributedEventBusOptions> abpDistributedEventBusOptions,
        IOptions<AbpRebusEventBusOptions> abpEventBusRebusOptions,
        IRebusSerializer serializer,
        IGuidGenerator guidGenerator,
        IClock clock,
        IEventHandlerInvoker eventHandlerInvoker,
        ILocalEventBus localEventBus,
        ICorrelationIdProvider correlationIdProvider) :
        base(
            serviceScopeFactory,
            currentTenant,
            unitOfWorkManager,
            abpDistributedEventBusOptions,
            guidGenerator,
            clock,
            eventHandlerInvoker,
            localEventBus,
            correlationIdProvider)
    {
        Rebus = rebus;
        Serializer = serializer;
        AbpRebusEventBusOptions = abpEventBusRebusOptions.Value;

        HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        EventTypes = new ConcurrentDictionary<string, Type>();
        DynamicHandlerFactories = new ConcurrentDictionary<string, List<IEventHandlerFactory>>();
    }

    public void Initialize()
    {
        SubscribeHandlers(AbpDistributedEventBusOptions.Handlers);
    }

    public async Task ProcessEventAsync(Type eventType, object eventData)
    {
        var messageId = MessageContext.Current.TransportMessage.GetMessageId();
        string eventName;
        if (eventType == typeof(DynamicEventData) && eventData is DynamicEventData dynamicEventData)
        {
            eventName = dynamicEventData.EventName;
        }
        else
        {
            eventName = EventNameAttribute.GetNameOrDefault(eventType);
        }
        var correlationId = MessageContext.Current.Headers.GetOrDefault(EventBusConsts.CorrelationIdHeaderName);

        if (await AddToInboxAsync(messageId, eventName, eventType, eventData, correlationId))
        {
            return;
        }

        using (CorrelationIdProvider.Change(correlationId))
        {
            await TriggerHandlersDirectAsync(eventType, eventData);
        }
    }

    public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
        var handlerFactories = GetOrCreateHandlerFactories(eventType);

        if (factory.IsInFactories(handlerFactories))
        {
            return NullDisposable.Instance;
        }

        handlerFactories.Add(factory);

        if (handlerFactories.Count == 1) //TODO: Multi-threading!
        {
            Rebus.Subscribe(eventType);
        }

        return new EventHandlerFactoryUnregistrar(this, eventType, factory);
    }

    /// <inheritdoc/>
    public override IDisposable Subscribe(string eventName, IEventHandlerFactory handler)
    {
        var handlerFactories = GetOrCreateDynamicHandlerFactories(eventName);

        if (handler.IsInFactories(handlerFactories))
        {
            return NullDisposable.Instance;
        }

        handlerFactories.Add(handler);
        
        if (DynamicHandlerFactories.Count == 1) //TODO: Multi-threading!
        {
            Rebus.Subscribe(typeof(DynamicEventData));
        }

        return new DynamicEventHandlerFactoryUnregistrar(this, eventName, handler);
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
                        if (!(factory is SingleInstanceHandlerFactory singleInstanceFactory))
                        {
                            return false;
                        }

                        if (!(singleInstanceFactory.HandlerInstance is ActionEventHandler<TEvent> actionHandler))
                        {
                            return false;
                        }

                        return actionHandler.Action == action;
                    });
            });

        Rebus.Unsubscribe(typeof(TEvent));
    }

    public override void Unsubscribe(Type eventType, IEventHandler handler)
    {
        GetOrCreateHandlerFactories(eventType)
            .Locking(factories =>
            {
                factories.RemoveAll(
                    factory =>
                        factory is SingleInstanceHandlerFactory handlerFactory &&
                        handlerFactory.HandlerInstance == handler
                );
            });

        Rebus.Unsubscribe(eventType);
    }

    public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
        Rebus.Unsubscribe(eventType);
    }

    public override void UnsubscribeAll(Type eventType)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
        Rebus.Unsubscribe(eventType);
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

    protected async override Task PublishToEventBusAsync(Type eventType, object eventData)
    {
        var headers = new Dictionary<string, string>();
        if (CorrelationIdProvider.Get() != null)
        {
            headers.Add(EventBusConsts.CorrelationIdHeaderName, CorrelationIdProvider.Get()!);
        }
        await PublishAsync(eventType, eventData, headersArguments: headers);
    }

    protected override void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
    {
        unitOfWork.AddOrReplaceDistributedEvent(eventRecord);
    }

    public async override Task PublishFromOutboxAsync(
        OutgoingEventInfo outgoingEvent,
        OutboxConfig outboxConfig)
    {
        var eventType = EventTypes.GetOrDefault(outgoingEvent.EventName);
        object eventData;

        if (eventType != null)
        {
            eventData = Serializer.Deserialize(outgoingEvent.EventData, eventType);
        }
        else if (DynamicHandlerFactories.ContainsKey(outgoingEvent.EventName))
        {
            eventData = new DynamicEventData(outgoingEvent.EventName, Serializer.Deserialize(outgoingEvent.EventData, typeof(object)));
            eventType = typeof(DynamicEventData);
        }
        else
        {
            return;
        }

        var headers = new Dictionary<string, string>();
        if (outgoingEvent.GetCorrelationId() != null)
        {
            headers.Add(EventBusConsts.CorrelationIdHeaderName, outgoingEvent.GetCorrelationId()!);
        }

        await PublishAsync(eventType, eventData, eventId: outgoingEvent.Id, headersArguments: headers);

        using (CorrelationIdProvider.Change(outgoingEvent.GetCorrelationId()))
        {
            await TriggerDistributedEventSentAsync(new DistributedEventSent() {
                Source = DistributedEventSource.Outbox,
                EventName = outgoingEvent.EventName,
                EventData = outgoingEvent.EventData
            });
        }
    }

    public async override Task PublishManyFromOutboxAsync(IEnumerable<OutgoingEventInfo> outgoingEvents, OutboxConfig outboxConfig)
    {
        var outgoingEventArray = outgoingEvents.ToArray();

        using (var scope = new RebusTransactionScope())
        {
            foreach (var outgoingEvent in outgoingEventArray)
            {
                await PublishFromOutboxAsync(outgoingEvent, outboxConfig);

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

            await scope.CompleteAsync();
        }
    }

    public async override Task ProcessFromInboxAsync(
        IncomingEventInfo incomingEvent,
        InboxConfig inboxConfig)
    {
        var eventType = EventTypes.GetOrDefault(incomingEvent.EventName);
        object eventData;

        if (eventType != null)
        {
            eventData = Serializer.Deserialize(incomingEvent.EventData, eventType);
        }
        else if (DynamicHandlerFactories.ContainsKey(incomingEvent.EventName))
        {
            eventData = new DynamicEventData(incomingEvent.EventName, Serializer.Deserialize(incomingEvent.EventData, typeof(object)));
            eventType = typeof(DynamicEventData);
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

    protected virtual async Task PublishAsync(
        Type eventType,
        object eventData,
        Guid? eventId = null,
        Dictionary<string, string>? headersArguments = null)
    {
        if (AbpRebusEventBusOptions.Publish != null)
        {
            await AbpRebusEventBusOptions.Publish(Rebus, eventType, eventData);
            return;
        }

        headersArguments ??= new Dictionary<string, string>();
        if (!headersArguments.ContainsKey(Headers.MessageId))
        {
            headersArguments[Headers.MessageId] = (eventId ?? GuidGenerator.Create()).ToString("N");
        }

        await Rebus.Publish(eventData, headersArguments);
    }

    protected override Task OnAddToOutboxAsync(string eventName, Type eventType, object eventData)
    {
        if (typeof(DynamicEventData) != eventType)
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

        foreach (var handlerFactory in DynamicHandlerFactories.Where(aehf => eventNames.Contains(aehf.Key)))
        {
            handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(typeof(DynamicEventData), handlerFactory.Value));
        }

        return handlerFactoryList.ToArray();
    }

    protected override Type? GetEventTypeByEventName(string eventName)
    {
        return EventTypes.GetOrDefault(eventName);
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
    public override void UnsubscribeAll(string eventName)
    {
        GetOrCreateDynamicHandlerFactories(eventName).Locking(factories => factories.Clear());
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetDynamicHandlerFactories(string eventName)
    {
        var eventType = GetEventTypeByEventName(eventName);
        if (eventType != null)
        {
            return GetHandlerFactories(eventType);
        }

        var result = new List<EventTypeWithEventHandlerFactories>();

        foreach (var handlerFactory in DynamicHandlerFactories.Where(hf => hf.Key == eventName))
        {
            result.Add(new EventTypeWithEventHandlerFactories(typeof(DynamicEventData), handlerFactory.Value));
        }

        return result;
    }

    private List<IEventHandlerFactory> GetOrCreateDynamicHandlerFactories(string eventName)
    {
        return DynamicHandlerFactories.GetOrAdd(eventName, _ => new List<IEventHandlerFactory>());
    }

    private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
    {
        //Should trigger same type
        if (handlerEventType == targetEventType)
        {
            return true;
        }

        //Should trigger for inherited types
        if (handlerEventType.IsAssignableFrom(targetEventType))
        {
            return true;
        }

        return false;
    }
}
