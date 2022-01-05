using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;

namespace Volo.Abp.EntityFrameworkCore.EntityHistory;

public class EntityHistoryHelper : IEntityHistoryHelper, ITransientDependency
{
    public ILogger<EntityHistoryHelper> Logger { get; set; }

    protected IAuditingStore AuditingStore { get; }
    protected IJsonSerializer JsonSerializer { get; }
    protected AbpAuditingOptions Options { get; }
    protected IAuditingHelper AuditingHelper { get; }
    protected IClock Clock { get; }

    public EntityHistoryHelper(
        IAuditingStore auditingStore,
        IOptions<AbpAuditingOptions> options,
        IClock clock,
        IJsonSerializer jsonSerializer,
        IAuditingHelper auditingHelper)
    {
        Clock = clock;
        AuditingStore = auditingStore;
        JsonSerializer = jsonSerializer;
        AuditingHelper = auditingHelper;
        Options = options.Value;

        Logger = NullLogger<EntityHistoryHelper>.Instance;
    }

    public virtual List<EntityChangeInfo> CreateChangeList(ICollection<EntityEntry> entityEntries)
    {
        var list = new List<EntityChangeInfo>();

        foreach (var entry in entityEntries)
        {
            if (!ShouldSaveEntityHistory(entry))
            {
                continue;
            }

            var entityChange = CreateEntityChangeOrNull(entry);
            if (entityChange == null)
            {
                continue;
            }

            list.Add(entityChange);
        }

        return list;
    }

    [CanBeNull]
    protected virtual EntityChangeInfo CreateEntityChangeOrNull(EntityEntry entityEntry)
    {
        var entity = entityEntry.Entity;

        EntityChangeType changeType;
        switch (entityEntry.State)
        {
            case EntityState.Added:
                changeType = EntityChangeType.Created;
                break;
            case EntityState.Deleted:
                changeType = EntityChangeType.Deleted;
                break;
            case EntityState.Modified:
                changeType = IsDeleted(entityEntry) ? EntityChangeType.Deleted : EntityChangeType.Updated;
                break;
            case EntityState.Detached:
            case EntityState.Unchanged:
            default:
                return null;
        }

        var entityId = GetEntityId(entity);
        if (entityId == null && changeType != EntityChangeType.Created)
        {
            return null;
        }

        var entityType = entity.GetType();
        var entityChange = new EntityChangeInfo
        {
            ChangeType = changeType,
            EntityEntry = entityEntry,
            EntityId = entityId,
            EntityTypeFullName = entityType.FullName,
            PropertyChanges = GetPropertyChanges(entityEntry),
            EntityTenantId = GetTenantId(entity)
        };

        return entityChange;
    }

    protected virtual Guid? GetTenantId(object entity)
    {
        if (!(entity is IMultiTenant multiTenantEntity))
        {
            return null;
        }

        return multiTenantEntity.TenantId;
    }

    protected virtual DateTime GetChangeTime(EntityChangeInfo entityChange)
    {
        var entity = entityChange.EntityEntry.As<EntityEntry>().Entity;
        switch (entityChange.ChangeType)
        {
            case EntityChangeType.Created:
                return (entity as IHasCreationTime)?.CreationTime ?? Clock.Now;
            case EntityChangeType.Deleted:
                return (entity as IHasDeletionTime)?.DeletionTime ?? Clock.Now;
            case EntityChangeType.Updated:
                return (entity as IHasModificationTime)?.LastModificationTime ?? Clock.Now;
            default:
                throw new AbpException($"Unknown {nameof(EntityChangeInfo)}: {entityChange}");
        }
    }

    protected virtual string GetEntityId(object entityAsObj)
    {
        if (!(entityAsObj is IEntity entity))
        {
            throw new AbpException($"Entities should implement the {typeof(IEntity).AssemblyQualifiedName} interface! Given entity does not implement it: {entityAsObj.GetType().AssemblyQualifiedName}");
        }

        var keys = entity.GetKeys();
        if (keys.All(k => k == null))
        {
            return null;
        }

        return keys.JoinAsString(",");
    }

