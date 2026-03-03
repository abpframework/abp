using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.Kafka;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Tracing;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Kafka;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IDistributedEventBus), typeof(KafkaDistributedEventBus))]
public class KafkaDistributedEventBus : DistributedEventBusBase, ISingletonDependency
{
    protected AbpKafkaEventBusOptions AbpKafkaEventBusOptions { get; }
    protected IKafkaMessageConsumerFactory MessageConsumerFactory { get; }
    protected IKafkaSerializer Serializer { get; }
    protected IProducerPool ProducerPool { get; }
    protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
    protected ConcurrentDictionary<string, Type> EventTypes { get; }
    protected ConcurrentDictionary<string, List<IEventHandlerFactory>> AnonymousHandlerFactories { get; }
    protected IKafkaMessageConsumer Consumer { get; private set; } = default!;

    public KafkaDistributedEventBus(
        IServiceScopeFactory serviceScopeFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IOptions<AbpKafkaEventBusOptions> abpKafkaEventBusOptions,
        IKafkaMessageConsumerFactory messageConsumerFactory,
        IOptions<AbpDistributedEventBusOptions> abpDistributedEventBusOptions,
        IKafkaSerializer serializer,
        IProducerPool producerPool,
        IGuidGenerator guidGenerator,
        IClock clock,
        IEventHandlerInvoker eventHandlerInvoker,
        ILocalEventBus localEventBus,
        ICorrelationIdProvider correlationIdProvider)
        : base(
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
        AbpKafkaEventBusOptions = abpKafkaEventBusOptions.Value;
        MessageConsumerFactory = messageConsumerFactory;
        Serializer = serializer;
        ProducerPool = producerPool;

        HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        EventTypes = new ConcurrentDictionary<string, Type>();
        AnonymousHandlerFactories = new ConcurrentDictionary<string, List<IEventHandlerFactory>>();
    }

    public void Initialize()
    {
        Consumer = MessageConsumerFactory.Create(
            AbpKafkaEventBusOptions.TopicName,
            AbpKafkaEventBusOptions.GroupId,
            AbpKafkaEventBusOptions.ConnectionName);
        Consumer.OnMessageReceived(ProcessEventAsync);

        SubscribeHandlers(AbpDistributedEventBusOptions.Handlers);
    }

