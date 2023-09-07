using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
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

    public IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : class
    {
        return Subscribe(typeof(TEvent), handler);
    }

    public override Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true)
    {
        return PublishAsync(eventType, eventData, onUnitOfWorkComplete, useOutbox: true);
    }

    public Task PublishAsync<TEvent>(
        TEvent eventData,
        bool onUnitOfWorkComplete = true,
        bool useOutbox = true)
        where TEvent : class
    {
        return PublishAsync(typeof(TEvent), eventData, onUnitOfWorkComplete, useOutbox);
    }

    public async Task PublishAsync(
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

        await TriggerDistributedEventSentAsync(new DistributedEventSent()
        {
            Source = DistributedEventSource.Direct,
            EventName = EventNameAttribute.GetNameOrDefault(eventType),
            EventData = eventData
        });

        await PublishToEventBusAsync(eventType, eventData);
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

        foreach (var outboxConfig in AbpDistributedEventBusOptions.Outboxes.Values.OrderBy(x => x.Selector is null))
        {
            if (outboxConfig.Selector == null || outboxConfig.Selector(eventType))
            {
                var eventOutbox = (IEventOutbox)unitOfWork.ServiceProvider.GetRequiredService(outboxConfig.ImplementationType);
                var eventName = EventNameAttribute.GetNameOrDefault(eventType);

                await OnAddToOutboxAsync(eventName, eventType, eventData);

                var outgoingEventInfo = new OutgoingEventInfo(
                    GuidGenerator.Create(),
                    eventName,
                    Serialize(eventData),
                    Clock.Now
                );
                outgoingEventInfo.SetCorrelationId(CorrelationIdProvider.Get()!);
                await eventOutbox.EnqueueAsync(outgoingEventInfo);
                return true;
            }
        }

        return false;
    }

    protected virtual Task OnAddToOutboxAsync(string eventName, Type eventType, object eventData)
    {
        return Task.CompletedTask;
    }

    protected async Task<bool> AddToInboxAsync(
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
                            continue;
                        }
                    }

                    var incomingEventInfo = new IncomingEventInfo(
                        GuidGenerator.Create(),
                        messageId!,
                        eventName,
                        Serialize(eventData),
                        Clock.Now
                    );
                    incomingEventInfo.SetCorrelationId(correlationId!);
                    await eventInbox.EnqueueAsync(incomingEventInfo);
                }
            }
        }

        return true;
    }

    protected abstract byte[] Serialize(object eventData);

    protected virtual async Task TriggerHandlersDirectAsync(Type eventType, object eventData)
    {
        await TriggerDistributedEventReceivedAsync(new DistributedEventReceived
        {
            Source = DistributedEventSource.Direct,
            EventName = EventNameAttribute.GetNameOrDefault(eventType),
            EventData = eventData
        });

        await TriggerHandlersAsync(eventType, eventData);
    }

    protected virtual async Task TriggerHandlersFromInboxAsync(Type eventType, object eventData, List<Exception> exceptions, InboxConfig? inboxConfig = null)
    {
        await TriggerDistributedEventReceivedAsync(new DistributedEventReceived
        {
            Source = DistributedEventSource.Inbox,
            EventName = EventNameAttribute.GetNameOrDefault(eventType),
            EventData = eventData
        });

        await TriggerHandlersAsync(eventType, eventData, exceptions, inboxConfig);
    }

    public virtual async Task TriggerDistributedEventSentAsync(DistributedEventSent distributedEvent)
    {
        try
        {
            await LocalEventBus.PublishAsync(distributedEvent);
        }
        catch (Exception _)
        {
            // ignored
        }
    }

    public virtual async Task TriggerDistributedEventReceivedAsync(DistributedEventReceived distributedEvent)
    {
        try
        {
            await LocalEventBus.PublishAsync(distributedEvent);
        }
        catch (Exception _)
        {
            // ignored
        }
    }
}
