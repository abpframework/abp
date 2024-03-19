using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
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
[ExposeServices(typeof(IDistributedEventBus), typeof(RabbitMqDistributedEventBus))]
public class RabbitMqDistributedEventBus : DistributedEventBusBase, ISingletonDependency
{
    protected AbpRabbitMqEventBusOptions AbpRabbitMqEventBusOptions { get; }
    protected IConnectionPool ConnectionPool { get; }
    protected IRabbitMqSerializer Serializer { get; }

    //TODO: Accessing to the List<IEventHandlerFactory> may not be thread-safe!
    protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
    protected ConcurrentDictionary<string, Type> EventTypes { get; }
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
    }

    public void Initialize()
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

    private async Task ProcessEventAsync(IModel channel, BasicDeliverEventArgs ea)
    {
        var eventName = ea.RoutingKey;
        var eventType = EventTypes.GetOrDefault(eventName);
        if (eventType == null)
        {
            return;
        }

        var eventData = Serializer.Deserialize(ea.Body.ToArray(), eventType);

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
        using (CorrelationIdProvider.Change(outgoingEvent.GetCorrelationId()))
        {
            await TriggerDistributedEventSentAsync(new DistributedEventSent()
            {
                Source = DistributedEventSource.Outbox,
                EventName = outgoingEvent.EventName,
                EventData = outgoingEvent.EventData
            });
        }

        await PublishAsync(outgoingEvent.EventName, outgoingEvent.EventData, eventId: outgoingEvent.Id, correlationId: outgoingEvent.GetCorrelationId());
    }

    public async override Task PublishManyFromOutboxAsync(
        IEnumerable<OutgoingEventInfo> outgoingEvents,
        OutboxConfig outboxConfig)
    {
        using (var channel = ConnectionPool.Get(AbpRabbitMqEventBusOptions.ConnectionName).CreateModel())
        {
            var outgoingEventArray = outgoingEvents.ToArray();
            channel.ConfirmSelect();

            foreach (var outgoingEvent in outgoingEventArray)
            {
                using (CorrelationIdProvider.Change(outgoingEvent.GetCorrelationId()))
                {
                    await TriggerDistributedEventSentAsync(new DistributedEventSent()
                    {
                        Source = DistributedEventSource.Outbox,
                        EventName = outgoingEvent.EventName,
                        EventData = outgoingEvent.EventData
                    });
                }

                await PublishAsync(
                    channel,
                    outgoingEvent.EventName,
                    outgoingEvent.EventData,
                    eventId: outgoingEvent.Id,
                    correlationId: outgoingEvent.GetCorrelationId());
            }

            channel.WaitForConfirmsOrDie();
        }
    }

    public async override Task ProcessFromInboxAsync(
        IncomingEventInfo incomingEvent,
        InboxConfig inboxConfig)
    {
        var eventType = EventTypes.GetOrDefault(incomingEvent.EventName);
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

    public virtual Task PublishAsync(
        Type eventType,
        object eventData,
        Dictionary<string, object>? headersArguments = null,
        Guid? eventId = null,
        string? correlationId = null)
    {
        var eventName = EventNameAttribute.GetNameOrDefault(eventType);
        var body = Serializer.Serialize(eventData);

        return PublishAsync( eventName, body, headersArguments, eventId, correlationId);
    }

    protected virtual Task PublishAsync(
        string eventName,
        byte[] body,
        Dictionary<string, object>? headersArguments = null,
        Guid? eventId = null,
        string? correlationId = null)
    {
        using (var channel = ConnectionPool.Get(AbpRabbitMqEventBusOptions.ConnectionName).CreateModel())
        {
            return PublishAsync(channel, eventName, body, headersArguments, eventId, correlationId);
        }
    }

    protected virtual Task PublishAsync(
        IModel channel,
        string eventName,
        byte[] body,
        Dictionary<string, object>? headersArguments = null,
        Guid? eventId = null,
        string? correlationId = null)
    {
        EnsureExchangeExists(channel);

        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = RabbitMqConsts.DeliveryModes.Persistent;

        if (properties.MessageId.IsNullOrEmpty())
        {
            properties.MessageId = (eventId ?? GuidGenerator.Create()).ToString("N");
        }

        if (correlationId != null)
        {
            properties.CorrelationId = correlationId;
        }

        SetEventMessageHeaders(properties, headersArguments);

        channel.BasicPublish(
            exchange: AbpRabbitMqEventBusOptions.ExchangeName,
            routingKey: eventName,
            mandatory: true,
            basicProperties: properties,
            body: body
        );

        return Task.CompletedTask;
    }

    private void EnsureExchangeExists(IModel channel)
    {
        if (_exchangeCreated)
        {
            return;
        }

        try
        {
            using (var temporaryChannel = ConnectionPool.Get(AbpRabbitMqEventBusOptions.ConnectionName).CreateModel())
            {
                temporaryChannel.ExchangeDeclarePassive(AbpRabbitMqEventBusOptions.ExchangeName);
            }
        }
        catch (Exception)
        {
            channel.ExchangeDeclare(
                AbpRabbitMqEventBusOptions.ExchangeName,
                AbpRabbitMqEventBusOptions.GetExchangeTypeOrDefault(),
                durable: true
            );
        }
        _exchangeCreated = true;
    }

    private void SetEventMessageHeaders(IBasicProperties properties, Dictionary<string, object>? headersArguments)
    {
        if (headersArguments == null)
        {
            return;
        }

        properties.Headers ??= new Dictionary<string, object>();

        foreach (var header in headersArguments)
        {
            properties.Headers[header.Key] = header.Value;
        }
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

        foreach (var handlerFactory in
                 HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
        {
            handlerFactoryList.Add(
                new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
        }

        return handlerFactoryList.ToArray();
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
