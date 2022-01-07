using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.AzureServiceBus;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Azure;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IDistributedEventBus), typeof(AzureDistributedEventBus))]
public class AzureDistributedEventBus : DistributedEventBusBase, ISingletonDependency
{
    private readonly AbpAzureEventBusOptions _options;
    private readonly IAzureServiceBusMessageConsumerFactory _messageConsumerFactory;
    private readonly IPublisherPool _publisherPool;
    private readonly IAzureServiceBusSerializer _serializer;
    private readonly ConcurrentDictionary<Type, List<IEventHandlerFactory>> _handlerFactories;
    private readonly ConcurrentDictionary<string, Type> _eventTypes;
    private IAzureServiceBusMessageConsumer _consumer;

    public AzureDistributedEventBus(
        IServiceScopeFactory serviceScopeFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IOptions<AbpDistributedEventBusOptions> abpDistributedEventBusOptions,
        IGuidGenerator guidGenerator,
        IClock clock,
        IOptions<AbpAzureEventBusOptions> abpAzureEventBusOptions,
        IAzureServiceBusSerializer serializer,
        IAzureServiceBusMessageConsumerFactory messageConsumerFactory,
        IPublisherPool publisherPool,
        IEventHandlerInvoker eventHandlerInvoker)
        : base(serviceScopeFactory,
            currentTenant,
            unitOfWorkManager,
            abpDistributedEventBusOptions,
            guidGenerator,
            clock,
            eventHandlerInvoker)
    {
        _options = abpAzureEventBusOptions.Value;
        _serializer = serializer;
        _messageConsumerFactory = messageConsumerFactory;
        _publisherPool = publisherPool;
        _handlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        _eventTypes = new ConcurrentDictionary<string, Type>();
    }

    public void Initialize()
    {
        _consumer = _messageConsumerFactory.CreateMessageConsumer(
            _options.TopicName,
            _options.SubscriberName,
            _options.ConnectionName);

        _consumer.OnMessageReceived(ProcessEventAsync);
        SubscribeHandlers(AbpDistributedEventBusOptions.Handlers);
    }

    private async Task ProcessEventAsync(ServiceBusReceivedMessage message)
    {
        var eventName = message.Subject;
        var eventType = _eventTypes.GetOrDefault(eventName);
        if (eventType == null)
        {
            return;
        }

        if (await AddToInboxAsync(message.MessageId, eventName, eventType, message.Body.ToArray()))
        {
            return;
        }

        var eventData = _serializer.Deserialize(message.Body.ToArray(), eventType);

        await TriggerHandlersAsync(eventType, eventData);
    }

    public override async Task PublishFromOutboxAsync(OutgoingEventInfo outgoingEvent, OutboxConfig outboxConfig)
    {
        await PublishAsync(outgoingEvent.EventName, outgoingEvent.EventData);
    }

    public override async Task ProcessFromInboxAsync(IncomingEventInfo incomingEvent, InboxConfig inboxConfig)
    {
        var eventType = _eventTypes.GetOrDefault(incomingEvent.EventName);
        if (eventType == null)
        {
            return;
        }

        var eventData = _serializer.Deserialize(incomingEvent.EventData, eventType);
        var exceptions = new List<Exception>();
        await TriggerHandlersAsync(eventType, eventData, exceptions, inboxConfig);
        if (exceptions.Any())
        {
            ThrowOriginalExceptions(eventType, exceptions);
        }
    }

    protected override byte[] Serialize(object eventData)
    {
        return _serializer.Serialize(eventData);
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
                        factory is SingleInstanceHandlerFactory handlerFactory &&
                        handlerFactory.HandlerInstance == handler
                );
            });
    }

    public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
    {
        GetOrCreateHandlerFactories(eventType)
            .Locking(factories => factories.Remove(factory));
    }

    public override void UnsubscribeAll(Type eventType)
    {
        GetOrCreateHandlerFactories(eventType)
            .Locking(factories => factories.Clear());
    }

    protected override async Task PublishToEventBusAsync(Type eventType, object eventData)
    {
        await PublishAsync(EventNameAttribute.GetNameOrDefault(eventType), eventData);
    }

    protected override void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
    {
        unitOfWork.AddOrReplaceDistributedEvent(eventRecord);
    }

    protected virtual async Task PublishAsync(string eventName, object eventData)
    {
        var body = _serializer.Serialize(eventData);

        var message = new ServiceBusMessage(body)
        {
            Subject = eventName
        };

        var publisher = await _publisherPool.GetAsync(
            _options.TopicName,
            _options.ConnectionName);

        await publisher.SendMessageAsync(message);
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
    {
        return _handlerFactories
            .Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key))
            .Select(handlerFactory =>
                new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value))
            .ToArray();
    }

    private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
    {
        return handlerEventType == targetEventType || handlerEventType.IsAssignableFrom(targetEventType);
    }

    private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
    {
        return _handlerFactories.GetOrAdd(
            eventType,
            type =>
            {
                var eventName = EventNameAttribute.GetNameOrDefault(type);
                _eventTypes[eventName] = type;
                return new List<IEventHandlerFactory>();
            }
        );
    }
}
