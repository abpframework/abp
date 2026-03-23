using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.RabbitMQ;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Tracing;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.RabbitMq;

/* TODO: How to handle unsubscribe to unbind on RabbitMq (may not be possible for)
 */
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IDistributedEventBus), typeof(RabbitMqDistributedEventBus), typeof(IRabbitMqDistributedEventBus))]
public class RabbitMqDistributedEventBus : DistributedEventBusBase, IRabbitMqDistributedEventBus, ISingletonDependency
{
    protected AbpRabbitMqEventBusOptions AbpRabbitMqEventBusOptions { get; }
    protected IConnectionPool ConnectionPool { get; }
    protected IRabbitMqSerializer Serializer { get; }

    //TODO: Accessing to the List<IEventHandlerFactory> may not be thread-safe!
    protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
    protected ConcurrentDictionary<string, Type> EventTypes { get; }
    protected ConcurrentDictionary<string, List<IEventHandlerFactory>> DynamicHandlerFactories { get; }
    protected IRabbitMqMessageConsumerFactory MessageConsumerFactory { get; }
    protected IRabbitMqMessageConsumer Consumer { get; private set; } = default!;

    private bool _exchangeCreated;

    public RabbitMqDistributedEventBus(
        IOptions<AbpRabbitMqEventBusOptions> options,
        IConnectionPool connectionPool,
        IRabbitMqSerializer serializer,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<AbpDistributedEventBusOptions> distributedEventBusOptions,
        IRabbitMqMessageConsumerFactory messageConsumerFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IGuidGenerator guidGenerator,
        IClock clock,
        IEventHandlerInvoker eventHandlerInvoker,
        ILocalEventBus localEventBus,
        ICorrelationIdProvider correlationIdProvider)
        : base(
            serviceScopeFactory,
            currentTenant,
            unitOfWorkManager,
            distributedEventBusOptions,
            guidGenerator,
            clock,
            eventHandlerInvoker,
            localEventBus,
            correlationIdProvider)
    {
        ConnectionPool = connectionPool;
        Serializer = serializer;
        MessageConsumerFactory = messageConsumerFactory;
        AbpRabbitMqEventBusOptions = options.Value;

        HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        EventTypes = new ConcurrentDictionary<string, Type>();
        DynamicHandlerFactories = new ConcurrentDictionary<string, List<IEventHandlerFactory>>();
    }

    public virtual void Initialize()
    {
        Consumer = MessageConsumerFactory.Create(
            new ExchangeDeclareConfiguration(
                AbpRabbitMqEventBusOptions.ExchangeName,
                type: AbpRabbitMqEventBusOptions.GetExchangeTypeOrDefault(),
                durable: true,
                arguments: AbpRabbitMqEventBusOptions.ExchangeArguments
            ),
            new QueueDeclareConfiguration(
                AbpRabbitMqEventBusOptions.ClientName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                prefetchCount: AbpRabbitMqEventBusOptions.PrefetchCount,
                arguments: AbpRabbitMqEventBusOptions.QueueArguments
            ),
            AbpRabbitMqEventBusOptions.ConnectionName
        );

        Consumer.OnMessageReceived(ProcessEventAsync);

        SubscribeHandlers(AbpDistributedEventBusOptions.Handlers);
    }

    private async Task ProcessEventAsync(IChannel channel, BasicDeliverEventArgs ea)
    {
        var eventName = ea.RoutingKey;
        var eventType = EventTypes.GetOrDefault(eventName);
        object eventData;

        if (eventType != null)
        {
            eventData = Serializer.Deserialize(ea.Body.ToArray(), eventType);
        }
        else if (DynamicHandlerFactories.ContainsKey(eventName))
        {
            var rawBytes = ea.Body.ToArray();
            eventType = typeof(DynamicEventData);
            eventData = new DynamicEventData(eventName, Serializer.Deserialize<object>(rawBytes));
        }
        else
        {
            return;
        }

        var correlationId = ea.BasicProperties.CorrelationId;
        if (await AddToInboxAsync(ea.BasicProperties.MessageId, eventName, eventType, eventData, correlationId))
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
            Consumer.BindAsync(EventNameAttribute.GetNameOrDefault(eventType));
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
        
        if (handlerFactories.Count == 1) //TODO: Multi-threading!
        {
            Consumer.BindAsync(eventName);
        }

        return new DynamicEventHandlerFactoryUnregistrar(this, eventName, handler);
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
                        (factory as SingleInstanceHandlerFactory)!.HandlerInstance == handler
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
        await PublishAsync(eventType, eventData, correlationId: CorrelationIdProvider.Get());
    }

