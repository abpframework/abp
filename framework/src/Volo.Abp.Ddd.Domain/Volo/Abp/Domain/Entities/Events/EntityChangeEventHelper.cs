using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Nito.Disposables.Internals;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.DynamicProxy;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Entities.Events;

/// <summary>
/// Used to trigger entity change events.
/// </summary>
public class EntityChangeEventHelper : IEntityChangeEventHelper, IBulkEntityChangeEventHelper, ITransientDependency
{
    private const string UnitOfWorkEventRecordEntityPropName = "_Abp_Entity";
    private const string UnitOfWorkEventRecordEntitiesPropName = "_Abp_Entities";

    private ILogger<EntityChangeEventHelper> Logger { get; set; }
    private ILocalEventBus LocalEventBus { get; set; }
    private IDistributedEventBus DistributedEventBus { get; set; }

    private IUnitOfWorkManager UnitOfWorkManager { get; }
    private IEntityToEtoMapper EntityToEtoMapper { get; }
    private AbpDistributedEntityEventOptions DistributedEntityEventOptions { get; }

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


    #region Bulk

    protected virtual void TriggerBulkEventWithEntities(
        IEventBus eventPublisher,
        Type genericEventType,
        List<object> entitiesOrEto,
        List<object> originalEntities)
    {
        var firstEntity = entitiesOrEto.FirstOrDefault();
        if (firstEntity == null)
        {
            throw new ArgumentOutOfRangeException(nameof(entitiesOrEto), "Can't publish a bulk event with no entities");
        }

        var entityType = ProxyHelper.UnProxy(firstEntity).GetType();
        var eventType = genericEventType.MakeGenericType(entityType);
        var eventData = Activator.CreateInstance(eventType, entitiesOrEto);
        var currentUow = UnitOfWorkManager.Current;
        if (currentUow == null)
        {
            Logger.LogWarning("UnitOfWorkManager.Current is null! Can not publish the event");
            return;
        }

        var eventRecord = new UnitOfWorkEventRecord(eventType, eventData, EventOrderGenerator.GetNext()) {
            Properties = { { UnitOfWorkEventRecordEntitiesPropName, originalEntities }, }
        };
        if (eventPublisher == DistributedEventBus)
        {
            currentUow.AddOrReplaceDistributedEvent(
                eventRecord,
                otherRecord => IsSameBulkEntityEventRecord(eventRecord, otherRecord)
            );
        }
        else
        {
            currentUow.AddOrReplaceLocalEvent(
                eventRecord,
                otherRecord => IsSameBulkEntityEventRecord(eventRecord, otherRecord)
            );
        }
    }

    public void PublishBulkEntityCreatedEvent(List<object> entities)
    {
        TriggerBulkEventWithEntities(
            LocalEventBus,
            typeof(BulkEntityCreatedEventData<>),
            entities,
            entities
        );

        var entitiesToPublish = entities.Where(ShouldPublishDistributedEventForEntity).ToList();
        if (entitiesToPublish.Count <= 0)
        {
            return;
        }

        var eto = entitiesToPublish.Select(EntityToEtoMapper.Map).WhereNotNull().ToList();
        if (eto.Count <= 0)
        {
            return;
        }

        TriggerBulkEventWithEntities(
            DistributedEventBus,
            typeof(BulkEntityCreatedEto<>),
            eto,
            entitiesToPublish
        );
    }

    public void PublishBulkEntityUpdatedEvent(List<object> entities)
    {
        TriggerBulkEventWithEntities(
            LocalEventBus,
            typeof(BulkEntityUpdatedEventData<>),
            entities,
            entities
        );

        var entitiesToPublish = entities.Where(ShouldPublishDistributedEventForEntity).ToList();
        if (entitiesToPublish.Count == 0)
        {
            return;
        }

        var eto = entitiesToPublish.Select(EntityToEtoMapper.Map).WhereNotNull().ToList();
        if (eto.Count == 0)
        {
            return;
        }

        TriggerBulkEventWithEntities(
            DistributedEventBus,
            typeof(BulkEntityUpdatedEto<>),
            eto,
            entitiesToPublish
        );
    }

