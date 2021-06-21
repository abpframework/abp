using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.DynamicProxy;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Entities.Events
{
    /// <summary>
    /// Used to trigger entity change events.
    /// </summary>
    public class EntityChangeEventHelper : IEntityChangeEventHelper, ITransientDependency
    {
        public ILogger<EntityChangeEventHelper> Logger { get; set; }
        public ILocalEventBus LocalEventBus { get; set; }
        public IDistributedEventBus DistributedEventBus { get; set; }

        protected IUnitOfWorkManager UnitOfWorkManager { get; }
        protected IEntityToEtoMapper EntityToEtoMapper { get; }
        protected AbpDistributedEntityEventOptions DistributedEntityEventOptions { get; }

        public EntityChangeEventHelper(
            IUnitOfWorkManager unitOfWorkManager,
            IEntityToEtoMapper entityToEtoMapper,
            IOptions<AbpDistributedEntityEventOptions> distributedEntityEventOptions)
        {
            UnitOfWorkManager = unitOfWorkManager;
            EntityToEtoMapper = entityToEtoMapper;
            DistributedEntityEventOptions = distributedEntityEventOptions.Value;

            LocalEventBus = NullLocalEventBus.Instance;
            DistributedEventBus = NullDistributedEventBus.Instance;
            Logger = NullLogger<EntityChangeEventHelper>.Instance;
        }

        public async Task TriggerEventsAsync(EntityChangeReport changeReport)
        {
            await TriggerEventsInternalAsync(changeReport);

            if (changeReport.IsEmpty() || UnitOfWorkManager.Current == null)
            {
                return;
            }

            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        public virtual async Task TriggerEntityCreatingEventAsync(object entity)
        {
            await TriggerEventWithEntity(
                LocalEventBus,
                typeof(EntityCreatingEventData<>),
                entity,
                entity,
                true
            );
        }

        public virtual async Task TriggerEntityCreatedEventOnUowCompletedAsync(object entity)
        {
            await TriggerEventWithEntity(
                LocalEventBus,
                typeof(EntityCreatedEventData<>),
                entity,
                entity,
                false
            );

            if (ShouldPublishDistributedEventForEntity(entity))
            {
                var eto = EntityToEtoMapper.Map(entity);
                if (eto != null)
                {
                    await TriggerEventWithEntity(
                        DistributedEventBus,
                        typeof(EntityCreatedEto<>),
                        eto,
                        entity,
                        false
                    );
                }
            }
        }

        private bool ShouldPublishDistributedEventForEntity(object entity)
        {
            return DistributedEntityEventOptions
                .AutoEventSelectors
                .IsMatch(
                    ProxyHelper
                        .UnProxy(entity)
                        .GetType()
                );
        }

        public virtual async Task TriggerEntityUpdatingEventAsync(object entity)
        {
            await TriggerEventWithEntity(
                LocalEventBus,
                typeof(EntityUpdatingEventData<>),
                entity,
                entity,
                true
            );
        }

        public virtual async Task TriggerEntityUpdatedEventOnUowCompletedAsync(object entity)
        {
            await TriggerEventWithEntity(
                LocalEventBus,
                typeof(EntityUpdatedEventData<>),
                entity,
                entity,
                false
            );

            if (ShouldPublishDistributedEventForEntity(entity))
            {
                var eto = EntityToEtoMapper.Map(entity);
                if (eto != null)
                {
                    await TriggerEventWithEntity(
                        DistributedEventBus,
                        typeof(EntityUpdatedEto<>),
                        eto,
                        entity,
                        false
                    );
                }
            }
        }

        public virtual async Task TriggerEntityDeletingEventAsync(object entity)
        {
            await TriggerEventWithEntity(
                LocalEventBus,
                typeof(EntityDeletingEventData<>),
                entity,
                entity,
                true
            );
        }

        public virtual async Task TriggerEntityDeletedEventOnUowCompletedAsync(object entity)
        {
            await TriggerEventWithEntity(
                LocalEventBus,
                typeof(EntityDeletedEventData<>),
                entity,
                entity,
                false
            );

            if (ShouldPublishDistributedEventForEntity(entity))
            {
                var eto = EntityToEtoMapper.Map(entity);
                if (eto != null)
                {
                    await TriggerEventWithEntity(
                        DistributedEventBus,
                        typeof(EntityDeletedEto<>),
                        eto,
                        entity,
                        false
                    );
                }
            }
        }

        protected virtual async Task TriggerEventsInternalAsync(EntityChangeReport changeReport)
        {
            await TriggerEntityChangeEvents(changeReport.ChangedEntities);
            await TriggerLocalEvents(changeReport.DomainEvents);
            await TriggerDistributedEvents(changeReport.DistributedEvents);
        }

        protected virtual async Task TriggerEntityChangeEvents(List<EntityChangeEntry> changedEntities)
        {
            foreach (var changedEntity in changedEntities)
            {
                switch (changedEntity.ChangeType)
                {
                    case EntityChangeType.Created:
                        await TriggerEntityCreatingEventAsync(changedEntity.Entity);
                        await TriggerEntityCreatedEventOnUowCompletedAsync(changedEntity.Entity);
                        break;
                    case EntityChangeType.Updated:
                        await TriggerEntityUpdatingEventAsync(changedEntity.Entity);
                        await TriggerEntityUpdatedEventOnUowCompletedAsync(changedEntity.Entity);
                        break;
                    case EntityChangeType.Deleted:
                        await TriggerEntityDeletingEventAsync(changedEntity.Entity);
                        await TriggerEntityDeletedEventOnUowCompletedAsync(changedEntity.Entity);
                        break;
                    default:
                        throw new AbpException("Unknown EntityChangeType: " + changedEntity.ChangeType);
                }
            }
        }

        protected virtual async Task TriggerLocalEvents(List<DomainEventEntry> localEvents)
        {
            foreach (var localEvent in localEvents)
            {
                await LocalEventBus.PublishAsync(localEvent.EventData.GetType(), localEvent.EventData);
            }
        }

        protected virtual async Task TriggerDistributedEvents(List<DomainEventEntry> distributedEvents)
        {
            foreach (var distributedEvent in distributedEvents)
            {
                await DistributedEventBus.PublishAsync(distributedEvent.EventData.GetType(),
                    distributedEvent.EventData);
            }
        }

        protected virtual async Task TriggerEventWithEntity(
            IEventBus eventPublisher,
            Type genericEventType,
            object entityOrEto,
            object originalEntity,
            bool triggerInCurrentUnitOfWork)
        {
            var entityType = ProxyHelper.UnProxy(entityOrEto).GetType();
            var eventType = genericEventType.MakeGenericType(entityType);
            var currentUow = UnitOfWorkManager.Current;

            if (triggerInCurrentUnitOfWork || currentUow == null)
            {
                await eventPublisher.PublishAsync(
                    eventType,
                    Activator.CreateInstance(eventType, entityOrEto)
                );

                return;
            }

            var eventList = GetEventList(currentUow);
            var isFirstEvent = !eventList.Any();

            eventList.AddUniqueEvent(eventPublisher, eventType, entityOrEto, originalEntity);

            /* Register to OnCompleted if this is the first item.
             * Other items will already be in the list once the UOW completes.
             */
            if (isFirstEvent)
            {
                currentUow.OnCompleted(
                    async () =>
                    {
                        foreach (var eventEntry in eventList)
                        {
                            try
                            {
                                await eventEntry.EventBus.PublishAsync(
                                    eventEntry.EventType,
                                    Activator.CreateInstance(eventEntry.EventType, eventEntry.EntityOrEto)
                                );
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(
                                    $"Caught an exception while publishing the event '{eventType.FullName}' for the entity '{entityOrEto}'");
                                Logger.LogException(ex);
                            }
                        }
                    }
                );
            }
        }

        private EntityChangeEventList GetEventList(IUnitOfWork currentUow)
        {
            return (EntityChangeEventList) currentUow.Items.GetOrAdd(
                "AbpEntityChangeEventList",
                () => new EntityChangeEventList()
            );
        }

        private class EntityChangeEventList : List<EntityChangeEventEntry>
        {
            public void AddUniqueEvent(IEventBus eventBus, Type eventType, object entityOrEto, object originalEntity)
            {
                var newEntry = new EntityChangeEventEntry(eventBus, eventType, entityOrEto, originalEntity);

                //Latest "same" event overrides the previous events.
                for (var i = 0; i < Count; i++)
                {
                    if (this[i].IsSameEvent(newEntry))
                    {
                        this[i] = newEntry;
                        return;
                    }
                }

                //If this is a "new" event, add to the end
                Add(newEntry);
            }
        }

        private class EntityChangeEventEntry
        {
            public IEventBus EventBus { get; }

            public Type EventType { get; }

            public object EntityOrEto { get; }

            public object OriginalEntity { get; }

            public EntityChangeEventEntry(IEventBus eventBus, Type eventType, object entityOrEto, object originalEntity)
            {
                EventType = eventType;
                EntityOrEto = entityOrEto;
                OriginalEntity = originalEntity;
                EventBus = eventBus;
            }

            public bool IsSameEvent(EntityChangeEventEntry otherEntry)
            {
                if (EventBus != otherEntry.EventBus || EventType != otherEntry.EventType)
                {
                    return false;
                }

                var originalEntityRef = OriginalEntity as IEntity;
                var otherOriginalEntityRef = otherEntry.OriginalEntity as IEntity;
                if (originalEntityRef == null || otherOriginalEntityRef == null)
                {
                    return false;
                }

                return EntityHelper.EntityEquals(originalEntityRef, otherOriginalEntityRef);
            }
        }
    }
}