using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Tracing;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Distributed;

public abstract class DistributedEventBusBase : EventBusBase, IDistributedEventBus, ISupportsEventBoxes
{
    protected IGuidGenerator GuidGenerator { get; }
    protected IClock Clock { get; }
    protected AbpDistributedEventBusOptions AbpDistributedEventBusOptions { get; }
    protected ILocalEventBus LocalEventBus { get; }
    protected ICorrelationIdProvider CorrelationIdProvider { get; }

    protected DistributedEventBusBase(
        IServiceScopeFactory serviceScopeFactory,
        ICurrentTenant currentTenant,
        IUnitOfWorkManager unitOfWorkManager,
        IOptions<AbpDistributedEventBusOptions> abpDistributedEventBusOptions,
        IGuidGenerator guidGenerator,
        IClock clock,
        IEventHandlerInvoker eventHandlerInvoker,
        ILocalEventBus localEventBus,
        ICorrelationIdProvider correlationIdProvider) : base(
        serviceScopeFactory,
        currentTenant,
        unitOfWorkManager,
        eventHandlerInvoker)
    {
        GuidGenerator = guidGenerator;
        Clock = clock;
        AbpDistributedEventBusOptions = abpDistributedEventBusOptions.Value;
        LocalEventBus = localEventBus;
        CorrelationIdProvider = correlationIdProvider;
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : class
    {
        return Subscribe(typeof(TEvent), handler);
    }

    /// <inheritdoc/>
    public virtual IDisposable Subscribe(string eventName, IDistributedEventHandler<DynamicEventData> handler)
    {
        return Subscribe(eventName, (IEventHandler)handler);
    }

    /// <inheritdoc/>
    public override Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true)
    {
        return PublishAsync(eventType, eventData, onUnitOfWorkComplete, useOutbox: true);
    }

    /// <inheritdoc/>
    public virtual Task PublishAsync<TEvent>(
        TEvent eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true)
        where TEvent : class
    {
        return PublishAsync(typeof(TEvent), eventData, onUnitOfWorkComplete, useOutbox);
    }

    /// <inheritdoc/>
    public virtual async Task PublishAsync(
        Type eventType,
        object eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true)
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

        await PublishToEventBusAsync(eventType, eventData);