    public void PublishBulkEntityDeletedEvent(List<object> entities)
    {
        TriggerBulkEventWithEntities(
            LocalEventBus,
            typeof(BulkEntityDeletedEventData<>),
            entities,
            entities
        );

        var entitiesToPublish = entities.Where(ShouldPublishDistributedEventForEntity).ToList();
        if (entitiesToPublish.Count == 0)
        {
            return;
        }

        var eto = entitiesToPublish.Select(EntityToEtoMapper.Map).WhereNotNull().ToList();
        if (eto.Count == 0)
        {
            return;
        }

        TriggerBulkEventWithEntities(
            DistributedEventBus,
            typeof(BulkEntityDeletedEto<>),
            eto,
            entitiesToPublish
        );
    }

    #endregion

    #region Single

    protected virtual void TriggerEventWithEntity(
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
            Logger.LogWarning("UnitOfWorkManager.Current is null! Can not publish the event");
            return;
        }

        var eventRecord = new UnitOfWorkEventRecord(eventType, eventData, EventOrderGenerator.GetNext()) {
            Properties = { { UnitOfWorkEventRecordEntityPropName, originalEntity }, }
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


    public virtual void PublishEntityCreatedEvent(object entity)
    {
        TriggerEventWithEntity(
            LocalEventBus,
            typeof(EntityCreatedEventData<>),
            entity,
            entity
        );

        if (!ShouldPublishDistributedEventForEntity(entity))
        {
            return;
        }

        var eto = EntityToEtoMapper.Map(entity);
        if (eto == null)
        {
            return;
        }

        TriggerEventWithEntity(
            DistributedEventBus,
            typeof(EntityCreatedEto<>),
            eto,
            entity
        );
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

    public virtual void PublishEntityUpdatedEvent(object entity)
    {
        TriggerEventWithEntity(
            LocalEventBus,
            typeof(EntityUpdatedEventData<>),
            entity,
            entity
        );

        if (!ShouldPublishDistributedEventForEntity(entity))
        {
            return;
        }

        var eto = EntityToEtoMapper.Map(entity);
        if (eto == null)
        {
            return;
        }

        TriggerEventWithEntity(
            DistributedEventBus,
            typeof(EntityUpdatedEto<>),
            eto,
            entity
        );
    }

    public virtual void PublishEntityDeletedEvent(object entity)
    {
        TriggerEventWithEntity(
            LocalEventBus,
            typeof(EntityDeletedEventData<>),
            entity,
            entity
        );

        if (!ShouldPublishDistributedEventForEntity(entity))
        {
            return;
        }

        var eto = EntityToEtoMapper.Map(entity);
        if (eto == null)
        {
            return;
        }

        TriggerEventWithEntity(
            DistributedEventBus,
            typeof(EntityDeletedEto<>),
            eto,
            entity
        );
    }

    private bool IsSameEntityEventRecord(UnitOfWorkEventRecord record1, UnitOfWorkEventRecord record2)
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

    private bool IsSameBulkEntityEventRecord(UnitOfWorkEventRecord record1, UnitOfWorkEventRecord record2)
    {
        if (record1.EventType != record2.EventType)
        {
            return false;
        }

        var record1OriginalEntity =
            (record1.Properties.GetOrDefault(UnitOfWorkEventRecordEntitiesPropName) as List<object>)?.OfType<IEntity>()
            .ToList();
        var record2OriginalEntity =
            (record2.Properties.GetOrDefault(UnitOfWorkEventRecordEntitiesPropName) as List<object>)?.OfType<IEntity>()
            .ToList();

        if (record1OriginalEntity == null || record1OriginalEntity.Count == 0 || record2OriginalEntity == null ||
            record2OriginalEntity.Count == 0 || record1OriginalEntity.Count != record2OriginalEntity.Count)
        {
            return false;
        }

        var i = 0;
        return record1OriginalEntity.All(x =>
            EntityHelper.EntityEquals(x, record2OriginalEntity[i++]));
    }

    #endregion
}