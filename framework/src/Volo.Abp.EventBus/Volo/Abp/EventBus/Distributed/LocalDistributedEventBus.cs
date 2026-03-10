using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

    protected ConcurrentDictionary<string, bool> AnonymousEventNames { get; }

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
        AnonymousEventNames = new ConcurrentDictionary<string, bool>();
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
        AnonymousEventNames.GetOrAdd(eventName, true);
        LocalEventBus.Subscribe(eventName, handler);
        return new AnonymousEventHandlerFactoryUnregistrar(this, eventName, handler);
    }

    /// <inheritdoc/>
    public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
        var eventName = EventNameAttribute.GetNameOrDefault(eventType);
        EventTypes.GetOrAdd(eventName, eventType);
        LocalEventBus.Subscribe(eventType, factory);
        return new EventHandlerFactoryUnregistrar(this, eventType, factory);
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
        CleanupAnonymousEventName(eventName);
    }

    /// <inheritdoc/>
    public override void Unsubscribe(string eventName, IEventHandler handler)
    {
        LocalEventBus.Unsubscribe(eventName, handler);
        CleanupAnonymousEventName(eventName);
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
        CleanupAnonymousEventName(eventName);
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
        var anonymousEventData = CreateAnonymousEnvelope(eventName, eventData);
        return TryPublishTypedByEventNameAsync(eventName, anonymousEventData, onUnitOfWorkComplete, useOutbox)
            ?? PublishAnonymousByEventNameAsync(anonymousEventData, onUnitOfWorkComplete, useOutbox);
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

        if (!TryResolveStoredEventType(outgoingEvent.EventName, out var eventType))
        {
            return;
        }

        var eventData = DeserializeStoredEventData(outgoingEvent.EventName, outgoingEvent.EventData, eventType);
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
        if (!TryResolveStoredEventType(incomingEvent.EventName, out var eventType))
        {
            return;
        }

        var eventData = DeserializeStoredEventData(incomingEvent.EventName, incomingEvent.EventData, eventType);
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
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(eventData));
    }

    protected override Task OnAddToOutboxAsync(string eventName, Type eventType, object eventData)
    {
        if (eventType != typeof(AnonymousEventData))
        {
            EventTypes.GetOrAdd(eventName, eventType);
        }
        return base.OnAddToOutboxAsync(eventName, eventType, eventData);
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
    {
        return LocalEventBus.GetEventHandlerFactories(eventType);
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetAnonymousHandlerFactories(string eventName)
    {
        return LocalEventBus.GetAnonymousEventHandlerFactories(eventName);
    }

    protected override Type? GetEventTypeByEventName(string eventName)
    {
        return EventTypes.GetOrDefault(eventName);
    }

    protected override AnonymousEventData CreateAnonymousEnvelope(string eventName, object eventData)
    {
        return eventData as AnonymousEventData ?? new AnonymousEventData(eventName, eventData);
    }

    protected override Task? TryPublishTypedByEventNameAsync(
        string eventName,
        AnonymousEventData anonymousEventData,
        bool onUnitOfWorkComplete,
        bool useOutbox)
    {
        var eventType = EventTypes.GetOrDefault(eventName);
        if (eventType == null)
        {
            return null;
        }

        var typedEventData = AnonymousEventDataConverter.ConvertToTypedObject(anonymousEventData, eventType);
        return PublishAsync(eventType, typedEventData, onUnitOfWorkComplete, useOutbox);
    }

    protected override Task PublishAnonymousByEventNameAsync(
        AnonymousEventData anonymousEventData,
        bool onUnitOfWorkComplete,
        bool useOutbox)
    {
        if (!HasAnonymousEventName(anonymousEventData.EventName))
        {
            return Task.CompletedTask;
        }

        return PublishAsync(typeof(AnonymousEventData), anonymousEventData, onUnitOfWorkComplete, useOutbox);
    }

    protected virtual bool TryResolveStoredEventType(string eventName, out Type eventType)
    {
        eventType = EventTypes.GetOrDefault(eventName)!;
        if (eventType != null)
        {
            return true;
        }

        if (!HasAnonymousEventName(eventName))
        {
            return false;
        }

        eventType = typeof(AnonymousEventData);
        return true;
    }

    protected virtual object DeserializeStoredEventData(string eventName, byte[] eventData, Type eventType)
    {
        if (eventType == typeof(AnonymousEventData))
        {
            return CreateAnonymousEventData(eventName, eventData);
        }

        return JsonSerializer.Deserialize(Encoding.UTF8.GetString(eventData), eventType)!;
    }

    protected virtual void CleanupAnonymousEventName(string eventName)
    {
        if (!LocalEventBus.GetAnonymousEventHandlerFactories(eventName).Any())
        {
            AnonymousEventNames.TryRemove(eventName, out _);
        }
    }

    protected virtual bool HasAnonymousEventName(string eventName)
    {
        if (!AnonymousEventNames.ContainsKey(eventName))
        {
            return false;
        }

        if (!LocalEventBus.GetAnonymousEventHandlerFactories(eventName).Any())
        {
            AnonymousEventNames.TryRemove(eventName, out _);
            return false;
        }

        return true;
    }
}