        await TriggerDistributedEventSentAsync(new DistributedEventSent()
        {
            Source = DistributedEventSource.Direct,
            EventName = GetEventName(eventType, eventData),
            EventData = GetEventData(eventData)
        });
    }

    /// <inheritdoc/>
    public virtual Task PublishAsync(
        string eventName,
        object eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true)
    {
        var eventType = GetEventTypeByEventName(eventName);
        var dynamicEventData = eventData as DynamicEventData ?? new DynamicEventData(eventName, eventData);

        if (eventType != null)
        {
            return PublishAsync(eventType, ConvertDynamicEventData(dynamicEventData.Data, eventType), onUnitOfWorkComplete, useOutbox);
        }

        return PublishAsync(typeof(DynamicEventData), dynamicEventData, onUnitOfWorkComplete, useOutbox);
    }

    public abstract Task PublishFromOutboxAsync(
        OutgoingEventInfo outgoingEvent,
        OutboxConfig outboxConfig
    );

    public abstract Task PublishManyFromOutboxAsync(
        IEnumerable<OutgoingEventInfo> outgoingEvents,
        OutboxConfig outboxConfig
    );

    public abstract Task ProcessFromInboxAsync(
        IncomingEventInfo incomingEvent,
        InboxConfig inboxConfig);

    protected virtual async Task<bool> AddToOutboxAsync(Type eventType, object eventData)
    {
        var unitOfWork = UnitOfWorkManager.Current;
        if (unitOfWork == null)
        {
            return false;
        }

        var addedToOutbox = false;

        foreach (var outboxConfig in AbpDistributedEventBusOptions.Outboxes.Values.OrderBy(x => x.Selector is null))
        {
            if (outboxConfig.Selector == null || outboxConfig.Selector(eventType))
            {
                var eventOutbox = (IEventOutbox)unitOfWork.ServiceProvider.GetRequiredService(outboxConfig.ImplementationType);
                (var eventName, eventData) = ResolveEventForPublishing(eventType, eventData);

                await OnAddToOutboxAsync(eventName, eventType, eventData);

                var outgoingEventInfo = new OutgoingEventInfo(
                    GuidGenerator.Create(),
                    eventName,
                    Serialize(eventData),
                    Clock.Now
                );

                var correlationId = CorrelationIdProvider.Get();
                if (correlationId != null)
                {
                    outgoingEventInfo.SetCorrelationId(correlationId);
                }

                await eventOutbox.EnqueueAsync(outgoingEventInfo);
                addedToOutbox = true;
            }
        }

        return addedToOutbox;
    }

    protected virtual Task OnAddToOutboxAsync(string eventName, Type eventType, object eventData)
    {
        return Task.CompletedTask;
    }

    protected virtual async Task<bool> AddToInboxAsync(
        string? messageId,
        string eventName,
        Type eventType,
        object eventData,
        string? correlationId)
    {
        if (AbpDistributedEventBusOptions.Inboxes.Count <= 0)
        {
            return false;
        }

        var addToInbox = false;

        using (var scope = ServiceScopeFactory.CreateScope())
        {
            foreach (var inboxConfig in AbpDistributedEventBusOptions.Inboxes.Values.OrderBy(x => x.EventSelector is null))
            {
                if (inboxConfig.EventSelector == null || inboxConfig.EventSelector(eventType))
                {
                    var eventInbox =
                        (IEventInbox)scope.ServiceProvider.GetRequiredService(inboxConfig.ImplementationType);

                    if (!messageId.IsNullOrEmpty())
                    {
                        if (await eventInbox.ExistsByMessageIdAsync(messageId!))
                        {
                            addToInbox = true;
                            continue;
                        }
                    }
                    
                    eventData = GetEventData(eventData);

                    var incomingEventInfo = new IncomingEventInfo(
                        GuidGenerator.Create(),
                        messageId!,
                        eventName,
                        Serialize(eventData),
                        Clock.Now
                    );
                    incomingEventInfo.SetCorrelationId(correlationId!);
                    await eventInbox.EnqueueAsync(incomingEventInfo);
                    addToInbox = true;
                }
            }
        }

        return addToInbox;
    }

    protected abstract byte[] Serialize(object eventData);

    protected virtual async Task TriggerHandlersDirectAsync(Type eventType, object eventData)
    {
        await TriggerDistributedEventReceivedAsync(new DistributedEventReceived
        {
            Source = DistributedEventSource.Direct,
            EventName = GetEventName(eventType, eventData),
            EventData = GetEventData(eventData)
        });

        await TriggerHandlersAsync(eventType, eventData);
    }

    protected virtual async Task TriggerHandlersFromInboxAsync(Type eventType, object eventData, List<Exception> exceptions, InboxConfig? inboxConfig = null)
    {
        await TriggerDistributedEventReceivedAsync(new DistributedEventReceived
        {
            Source = DistributedEventSource.Inbox,
            EventName = GetEventName(eventType, eventData),
            EventData = GetEventData(eventData)
        });

        await TriggerHandlersAsync(eventType, eventData, exceptions, inboxConfig);
    }

    public virtual async Task TriggerDistributedEventSentAsync(DistributedEventSent distributedEvent)
    {
        try
        {
            await LocalEventBus.PublishAsync(distributedEvent, onUnitOfWorkComplete: false);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    public virtual async Task TriggerDistributedEventReceivedAsync(DistributedEventReceived distributedEvent)
    {
        try
        {
            await LocalEventBus.PublishAsync(distributedEvent, false);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    protected virtual string GetEventName(Type eventType, object eventData)
    {
        if (eventData is DynamicEventData dynamicEventData)
        {
            return dynamicEventData.EventName;
        }

        return EventNameAttribute.GetNameOrDefault(eventType);
    }

    protected virtual object GetEventData(object eventData)
    {
        if (eventData is DynamicEventData dynamicEventData)
        {
            return dynamicEventData.Data;
        }

        return eventData;
    }

    protected virtual (string EventName, object EventData) ResolveEventForPublishing(Type eventType, object eventData)
    {
        return (GetEventName(eventType, eventData), GetEventData(eventData));
    }
}
