using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Entities.Events.Distributed;

public abstract class EntitySynchronizer<TEntity, TKey, TSourceEntityEto> :
    EntitySynchronizer<TEntity, TSourceEntityEto>
    where TEntity : class, IEntity<TKey>
    where TSourceEntityEto : IEntityEto<TKey> 
{
    protected new IRepository<TEntity, TKey> Repository { get; }

    protected EntitySynchronizer(IObjectMapper objectMapper, IRepository<TEntity, TKey> repository) :
        base(objectMapper, repository)
    {
        Repository = repository;
    }

    protected override Task<TEntity> FindLocalEntityAsync(TSourceEntityEto eto)
    {
        return Repository.FindAsync(eto.Id);
    }
}

public abstract class EntitySynchronizer<TEntity, TSourceEntityEto> :
    IDistributedEventHandler<EntityCreatedEto<TSourceEntityEto>>,
    IDistributedEventHandler<EntityUpdatedEto<TSourceEntityEto>>,
    IDistributedEventHandler<EntityDeletedEto<TSourceEntityEto>>,
    ITransientDependency
    where TEntity : class, IEntity
{
    protected IObjectMapper ObjectMapper { get; }
    protected IRepository<TEntity> Repository { get; }

    protected bool IgnoreEntityCreatedEvent { get; set; }
    protected bool IgnoreEntityUpdatedEvent { get; set; }
    protected bool IgnoreEntityDeletedEvent { get; set; }

    public EntitySynchronizer(
        IObjectMapper objectMapper,
        IRepository<TEntity> repository)
    {
        ObjectMapper = objectMapper;
        Repository = repository;
    }

    public virtual async Task HandleEventAsync(EntityCreatedEto<TSourceEntityEto> eventData)
    {
        if (IgnoreEntityCreatedEvent)
        {
            return;
        }

        await TryCreateOrUpdateEntityAsync(eventData.Entity);
    }

    public virtual async Task HandleEventAsync(EntityUpdatedEto<TSourceEntityEto> eventData)
    {
        if (IgnoreEntityUpdatedEvent)
        {
            return;
        }

        await TryCreateOrUpdateEntityAsync(eventData.Entity);
    }

    public virtual async Task HandleEventAsync(EntityDeletedEto<TSourceEntityEto> eventData)
    {
        if (IgnoreEntityDeletedEvent)
        {
            return;
        }

        await TryDeleteEntityAsync(eventData.Entity);
    }

    [UnitOfWork]
    protected virtual async Task<bool> TryCreateOrUpdateEntityAsync(TSourceEntityEto eto)
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
                ObjectHelper.TrySetProperty(
                    versionedLocalEntity,
                    x => x.EntityVersion,
                    () => versionedEto.EntityVersion
                );
            }

            await Repository.InsertAsync(localEntity);
        }
        else
        {
            await MapToEntityAsync(eto, localEntity);

            if (localEntity is IHasEntityVersion versionedLocalEntity && eto is IHasEntityVersion versionedEto)
            {
                /* The version will auto-increment by one when the repository updates the entity.
                 * So, we are decreasing it as a workaround here.
                 */
                var entityVersion = versionedEto.EntityVersion - 1;
                ObjectHelper.TrySetProperty(
                    versionedLocalEntity,
                    x => x.EntityVersion,
                    () => entityVersion
                );
            }

            await Repository.UpdateAsync(localEntity);
        }

        return true;
    }

    protected virtual Task<TEntity> MapToEntityAsync(TSourceEntityEto eto)
    {
        return Task.FromResult(ObjectMapper.Map<TSourceEntityEto, TEntity>(eto));
    }

    protected virtual Task MapToEntityAsync(TSourceEntityEto eto, TEntity localEntity)
    {
        ObjectMapper.Map(eto, localEntity);
        return Task.CompletedTask;
    }

    [UnitOfWork]
    protected virtual async Task<bool> TryDeleteEntityAsync(TSourceEntityEto eto)
    {
        var localEntity = await FindLocalEntityAsync(eto);

        if (localEntity == null)
        {
            return false;
        }

        await Repository.DeleteAsync(localEntity, true);

        return true;
    }

    [ItemCanBeNull]
    protected abstract Task<TEntity> FindLocalEntityAsync(TSourceEntityEto eto);

    protected virtual Task<bool> IsEtoNewerAsync(TSourceEntityEto eto, [CanBeNull] TEntity localEntity)
    {
        if (localEntity is IHasEntityVersion versionedLocalEntity && eto is IHasEntityVersion versionedEto)
        {
            /* We are also accepting the same version because
             * the entity may be updated, but the version might not be changed.
             */
            return Task.FromResult(versionedEto.EntityVersion >= versionedLocalEntity.EntityVersion);
        }

        return Task.FromResult(true);
    }
}