    /// <summary>
    /// Gets the property changes for this entry.
    /// </summary>
    protected virtual List<EntityPropertyChangeInfo> GetPropertyChanges(EntityEntry entityEntry)
    {
        var propertyChanges = new List<EntityPropertyChangeInfo>();
        var properties = entityEntry.Metadata.GetProperties();
        var isCreated = IsCreated(entityEntry);
        var isDeleted = IsDeleted(entityEntry);

        foreach (var property in properties)
        {
            var propertyEntry = entityEntry.Property(property.Name);
            if (ShouldSavePropertyHistory(propertyEntry, isCreated || isDeleted) && !IsSoftDeleted(entityEntry))
            {
                propertyChanges.Add(new EntityPropertyChangeInfo
                {
                    NewValue = isDeleted ? null : JsonSerializer.Serialize(propertyEntry.CurrentValue).TruncateWithPostfix(EntityPropertyChangeInfo.MaxValueLength),
                    OriginalValue = isCreated ? null : JsonSerializer.Serialize(propertyEntry.OriginalValue).TruncateWithPostfix(EntityPropertyChangeInfo.MaxValueLength),
                    PropertyName = property.Name,
                    PropertyTypeFullName = property.ClrType.GetFirstGenericArgumentIfNullable().FullName
                });
            }
        }

        return propertyChanges;
    }

    protected virtual bool IsCreated(EntityEntry entityEntry)
    {
        return entityEntry.State == EntityState.Added;
    }

    protected virtual bool IsDeleted(EntityEntry entityEntry)
    {
        return entityEntry.State == EntityState.Deleted || IsSoftDeleted(entityEntry);
    }

    protected virtual bool IsSoftDeleted(EntityEntry entityEntry)
    {
        var entity = entityEntry.Entity;
        return entity is ISoftDelete && entity.As<ISoftDelete>().IsDeleted;
    }

    protected virtual bool ShouldSaveEntityHistory(EntityEntry entityEntry, bool defaultValue = false)
    {
        if (entityEntry.State == EntityState.Detached ||
            entityEntry.State == EntityState.Unchanged)
        {
            return false;
        }

        var entityType = entityEntry.Metadata.ClrType;

        if (!EntityHelper.IsEntity(entityType))
        {
            return false;
        }

        if (AuditingHelper.IsEntityHistoryEnabled(entityType))
        {
            return true;
        }

        return defaultValue;
    }

    protected virtual bool ShouldSavePropertyHistory(PropertyEntry propertyEntry, bool defaultValue)
    {
        if (propertyEntry.Metadata.Name == "Id")
        {
            return false;
        }

        var propertyInfo = propertyEntry.Metadata.PropertyInfo;
        if (propertyInfo != null && propertyInfo.IsDefined(typeof(DisableAuditingAttribute), true))
        {
            return false;
        }

        var entityType = propertyEntry.EntityEntry.Entity.GetType();
        if (entityType.IsDefined(typeof(DisableAuditingAttribute), true))
        {
            if (propertyInfo == null || !propertyInfo.IsDefined(typeof(AuditedAttribute), true))
            {
                return false;
            }
        }

        if (propertyInfo != null && IsBaseAuditProperty(propertyInfo, entityType))
        {
            return false;
        }

        if (propertyEntry.OriginalValue is ExtraPropertyDictionary originalValue && propertyEntry.CurrentValue is ExtraPropertyDictionary currentValue)
        {
            if (originalValue.IsNullOrEmpty() && currentValue.IsNullOrEmpty())
            {
                return false;
            }

            if (!originalValue.Select(x => x.Key).SequenceEqual(currentValue.Select(x => x.Key)))
            {
                return true;
            }

            if (!originalValue.Select(x => x.Value).SequenceEqual(currentValue.Select(x => x.Value)))
            {
                return true;
            }

            return defaultValue;
        }

        var isModified = !(propertyEntry.OriginalValue?.Equals(propertyEntry.CurrentValue) ?? propertyEntry.CurrentValue == null);
        if (isModified)
        {
            return true;
        }

        return defaultValue;
    }

