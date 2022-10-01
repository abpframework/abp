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

public abstract class ExternalEntitySynchronizer<TEntity, TKey, TExternalEntityEto> :
    ExternalEntitySynchronizer<TEntity, TExternalEntityEto>
    where TEntity : class, IEntity<TKey>, IHasRemoteModificationTime
    where TExternalEntityEto : EntityEto, IHasModificationTime
{
    private readonly IRepository<TEntity, TKey> _repository;

    protected ExternalEntitySynchronizer(IObjectMapper objectMapper, IRepository<TEntity, TKey> repository) :
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

public abstract class ExternalEntitySynchronizer<TEntity, TExternalEntityEto> :
    IDistributedEventHandler<EntityCreatedEto<TExternalEntityEto>>,
    IDistributedEventHandler<EntityUpdatedEto<TExternalEntityEto>>,
    IDistributedEventHandler<EntityDeletedEto<TExternalEntityEto>>,
    IUnitOfWorkEnabled
    where TEntity : class, IEntity, IHasRemoteModificationTime
    where TExternalEntityEto : EntityEto, IHasModificationTime
{
    protected IObjectMapper ObjectMapper { get; }
    private readonly IRepository<TEntity> _repository;

    protected virtual bool IgnoreEntityCreatedEvent { get; set; }
    protected virtual bool IgnoreEntityUpdatedEvent { get; set; }
    protected virtual bool IgnoreEntityDeletedEvent { get; set; }

    public ExternalEntitySynchronizer(
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

        await CreateOrUpdateEntityAsync(eventData.Entity);
    }

    public virtual async Task HandleEventAsync(EntityUpdatedEto<TExternalEntityEto> eventData)
    {
        if (IgnoreEntityUpdatedEvent)
        {
            return;
        }

        await CreateOrUpdateEntityAsync(eventData.Entity);
    }

    public virtual async Task HandleEventAsync(EntityDeletedEto<TExternalEntityEto> eventData)
    {
        if (IgnoreEntityDeletedEvent)
        {
            return;
        }

        await TryDeleteEntityAsync(eventData.Entity);
    }

    protected virtual async Task CreateOrUpdateEntityAsync(TExternalEntityEto eto)
    {
        var localEntity = await FindLocalEntityAsync(eto);

        if (!await IsEtoNewerAsync(eto, localEntity))
        {
            return;
        }

        if (localEntity == null)
        {
            localEntity = await MapToEntityAsync(eto);
            ObjectHelper.TrySetProperty(localEntity, x => x.RemoteLastModificationTime, () => eto.LastModificationTime);

            await _repository.InsertAsync(localEntity, true);
        }
        else
        {
            await MapToEntityAsync(eto, localEntity);
            ObjectHelper.TrySetProperty(localEntity, x => x.RemoteLastModificationTime, () => eto.LastModificationTime);

            await _repository.UpdateAsync(localEntity, true);
        }
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

    protected virtual async Task TryDeleteEntityAsync(TExternalEntityEto eto)
    {
        var localEntity = await FindLocalEntityAsync(eto);

        if (localEntity == null)
        {
            return;
        }

        await _repository.DeleteAsync(localEntity, true);
    }

    [ItemCanBeNull]
    protected abstract Task<TEntity> FindLocalEntityAsync(TExternalEntityEto eto);

    protected virtual Task<bool> IsEtoNewerAsync(TExternalEntityEto eto, [CanBeNull] TEntity localEntity)
    {
        return Task.FromResult(
            localEntity?.RemoteLastModificationTime == null ||
            eto.LastModificationTime > localEntity.RemoteLastModificationTime
        );
    }
}