    private async Task ProcessEventAsync(Message<string, byte[]> message)
    {
        var eventName = message.Key;
        var eventType = EventTypes.GetOrDefault(eventName);

        var messageId = message.GetMessageId();
        var correlationId = message.GetCorrelationId();
        object eventData;

        if (eventType != null)
        {
            eventData = Serializer.Deserialize(message.Value, eventType);
        }
        else if (AnonymousHandlerFactories.ContainsKey(eventName))
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(message.Value);
            eventData = new AnonymousEventData(eventName, jsonElement);
            eventType = typeof(AnonymousEventData);
        }
        else
        {
            return;
        }

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
                        factory is SingleInstanceHandlerFactory handlerFactory &&
                        handlerFactory.HandlerInstance == handler
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
            return PublishAsync(typeof(AnonymousEventData), new AnonymousEventData(eventName, eventData), onUnitOfWorkComplete);
        }

        throw new AbpException($"Unknown event name: {eventName}");
    }

    protected override async Task PublishToEventBusAsync(Type eventType, object eventData)
    {
        var headers = new Headers
        {
            { "messageId", System.Text.Encoding.UTF8.GetBytes(Guid.NewGuid().ToString("N")) }
        };

        if (CorrelationIdProvider.Get() != null)
        {
            headers.Add(EventBusConsts.CorrelationIdHeaderName, System.Text.Encoding.UTF8.GetBytes(CorrelationIdProvider.Get()!));
        }

        await PublishAsync(
            AbpKafkaEventBusOptions.TopicName,
            eventType,
            eventData,
            headers
        );
    }

    protected override void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
    {
        unitOfWork.AddOrReplaceDistributedEvent(eventRecord);
    }

    public override async Task PublishFromOutboxAsync(
        OutgoingEventInfo outgoingEvent,
        OutboxConfig outboxConfig)
    {
        var headers = new Headers
        {
            { "messageId", System.Text.Encoding.UTF8.GetBytes(outgoingEvent.Id.ToString("N")) }
        };
        if (outgoingEvent.GetCorrelationId() != null)
        {
            headers.Add(EventBusConsts.CorrelationIdHeaderName, System.Text.Encoding.UTF8.GetBytes(outgoingEvent.GetCorrelationId()!));
        }

        var result = await PublishAsync(
            AbpKafkaEventBusOptions.TopicName,
            outgoingEvent.EventName,
            outgoingEvent.EventData,
            headers
        );

        if (result.Status != PersistenceStatus.Persisted)
        {
            throw new AbpException($"Failed to publish event '{outgoingEvent.EventName}' to topic '{AbpKafkaEventBusOptions.TopicName}'. Status: {result.Status}");
        }

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

    public override async Task PublishManyFromOutboxAsync(IEnumerable<OutgoingEventInfo> outgoingEvents, OutboxConfig outboxConfig)
    {
        var producer = ProducerPool.Get(AbpKafkaEventBusOptions.ConnectionName);
        var outgoingEventArray = outgoingEvents.ToArray();

        foreach (var outgoingEvent in outgoingEventArray)
        {
            var messageId = outgoingEvent.Id.ToString("N");
            var headers = new Headers
            {
                { "messageId", System.Text.Encoding.UTF8.GetBytes(messageId)}
            };

            if (outgoingEvent.GetCorrelationId() != null)
            {
                headers.Add(EventBusConsts.CorrelationIdHeaderName, System.Text.Encoding.UTF8.GetBytes(outgoingEvent.GetCorrelationId()!));
            }

            var result = await producer.ProduceAsync(
                AbpKafkaEventBusOptions.TopicName,
                new Message<string, byte[]>
                {
                    Key = outgoingEvent.EventName,
                    Value = outgoingEvent.EventData,
                    Headers = headers
                });

            if (result.Status != PersistenceStatus.Persisted)
            {
                throw new AbpException($"Failed to publish event '{outgoingEvent.EventName}' to topic '{AbpKafkaEventBusOptions.TopicName}'. Status: {result.Status}");
            }

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

    public override async Task ProcessFromInboxAsync(
        IncomingEventInfo incomingEvent,
        InboxConfig inboxConfig)
    {
        var eventType = EventTypes.GetOrDefault(incomingEvent.EventName);
        object eventData;

        if (eventType != null)
        {
            eventData = Serializer.Deserialize(incomingEvent.EventData, eventType);
        }
        else if (AnonymousHandlerFactories.ContainsKey(incomingEvent.EventName))
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(incomingEvent.EventData);
            eventData = new AnonymousEventData(incomingEvent.EventName, jsonElement);
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

    private async Task PublishAsync(string topicName, Type eventType, object eventData, Headers headers)
    {
        var (eventName, resolvedData) = ResolveEventForPublishing(eventType, eventData);
        var body = Serializer.Serialize(resolvedData);

        var result = await PublishAsync(topicName, eventName, body, headers);
        if (result.Status != PersistenceStatus.Persisted)
        {
            throw new AbpException($"Failed to publish event '{eventName}' to topic '{topicName}'. Status: {result.Status}");
        }
    }

    private Task<DeliveryResult<string, byte[]>> PublishAsync(
        string topicName,
        string eventName,
        byte[] body,
        Headers headers)
    {
        var producer = ProducerPool.Get(AbpKafkaEventBusOptions.ConnectionName);

        return producer.ProduceAsync(
            topicName,
            new Message<string, byte[]>
            {
                Key = eventName,
                Value = body,
                Headers = headers
            });
    }

    protected override Task OnAddToOutboxAsync(string eventName, Type eventType, object eventData)
    {
        EventTypes.GetOrAdd(eventName, eventType);
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

        foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key))
        )
        {
            handlerFactoryList.Add(
                new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
        }

        return handlerFactoryList.ToArray();
    }

    protected override Type? GetEventTypeByEventName(string eventName)
    {
        return EventTypes.GetOrDefault(eventName);
    }

    public override IDisposable Subscribe(string eventName, IEventHandlerFactory handler)
    {
        GetOrCreateAnonymousHandlerFactories(eventName).Locking(factories =>
        {
            if (!handler.IsInFactories(factories))
            {
                factories.Add(handler);
            }
        });

        return new AnonymousEventHandlerFactoryUnregistrar(this, eventName, handler);
    }

    public override void Unsubscribe(string eventName, IEventHandlerFactory factory)
    {
        GetOrCreateAnonymousHandlerFactories(eventName).Locking(factories => factories.Remove(factory));
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

        //Should trigger for inherited types
        if (handlerEventType.IsAssignableFrom(targetEventType))
        {
            return true;
        }

        return false;
    }
}
