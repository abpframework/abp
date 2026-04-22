using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Collections;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Tracing;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Distributed;

[Dependency(TryRegister = true)]
[ExposeServices(typeof(IDistributedEventBus), typeof(LocalDistributedEventBus))]
public class LocalDistributedEventBus : DistributedEventBusBase, ISingletonDependency
{
    protected ConcurrentDictionary<string, Type> EventTypes { get; }

    protected ConcurrentDictionary<string, bool> DynamicEventNames { get; }

    public LocalDistributedEventBus(
        IServiceScopeFactory serviceScopeFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IOptions<AbpDistributedEventBusOptions> abpDistributedEventBusOptions,
        IGuidGenerator guidGenerator,
        IClock clock,
        IEventHandlerInvoker eventHandlerInvoker,
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
        EventTypes = new ConcurrentDictionary<string, Type>();
        DynamicEventNames = new ConcurrentDictionary<string, bool>();
        Subscribe(abpDistributedEventBusOptions.Value.Handlers);
    }

    public virtual void Subscribe(ITypeList<IEventHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            var interfaces = handler.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(@interface))
                {
                    continue;
                }

                var genericArgs = @interface.GetGenericArguments();
                if (genericArgs.Length == 1)
                {
                    Subscribe(genericArgs[0], new IocEventHandlerFactory(ServiceScopeFactory, handler));
                }
            }
        }
    }

    /// <inheritdoc/>
    public override IDisposable Subscribe(string eventName, IEventHandlerFactory handler)
    {
        DynamicEventNames.GetOrAdd(eventName, true);
        return LocalEventBus.Subscribe(eventName, handler);
    }

    /// <inheritdoc/>
    public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
        var eventName = EventNameAttribute.GetNameOrDefault(eventType);
        EventTypes.GetOrAdd(eventName, eventType);
        return LocalEventBus.Subscribe(eventType, factory);
    }

    public override void Unsubscribe<TEvent>(Func<TEvent, Task> action)
    {
        LocalEventBus.Unsubscribe(action);
    }

    public override void Unsubscribe(Type eventType, IEventHandler handler)
    {
        LocalEventBus.Unsubscribe(eventType, handler);
    }

    public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
    {
        LocalEventBus.Unsubscribe(eventType, factory);
    }

    /// <inheritdoc/>
    public override void Unsubscribe(string eventName, IEventHandlerFactory factory)
    {
        LocalEventBus.Unsubscribe(eventName, factory);
    }

    /// <inheritdoc/>
    public override void Unsubscribe(string eventName, IEventHandler handler)
    {
        LocalEventBus.Unsubscribe(eventName, handler);
    }

    /// <inheritdoc/>
    public override void UnsubscribeAll(Type eventType)
    {
        LocalEventBus.UnsubscribeAll(eventType);
    }

    /// <inheritdoc/>
    public override void UnsubscribeAll(string eventName)
    {
        LocalEventBus.UnsubscribeAll(eventName);
    }

    /// <inheritdoc/>
    public async override Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true, bool useOutbox = true)
    {
        if (onUnitOfWorkComplete && UnitOfWorkManager.Current != null)
        {
            AddToUnitOfWork(
                UnitOfWorkManager.Current,
                new UnitOfWorkEventRecord(eventType, eventData, EventOrderGenerator.GetNext(), useOutbox)
            );
            return;
        }

        if (useOutbox)
        {
            if (await AddToOutboxAsync(eventType, eventData))
            {
                return;
            }
        }

        await TriggerDistributedEventSentAsync(new DistributedEventSent()
        {
            Source = DistributedEventSource.Direct,
            EventName = GetEventName(eventType, eventData),
            EventData = GetEventData(eventData)
        });

        await TriggerDistributedEventReceivedAsync(new DistributedEventReceived
        {
            Source = DistributedEventSource.Direct,
            EventName = GetEventName(eventType, eventData),
            EventData = GetEventData(eventData)
        });

        await PublishToEventBusAsync(eventType, eventData);
    }

    /// <inheritdoc/>
    public override Task PublishAsync(string eventName, object eventData, bool onUnitOfWorkComplete = true)
    {
        return PublishAsync(eventName, eventData, onUnitOfWorkComplete, useOutbox: true);
    }

    /// <inheritdoc/>
    public override Task PublishAsync(string eventName, object eventData, bool onUnitOfWorkComplete = true, bool useOutbox = true)
    {
        var eventType = EventTypes.GetOrDefault(eventName);
        var dynamicEventData = eventData as DynamicEventData ?? new DynamicEventData(eventName, eventData);

        if (eventType != null)
        {
            return PublishAsync(eventType, ConvertDynamicEventData(dynamicEventData.Data, eventType), onUnitOfWorkComplete, useOutbox);
        }

        return PublishAsync(typeof(DynamicEventData), dynamicEventData, onUnitOfWorkComplete, useOutbox);
    }

    protected async override Task PublishToEventBusAsync(Type eventType, object eventData)
    {
        if (await AddToInboxAsync(Guid.NewGuid().ToString(), GetEventName(eventType, eventData), eventType, eventData, null))
        {
            return;
        }

        await LocalEventBus.PublishAsync(eventType, eventData, false);
    }

    protected override void AddToUnitOfWork(IUnitOfWork unitOfWork, UnitOfWorkEventRecord eventRecord)
    {
        unitOfWork.AddOrReplaceDistributedEvent(eventRecord);
    }

    public async override Task PublishFromOutboxAsync(OutgoingEventInfo outgoingEvent, OutboxConfig outboxConfig)
    {
        await TriggerDistributedEventSentAsync(new DistributedEventSent()
        {
            Source = DistributedEventSource.Outbox,
            EventName = outgoingEvent.EventName,
            EventData = outgoingEvent.EventData
        });

        await TriggerDistributedEventReceivedAsync(new DistributedEventReceived
        {
            Source = DistributedEventSource.Direct,
            EventName = outgoingEvent.EventName,
            EventData = outgoingEvent.EventData
        });

        var eventType = EventTypes.GetOrDefault(outgoingEvent.EventName);
        if (eventType == null)
        {
            var isDynamic = DynamicEventNames.ContainsKey(outgoingEvent.EventName);
            if (!isDynamic)
            {
                return;
            }

            eventType = typeof(DynamicEventData);
        }

        object eventData;
        if (eventType == typeof(DynamicEventData))
        {
            eventData = new DynamicEventData(
                outgoingEvent.EventName,
                System.Text.Json.JsonSerializer.Deserialize<object>(outgoingEvent.EventData)!);
        }
        else
        {
            eventData = System.Text.Json.JsonSerializer.Deserialize(outgoingEvent.EventData, eventType)!;
        }

        if (await AddToInboxAsync(Guid.NewGuid().ToString(), outgoingEvent.EventName, eventType, eventData, null))
        {
            return;
        }

        await LocalEventBus.PublishAsync(eventType, eventData, false);
    }

    public async override Task PublishManyFromOutboxAsync(IEnumerable<OutgoingEventInfo> outgoingEvents, OutboxConfig outboxConfig)
    {
        foreach (var outgoingEvent in outgoingEvents)
        {
            await PublishFromOutboxAsync(outgoingEvent, outboxConfig);
        }
    }

    public async override Task ProcessFromInboxAsync(IncomingEventInfo incomingEvent, InboxConfig inboxConfig)
    {
        var eventType = EventTypes.GetOrDefault(incomingEvent.EventName);
        if (eventType == null)
        {
            var isDynamic = DynamicEventNames.ContainsKey(incomingEvent.EventName);
            if (!isDynamic)
            {
                return;
            }

            eventType = typeof(DynamicEventData);
        }

        object eventData;
        if (eventType == typeof(DynamicEventData))
        {
            eventData = new DynamicEventData(
                incomingEvent.EventName,
                System.Text.Json.JsonSerializer.Deserialize<object>(incomingEvent.EventData)!);
        }
        else
        {
            eventData = System.Text.Json.JsonSerializer.Deserialize(incomingEvent.EventData, eventType)!;
        }

        var exceptions = new List<Exception>();
        using (CorrelationIdProvider.Change(incomingEvent.GetCorrelationId()))
        {
            await TriggerHandlersFromInboxAsync(eventType, eventData!, exceptions, inboxConfig);
        }
        if (exceptions.Any())
        {
            ThrowOriginalExceptions(eventType, exceptions);
        }
    }

    protected override byte[] Serialize(object eventData)
    {
        return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(eventData);
    }

    protected override Task OnAddToOutboxAsync(string eventName, Type eventType, object eventData)
    {
        if (eventType != typeof(DynamicEventData))
        {
            EventTypes.GetOrAdd(eventName, eventType);
        }
        return base.OnAddToOutboxAsync(eventName, eventType, eventData);
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
    {
        return LocalEventBus.GetEventHandlerFactories(eventType);
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetDynamicHandlerFactories(string eventName)
    {
        return LocalEventBus.GetDynamicEventHandlerFactories(eventName);
    }

    protected override Type? GetEventTypeByEventName(string eventName)
    {
        return EventTypes.GetOrDefault(eventName);
    }
}