    protected override void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
    {
        unitOfWork.AddOrReplaceDistributedEvent(eventRecord);
    }

    public async override Task PublishFromOutboxAsync(
        OutgoingEventInfo outgoingEvent,
        OutboxConfig outboxConfig)
    {
        await PublishAsync(outgoingEvent.EventName, outgoingEvent.EventData, eventId: outgoingEvent.Id, correlationId: outgoingEvent.GetCorrelationId());

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

    public async override Task PublishManyFromOutboxAsync(
        IEnumerable<OutgoingEventInfo> outgoingEvents,
        OutboxConfig outboxConfig)
    {
        using (var channel = await (await ConnectionPool.GetAsync(AbpRabbitMqEventBusOptions.ConnectionName))
                   .CreateChannelAsync(new CreateChannelOptions(publisherConfirmationsEnabled: true, publisherConfirmationTrackingEnabled: true, new ThrottlingRateLimiter(256))))
        {
            var outgoingEventArray = outgoingEvents.ToArray();

            foreach (var outgoingEvent in outgoingEventArray)
            {
                await PublishAsync(
                    channel,
                    outgoingEvent.EventName,
                    outgoingEvent.EventData,
                    eventId: outgoingEvent.Id,
                    correlationId: outgoingEvent.GetCorrelationId());

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
            eventData = new DynamicEventData(incomingEvent.EventName, Serializer.Deserialize<object>(incomingEvent.EventData));
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

    public virtual Task PublishAsync(
        Type eventType,
        object eventData,
        Dictionary<string, object>? headersArguments = null,
        Guid? eventId = null,
        string? correlationId = null)
    {
        var (eventName, resolvedData) = ResolveEventForPublishing(eventType, eventData);
        var body = Serializer.Serialize(resolvedData);

        return PublishAsync(eventName, body, headersArguments, eventId, correlationId);
    }

    protected virtual async Task PublishAsync(
        string eventName,
        byte[] body,
        Dictionary<string, object>? headersArguments = null,
        Guid? eventId = null,
        string? correlationId = null)
    {
        using (var channel = await (await ConnectionPool.GetAsync(AbpRabbitMqEventBusOptions.ConnectionName)).CreateChannelAsync())
        {
            await PublishAsync(channel, eventName, body, headersArguments, eventId, correlationId);
        }
    }

    protected virtual async Task PublishAsync(
        IChannel channel,
        string eventName,
        byte[] body,
        Dictionary<string, object>? headersArguments = null,
        Guid? eventId = null,
        string? correlationId = null)
    {
        await EnsureExchangeExistsAsync(channel);

        var properties = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent
        };

        if (properties.MessageId.IsNullOrEmpty())
        {
            properties.MessageId = (eventId ?? GuidGenerator.Create()).ToString("N");
        }

        if (correlationId != null)
        {
            properties.CorrelationId = correlationId;
        }

        SetEventMessageHeaders(properties, headersArguments);

        await channel.BasicPublishAsync(
            exchange: AbpRabbitMqEventBusOptions.ExchangeName,
            routingKey: eventName,
            mandatory: false,
            basicProperties: properties,
            body: body
        );
    }

    protected virtual async Task EnsureExchangeExistsAsync(IChannel channel)
    {
        if (_exchangeCreated)
        {
            return;
        }

        try
        {
            using (var temporaryChannel = await (await ConnectionPool.GetAsync(AbpRabbitMqEventBusOptions.ConnectionName)).CreateChannelAsync())
            {
                await temporaryChannel.ExchangeDeclarePassiveAsync(AbpRabbitMqEventBusOptions.ExchangeName);
            }
        }
        catch (Exception)
        {
            await channel.ExchangeDeclareAsync(
                AbpRabbitMqEventBusOptions.ExchangeName,
                AbpRabbitMqEventBusOptions.GetExchangeTypeOrDefault(),
                durable: true
            );
        }
        _exchangeCreated = true;
    }

    protected virtual void SetEventMessageHeaders(IBasicProperties properties, Dictionary<string, object>? headersArguments)
    {
        if (headersArguments == null)
        {
            return;
        }

        properties.Headers ??= new Dictionary<string, object?>();

        foreach (var header in headersArguments)
        {
            properties.Headers[header.Key] = header.Value;
        }
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
        var result = new List<EventTypeWithEventHandlerFactories>();

        var eventType = GetEventTypeByEventName(eventName);
        if (eventType != null)
        {
            return GetHandlerFactories(eventType);
        }

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

        //TODO: Support inheritance? But it does not support on subscription to RabbitMq!
        //Should trigger for inherited types
        if (handlerEventType.IsAssignableFrom(targetEventType))
        {
            return true;
        }

        return false;
    }
}