    protected virtual bool IsBaseAuditProperty(PropertyInfo propertyInfo, Type entityType)
    {
        if (entityType.IsAssignableTo<IHasCreationTime>()
            && propertyInfo.Name == nameof(IHasCreationTime.CreationTime))
        {
            return true;
        }

        if (entityType.IsAssignableTo<IMayHaveCreator>()
            && propertyInfo.Name == nameof(IMayHaveCreator.CreatorId))
        {
            return true;
        }

        if (entityType.IsAssignableTo<IMustHaveCreator>()
            && propertyInfo.Name == nameof(IMustHaveCreator.CreatorId))
        {
            return true;
        }

        if (entityType.IsAssignableTo<IHasModificationTime>()
            && propertyInfo.Name == nameof(IHasModificationTime.LastModificationTime))
        {
            return true;
        }

        if (entityType.IsAssignableTo<IModificationAuditedObject>()
            && propertyInfo.Name == nameof(IModificationAuditedObject.LastModifierId))
        {
            return true;
        }

        if (entityType.IsAssignableTo<ISoftDelete>()
            && propertyInfo.Name == nameof(ISoftDelete.IsDeleted))
        {
            return true;
        }

        if (entityType.IsAssignableTo<IHasDeletionTime>()
            && propertyInfo.Name == nameof(IHasDeletionTime.DeletionTime))
        {
            return true;
        }

        if (entityType.IsAssignableTo<IDeletionAuditedObject>()
            && propertyInfo.Name == nameof(IDeletionAuditedObject.DeleterId))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Updates change time, entity id and foreign keys after SaveChanges is called.
    /// </summary>
    public virtual void UpdateChangeList(List<EntityChangeInfo> entityChanges)
    {
        foreach (var entityChange in entityChanges)
        {
            /* Update change time */

            entityChange.ChangeTime = GetChangeTime(entityChange);

            /* Update entity id */

            var entityEntry = entityChange.EntityEntry.As<EntityEntry>();
            entityChange.EntityId = GetEntityId(entityEntry.Entity);

            /* Update foreign keys */

            var foreignKeys = entityEntry.Metadata.GetForeignKeys();

            foreach (var foreignKey in foreignKeys)
            {
                foreach (var property in foreignKey.Properties)
                {
                    var propertyEntry = entityEntry.Property(property.Name);
                    var propertyChange = entityChange.PropertyChanges.FirstOrDefault(pc => pc.PropertyName == property.Name);

                    if (propertyChange == null)
                    {
                        if (!(propertyEntry.OriginalValue?.Equals(propertyEntry.CurrentValue) ?? propertyEntry.CurrentValue == null))
                        {
                            // Add foreign key
                            entityChange.PropertyChanges.Add(new EntityPropertyChangeInfo
                            {
                                NewValue = JsonSerializer.Serialize(propertyEntry.CurrentValue),
                                OriginalValue = JsonSerializer.Serialize(propertyEntry.OriginalValue),
                                PropertyName = property.Name,
                                PropertyTypeFullName = property.ClrType.GetFirstGenericArgumentIfNullable().FullName
                            });
                        }

                        continue;
                    }

                    if (propertyChange.OriginalValue == propertyChange.NewValue)
                    {
                        var newValue = JsonSerializer.Serialize(propertyEntry.CurrentValue);
                        if (newValue == propertyChange.NewValue)
                        {
                            // No change
                            entityChange.PropertyChanges.Remove(propertyChange);
                        }
                        else
                        {
                            // Update foreign key
                            propertyChange.NewValue = newValue.TruncateWithPostfix(EntityPropertyChangeInfo.MaxValueLength);
                        }
                    }
                }
            }
        }
    }
}
