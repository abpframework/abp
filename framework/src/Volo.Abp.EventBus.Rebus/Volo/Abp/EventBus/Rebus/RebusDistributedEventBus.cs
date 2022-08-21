﻿using System;
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
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
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
        IEventHandlerInvoker eventHandlerInvoker) :
        base(
            serviceScopeFactory,
            currentTenant,
            unitOfWorkManager,
            abpDistributedEventBusOptions,
            guidGenerator,
            clock,
            eventHandlerInvoker)
    {
        Rebus = rebus;
        Serializer = serializer;
        AbpRebusEventBusOptions = abpEventBusRebusOptions.Value;

        HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
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

        if (handlerFactories.Count == 1) //TODO: Multi-threading!
        {
            Rebus.Subscribe(eventType);
        }

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

    public async Task ProcessEventAsync(Type eventType, object eventData)
    {
        var messageId = MessageContext.Current.TransportMessage.GetMessageId();
        var eventName = EventNameAttribute.GetNameOrDefault(eventType);

        if (await AddToInboxAsync(messageId, eventName, eventType, MessageContext.Current.TransportMessage.Body))
        {
            return;
        }

        await TriggerHandlersAsync(eventType, eventData);
    }

    protected async override Task PublishToEventBusAsync(Type eventType, object eventData)
    {
        await PublishAsync(eventType, eventData);
    }

    protected virtual async Task PublishAsync(
        Type eventType,
        object eventData,
        Guid? eventId = null,
        Dictionary<string, string> headersArguments = null)
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

    protected override void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
    {
        unitOfWork.AddOrReplaceDistributedEvent(eventRecord);
    }

    private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
    {
        return HandlerFactories.GetOrAdd(
            eventType,
            type =>
            {
                var eventName = EventNameAttribute.GetNameOrDefault(type);
                EventTypes[eventName] = type;
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

    public override Task PublishFromOutboxAsync(
        OutgoingEventInfo outgoingEvent,
        OutboxConfig outboxConfig)
    {
        var eventType = EventTypes.GetOrDefault(outgoingEvent.EventName);
        var eventData = Serializer.Deserialize(outgoingEvent.EventData, eventType);

        return PublishAsync(eventType, eventData, eventId: outgoingEvent.Id);
    }

    public async override Task PublishManyFromOutboxAsync(IEnumerable<OutgoingEventInfo> outgoingEvents, OutboxConfig outboxConfig)
    {
        var outgoingEventArray = outgoingEvents.ToArray();

        using (var scope = new RebusTransactionScope())
        {
            foreach (var outgoingEvent in outgoingEventArray)
            {
                await PublishFromOutboxAsync(outgoingEvent, outboxConfig);
            }
            
            await scope.CompleteAsync();
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
        await TriggerHandlersAsync(eventType, eventData, exceptions, inboxConfig);
        if (exceptions.Any())
        {
            ThrowOriginalExceptions(eventType, exceptions);
        }
    }

    protected override byte[] Serialize(object eventData)
    {
        return Serializer.Serialize(eventData);
    }
}
