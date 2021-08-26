using System;
using System.Collections.Generic;
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
        private const string UnitOfWorkEventRecordEntityPropName = "_Abp_Entity";
        
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
            await TriggerEntityChangeEvents(changeReport.ChangedEntities);
            await TriggerLocalEvents(changeReport.DomainEvents);
            await TriggerDistributedEvents(changeReport.DistributedEvents);
        }

        public virtual async Task TriggerEntityCreatingEventAsync(object entity)
        {
            await TriggerEventWithEntity(
                LocalEventBus,
                typeof(EntityCreatingEventData<>),
                entity,
                entity
            );
        }

        public virtual async Task TriggerEntityCreatedEventAsync(object entity)
        {
            await TriggerEventWithEntity(
                LocalEventBus,
                typeof(EntityCreatedEventData<>),
                entity,
                entity
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
                        entity
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
                entity
            );
        }

        public virtual async Task TriggerEntityUpdatedEventAsync(object entity)
        {
            await TriggerEventWithEntity(
                LocalEventBus,
                typeof(EntityUpdatedEventData<>),
                entity,
                entity
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
                        entity
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
                entity
            );
        }

        public virtual async Task TriggerEntityDeletedEventAsync(object entity)
        {
            await TriggerEventWithEntity(
                LocalEventBus,
                typeof(EntityDeletedEventData<>),
                entity,
                entity
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
                        entity
                    );
                }
            }
        }

        protected virtual async Task TriggerEntityChangeEvents(List<EntityChangeEntry> changedEntities)
        {
            foreach (var changedEntity in changedEntities)
            {
                switch (changedEntity.ChangeType)
                {
                    case EntityChangeType.Created:
                        await TriggerEntityCreatingEventAsync(changedEntity.Entity);
                        await TriggerEntityCreatedEventAsync(changedEntity.Entity);
                        break;
                    case EntityChangeType.Updated:
                        await TriggerEntityUpdatingEventAsync(changedEntity.Entity);
                        await TriggerEntityUpdatedEventAsync(changedEntity.Entity);
                        break;
                    case EntityChangeType.Deleted:
                        await TriggerEntityDeletingEventAsync(changedEntity.Entity);
                        await TriggerEntityDeletedEventAsync(changedEntity.Entity);
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
            object originalEntity)
        {
            var entityType = ProxyHelper.UnProxy(entityOrEto).GetType();
            var eventType = genericEventType.MakeGenericType(entityType);
            var eventData = Activator.CreateInstance(eventType, entityOrEto);
            var currentUow = UnitOfWorkManager.Current;
            
            if (currentUow == null)
            {
                await eventPublisher.PublishAsync(
                    eventType,
                    eventData,
                    onUnitOfWorkComplete: false
                );

                return;
            }
            
            var eventRecord = new UnitOfWorkEventRecord(eventType, eventData)
            {
                Properties =
                {
                    { UnitOfWorkEventRecordEntityPropName, originalEntity },
                }
            };
            
            /* We are trying to eliminate same events for the same entity.
             * In this way, for example, we don't trigger update event for an entity multiple times
             * even if it is updated multiple times in the current UOW.
             */

            if (eventPublisher == DistributedEventBus)
            {
                currentUow.AddOrReplaceDistributedEvent(
                    eventRecord,
                    otherRecord => IsSameEntityEventRecord(eventRecord, otherRecord)
                );
            }
            else
            {
                currentUow.AddOrReplaceLocalEvent(
                    eventRecord,
                    otherRecord => IsSameEntityEventRecord(eventRecord, otherRecord)
                );
            }
        }

        public bool IsSameEntityEventRecord(UnitOfWorkEventRecord record1, UnitOfWorkEventRecord record2)
        {
            if (record1.EventType != record2.EventType)
            {
                return false;
            }

            var record1OriginalEntity = record1.Properties.GetOrDefault(UnitOfWorkEventRecordEntityPropName) as IEntity;
            var record2OriginalEntity = record2.Properties.GetOrDefault(UnitOfWorkEventRecordEntityPropName) as IEntity;

            if (record1OriginalEntity == null || record2OriginalEntity == null)
            {
                return false;
            }

            return EntityHelper.EntityEquals(record1OriginalEntity, record2OriginalEntity);
        }
    }
}