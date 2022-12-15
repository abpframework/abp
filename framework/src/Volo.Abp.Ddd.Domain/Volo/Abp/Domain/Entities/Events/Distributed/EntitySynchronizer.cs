using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Entities.Events.Distributed;

public abstract class EntitySynchronizer<TEntity, TKey, TExternalEntityEto> :
    EntitySynchronizer<TEntity, TExternalEntityEto>
    where TEntity : class, IEntity<TKey>
    where TExternalEntityEto : EntityEto
{
    private readonly IRepository<TEntity, TKey> _repository;

    protected EntitySynchronizer(IObjectMapper objectMapper, IRepository<TEntity, TKey> repository) :
        base(objectMapper, repository)
    {
        _repository = repository;
    }

    protected override Task<TEntity> FindLocalEntityAsync(TExternalEntityEto eto)
    {
        return _repository.FindAsync(GetExternalEntityId(eto));
    }

    protected virtual TKey GetExternalEntityId(TExternalEntityEto eto)
    {
        var keyType = typeof(TKey);
        var keyValue = Check.NotNullOrEmpty(eto.KeysAsString, nameof(eto.KeysAsString));

        if (keyType == typeof(Guid))
        {
            return (TKey)TypeDescriptor.GetConverter(keyType).ConvertFromInvariantString(keyValue);
        }

        return (TKey)Convert.ChangeType(keyValue, keyType, CultureInfo.InvariantCulture);
    }
}

public abstract class EntitySynchronizer<TEntity, TExternalEntityEto> :
    IDistributedEventHandler<EntityCreatedEto<TExternalEntityEto>>,
    IDistributedEventHandler<EntityUpdatedEto<TExternalEntityEto>>,
    IDistributedEventHandler<EntityDeletedEto<TExternalEntityEto>>,
    IUnitOfWorkEnabled
    where TEntity : class, IEntity
    where TExternalEntityEto : EntityEto
{
    protected IObjectMapper ObjectMapper { get; }
    private readonly IRepository<TEntity> _repository;

    protected virtual bool IgnoreEntityCreatedEvent { get; set; }
    protected virtual bool IgnoreEntityUpdatedEvent { get; set; }
    protected virtual bool IgnoreEntityDeletedEvent { get; set; }

    public EntitySynchronizer(
        IObjectMapper objectMapper,
        IRepository<TEntity> repository)
    {
        ObjectMapper = objectMapper;
        _repository = repository;
    }

    public virtual async Task HandleEventAsync(EntityCreatedEto<TExternalEntityEto> eventData)
    {
        if (IgnoreEntityCreatedEvent)
        {
            return;
        }

        await TryCreateOrUpdateEntityAsync(eventData.Entity);
    }

    public virtual async Task HandleEventAsync(EntityUpdatedEto<TExternalEntityEto> eventData)
    {
        if (IgnoreEntityUpdatedEvent)
        {
            return;
        }

        await TryCreateOrUpdateEntityAsync(eventData.Entity);
    }

    public virtual async Task HandleEventAsync(EntityDeletedEto<TExternalEntityEto> eventData)
    {
        if (IgnoreEntityDeletedEvent)
        {
            return;
        }

        await TryDeleteEntityAsync(eventData.Entity);
    }

    protected virtual async Task<bool> TryCreateOrUpdateEntityAsync(TExternalEntityEto eto)
    {
        var localEntity = await FindLocalEntityAsync(eto);

        if (!await IsEtoNewerAsync(eto, localEntity))
        {
            return false;
        }

        if (localEntity == null)
        {
            localEntity = await MapToEntityAsync(eto);

            if (localEntity is IHasEntityVersion versionedLocalEntity && eto is IHasEntityVersion versionedEto)
            {
                ObjectHelper.TrySetProperty(versionedLocalEntity, x => x.EntityVersion,
                    () => versionedEto.EntityVersion);
            }

            await _repository.InsertAsync(localEntity, true);
        }
        else
        {
            await MapToEntityAsync(eto, localEntity);

            if (localEntity is IHasEntityVersion versionedLocalEntity && eto is IHasEntityVersion versionedEto)
            {
                // The version will auto-increment by one when the repository updates the entity.
                var entityVersion = versionedEto.EntityVersion - 1;

                ObjectHelper.TrySetProperty(versionedLocalEntity, x => x.EntityVersion, () => entityVersion);
            }

            await _repository.UpdateAsync(localEntity, true);
        }

        return true;
    }

    protected virtual Task<TEntity> MapToEntityAsync(TExternalEntityEto eto)
    {
        return Task.FromResult(ObjectMapper.Map<TExternalEntityEto, TEntity>(eto));
    }

    protected virtual Task MapToEntityAsync(TExternalEntityEto eto, TEntity localEntity)
    {
        ObjectMapper.Map(eto, localEntity);
        return Task.CompletedTask;
    }

    protected virtual async Task<bool> TryDeleteEntityAsync(TExternalEntityEto eto)
    {
        var localEntity = await FindLocalEntityAsync(eto);

        if (localEntity == null)
        {
            return false;
        }

        await _repository.DeleteAsync(localEntity, true);

        return true;
    }

    [ItemCanBeNull]
    protected abstract Task<TEntity> FindLocalEntityAsync(TExternalEntityEto eto);

    protected virtual Task<bool> IsEtoNewerAsync(TExternalEntityEto eto, [CanBeNull] TEntity localEntity)
    {
        if (localEntity is IHasEntityVersion versionedLocalEntity && eto is IHasEntityVersion versionedEto)
        {
            return Task.FromResult(versionedEto.EntityVersion > versionedLocalEntity.EntityVersion);
        }

        return Task.FromResult(true);
    }
}