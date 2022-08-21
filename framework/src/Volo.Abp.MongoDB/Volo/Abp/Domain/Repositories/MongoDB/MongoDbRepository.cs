using JetBrains.Annotations;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Repositories.MongoDB;

public class MongoDbRepository<TMongoDbContext, TEntity>
    : RepositoryBase<TEntity>,
    IMongoDbRepository<TEntity>
    where TMongoDbContext : IAbpMongoDbContext
    where TEntity : class, IEntity
{
    [Obsolete("Use GetCollectionAsync method.")]
    public virtual IMongoCollection<TEntity> Collection => DbContext.Collection<TEntity>();

    public async Task<IMongoCollection<TEntity>> GetCollectionAsync(CancellationToken cancellationToken = default)
    {
        return (await GetDbContextAsync(GetCancellationToken(cancellationToken))).Collection<TEntity>();
    }

    [Obsolete("Use GetDatabaseAsync method.")]
    public virtual IMongoDatabase Database => DbContext.Database;

    public async Task<IMongoDatabase> GetDatabaseAsync(CancellationToken cancellationToken = default)
    {
        return (await GetDbContextAsync(GetCancellationToken(cancellationToken))).Database;
    }

    [Obsolete("Use GetSessionHandleAsync method.")]
    protected virtual IClientSessionHandle SessionHandle => DbContext.SessionHandle;

    protected async Task<IClientSessionHandle> GetSessionHandleAsync(CancellationToken cancellationToken = default)
    {
        return (await GetDbContextAsync(GetCancellationToken(cancellationToken))).SessionHandle;
    }

    [Obsolete("Use GetDbContextAsync method.")]
    protected virtual TMongoDbContext DbContext => GetDbContext();

    [Obsolete("Use GetDbContextAsync method.")]
    private TMongoDbContext GetDbContext()
    {
        // Multi-tenancy unaware entities should always use the host connection string
        if (!EntityHelper.IsMultiTenant<TEntity>())
        {
            using (CurrentTenant.Change(null))
            {
                return DbContextProvider.GetDbContext();
            }
        }

        return DbContextProvider.GetDbContext();
    }

    protected Task<TMongoDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        // Multi-tenancy unaware entities should always use the host connection string
        if (!EntityHelper.IsMultiTenant<TEntity>())
        {
            using (CurrentTenant.Change(null))
            {
                return DbContextProvider.GetDbContextAsync(cancellationToken);
            }
        }

        return DbContextProvider.GetDbContextAsync(cancellationToken);
    }

    protected IMongoDbContextProvider<TMongoDbContext> DbContextProvider { get; }

    public ILocalEventBus LocalEventBus => LazyServiceProvider.LazyGetService<ILocalEventBus>(NullLocalEventBus.Instance);

    public IDistributedEventBus DistributedEventBus => LazyServiceProvider.LazyGetService<IDistributedEventBus>(NullDistributedEventBus.Instance);

    public IEntityChangeEventHelper EntityChangeEventHelper => LazyServiceProvider.LazyGetService<IEntityChangeEventHelper>(NullEntityChangeEventHelper.Instance);

    public IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

    public IAuditPropertySetter AuditPropertySetter => LazyServiceProvider.LazyGetRequiredService<IAuditPropertySetter>();

    public IMongoDbBulkOperationProvider BulkOperationProvider => LazyServiceProvider.LazyGetService<IMongoDbBulkOperationProvider>();

    public MongoDbRepository(IMongoDbContextProvider<TMongoDbContext> dbContextProvider)
    {
        DbContextProvider = dbContextProvider;
    }

    public async override Task<TEntity> InsertAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        await ApplyAbpConceptsForAddedEntityAsync(entity);

        var dbContext = await GetDbContextAsync(cancellationToken);
        var collection = dbContext.Collection<TEntity>();

        if (dbContext.SessionHandle != null)
        {
            await collection.InsertOneAsync(
                dbContext.SessionHandle,
                entity,
                cancellationToken: cancellationToken
            );
        }
        else
        {
            await collection.InsertOneAsync(
                entity,
                cancellationToken: cancellationToken
            );
        }

        return entity;
    }

    public async override Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var entityArray = entities.ToArray();

        foreach (var entity in entityArray)
        {
            await ApplyAbpConceptsForAddedEntityAsync(entity);
        }

        var dbContext = await GetDbContextAsync(cancellationToken);
        var collection = dbContext.Collection<TEntity>();

        if (BulkOperationProvider != null)
        {
            await BulkOperationProvider.InsertManyAsync(this, entityArray, dbContext.SessionHandle, autoSave, cancellationToken);
            return;
        }

        if (dbContext.SessionHandle != null)
        {
            await collection.InsertManyAsync(
                dbContext.SessionHandle,
                entityArray,
                cancellationToken: cancellationToken);
        }
        else
        {
            await collection.InsertManyAsync(
                entityArray,
                cancellationToken: cancellationToken);
        }
    }

    public async override Task<TEntity> UpdateAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        SetModificationAuditProperties(entity);

        if (entity is ISoftDelete softDeleteEntity && softDeleteEntity.IsDeleted)
        {
            SetDeletionAuditProperties(entity);
            TriggerEntityDeleteEvents(entity);
        }
        else
        {
            TriggerEntityUpdateEvents(entity);
        }

        TriggerDomainEvents(entity);

        var oldConcurrencyStamp = SetNewConcurrencyStamp(entity);
        ReplaceOneResult result;

        var dbContext = await GetDbContextAsync(cancellationToken);
        var collection = dbContext.Collection<TEntity>();

        if (dbContext.SessionHandle != null)
        {
            result = await collection.ReplaceOneAsync(
                dbContext.SessionHandle,
                await CreateEntityFilterAsync(entity, true, oldConcurrencyStamp),
                entity,
                cancellationToken: cancellationToken
            );
        }
        else
        {
            result = await collection.ReplaceOneAsync(
                await CreateEntityFilterAsync(entity, true, oldConcurrencyStamp),
                entity,
                cancellationToken: cancellationToken
            );
        }

        if (result.MatchedCount <= 0)
        {
            ThrowOptimisticConcurrencyException();
        }

        return entity;
    }

    public async override Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entityArray = entities.ToArray();

        foreach (var entity in entityArray)
        {
            SetModificationAuditProperties(entity);

            var isEntityDeleted = entity is ISoftDelete softDeleteEntity && softDeleteEntity.IsDeleted;
            if (isEntityDeleted)
            {
                SetDeletionAuditProperties(entity);
                TriggerEntityDeleteEvents(entity);
            }
            else
            {
                TriggerEntityUpdateEvents(entity);
            }

            TriggerDomainEvents(entity);

            SetNewConcurrencyStamp(entity);
        }

        cancellationToken = GetCancellationToken(cancellationToken);
        var dbContext = await GetDbContextAsync(cancellationToken);

        if (BulkOperationProvider != null)
        {
            await BulkOperationProvider.UpdateManyAsync(this, entityArray, dbContext.SessionHandle, autoSave, cancellationToken);
            return;
        }

        BulkWriteResult result;

        var replaceRequests = new List<WriteModel<TEntity>>();
        foreach (var entity in entityArray)
        {
            replaceRequests.Add(new ReplaceOneModel<TEntity>(await CreateEntityFilterAsync(entity), entity));
        }

        var collection = dbContext.Collection<TEntity>();
        if (dbContext.SessionHandle != null)
        {
            result = await collection.BulkWriteAsync(dbContext.SessionHandle, replaceRequests, cancellationToken: cancellationToken);
        }
        else
        {
            result = await collection.BulkWriteAsync(replaceRequests, cancellationToken: cancellationToken);
        }

        if (result.MatchedCount < entityArray.Length)
        {
            ThrowOptimisticConcurrencyException();
        }
    }

    public async override Task DeleteAsync(
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var dbContext = await GetDbContextAsync(cancellationToken);
        var collection = dbContext.Collection<TEntity>();

        var oldConcurrencyStamp = SetNewConcurrencyStamp(entity);

        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)) && !IsHardDeleted(entity))
        {
            ObjectHelper.TrySetProperty(((ISoftDelete)entity), x => x.IsDeleted, () => true);
            ApplyAbpConceptsForDeletedEntity(entity);

            ReplaceOneResult result;

            if (dbContext.SessionHandle != null)
            {
                result = await collection.ReplaceOneAsync(
                    dbContext.SessionHandle,
                    await CreateEntityFilterAsync(entity, true, oldConcurrencyStamp),
                    entity,
                    cancellationToken: cancellationToken
                );
            }
            else
            {
                result = await collection.ReplaceOneAsync(
                    await CreateEntityFilterAsync(entity, true, oldConcurrencyStamp),
                    entity,
                    cancellationToken: cancellationToken
                );
            }

            if (result.MatchedCount <= 0)
            {
                ThrowOptimisticConcurrencyException();
            }
        }
        else
        {
            ApplyAbpConceptsForDeletedEntity(entity);

            DeleteResult result;

            if (dbContext.SessionHandle != null)
            {
                result = await collection.DeleteOneAsync(
                    dbContext.SessionHandle,
                    await CreateEntityFilterAsync(entity, true, oldConcurrencyStamp),
                    cancellationToken: cancellationToken
                );
            }
            else
            {
                result = await collection.DeleteOneAsync(
                    await CreateEntityFilterAsync(entity, true, oldConcurrencyStamp),
                    cancellationToken
                );
            }

            if (result.DeletedCount <= 0)
            {
                ThrowOptimisticConcurrencyException();
            }
        }
    }

    public async override Task DeleteManyAsync(
       IEnumerable<TEntity> entities,
       bool autoSave = false,
       CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var softDeletedEntities = new Dictionary<TEntity, string>();
        var hardDeletedEntities = new List<TEntity>();

        foreach (var entity in entities)
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)) && !IsHardDeleted(entity))
            {
                ObjectHelper.TrySetProperty(((ISoftDelete)entity), x => x.IsDeleted, () => true);
                softDeletedEntities.Add(entity, SetNewConcurrencyStamp(entity));
            }
            else
            {
                hardDeletedEntities.Add(entity);
            }

            ApplyAbpConceptsForDeletedEntity(entity);
        }

        var dbContext = await GetDbContextAsync(cancellationToken);
        var collection = dbContext.Collection<TEntity>();

        if (BulkOperationProvider != null)
        {
            await BulkOperationProvider.DeleteManyAsync(this, entities, dbContext.SessionHandle, autoSave, cancellationToken);
            return;
        }

        if (softDeletedEntities.Count > 0)
        {
            BulkWriteResult updateResult;

            var replaceRequests = new List<WriteModel<TEntity>>();
            foreach (var softDeletedEntity in softDeletedEntities)
            {
                replaceRequests.Add(new ReplaceOneModel<TEntity>(await CreateEntityFilterAsync(softDeletedEntity.Key, true, softDeletedEntity.Value), softDeletedEntity.Key));
            }

            if (dbContext.SessionHandle != null)
            {
                updateResult = await collection.BulkWriteAsync(dbContext.SessionHandle, replaceRequests, cancellationToken: cancellationToken);
            }
            else
            {
                updateResult = await collection.BulkWriteAsync(replaceRequests, cancellationToken: cancellationToken);
            }

            if (updateResult.MatchedCount < softDeletedEntities.Count)
            {
                ThrowOptimisticConcurrencyException();
            }
        }

        if (hardDeletedEntities.Count > 0)
        {
            DeleteResult deleteResult;
            var hardDeletedEntitiesCount = hardDeletedEntities.Count;

            if (dbContext.SessionHandle != null)
            {
                deleteResult = await collection.DeleteManyAsync(
                    dbContext.SessionHandle,
                    await CreateEntitiesFilterAsync(hardDeletedEntities),
                    cancellationToken: cancellationToken);
            }
            else
            {
                deleteResult = await collection.DeleteManyAsync(
                    await CreateEntitiesFilterAsync(hardDeletedEntities),
                    cancellationToken: cancellationToken);
            }

            if (deleteResult.DeletedCount < hardDeletedEntitiesCount)
            {
                ThrowOptimisticConcurrencyException();
            }
        }
    }

    public async override Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetMongoQueryableAsync(cancellationToken)).ToListAsync(cancellationToken);
    }

    public async override Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetMongoQueryableAsync(cancellationToken)).Where(predicate).ToListAsync(cancellationToken);
    }

    public async override Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        return await (await GetMongoQueryableAsync(cancellationToken)).LongCountAsync(cancellationToken);
    }

    public async override Task<List<TEntity>> GetPagedListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        return await (await GetMongoQueryableAsync(cancellationToken))
            .OrderByIf<TEntity, IQueryable<TEntity>>(!sorting.IsNullOrWhiteSpace(), sorting)
            .As<IMongoQueryable<TEntity>>()
            .PageBy<TEntity, IMongoQueryable<TEntity>>(skipCount, maxResultCount)
            .ToListAsync(cancellationToken);
    }

    public async override Task DeleteAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var entities = await (await GetMongoQueryableAsync(cancellationToken))
            .Where(predicate)
            .ToListAsync(cancellationToken);

        await DeleteManyAsync(entities, autoSave, cancellationToken);
    }

    [Obsolete("Use GetQueryableAsync method.")]
    protected override IQueryable<TEntity> GetQueryable()
    {
        return GetMongoQueryable();
    }

    public async override Task<IQueryable<TEntity>> GetQueryableAsync()
    {
        return await GetMongoQueryableAsync();
    }

    public async override Task<TEntity> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(predicate)
            .SingleOrDefaultAsync(cancellationToken);
    }

    [Obsolete("Use GetMongoQueryableAsync method.")]
    public virtual IMongoQueryable<TEntity> GetMongoQueryable()
    {
        return ApplyDataFilters(
            SessionHandle != null
                ? Collection.AsQueryable(SessionHandle)
                : Collection.AsQueryable()
        );
    }

    public virtual Task<IMongoQueryable<TEntity>> GetMongoQueryableAsync(CancellationToken cancellationToken = default, AggregateOptions aggregateOptions = null)
    {
        return GetMongoQueryableAsync<TEntity>(cancellationToken, aggregateOptions);
    }

    protected virtual async Task<IMongoQueryable<TOtherEntity>> GetMongoQueryableAsync<TOtherEntity>(CancellationToken cancellationToken = default, AggregateOptions aggregateOptions = null)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var dbContext = await GetDbContextAsync(cancellationToken);
        var collection = dbContext.Collection<TOtherEntity>();

        return ApplyDataFilters<IMongoQueryable<TOtherEntity>, TOtherEntity>(
            dbContext.SessionHandle != null
                ? collection.AsQueryable(dbContext.SessionHandle, aggregateOptions)
                : collection.AsQueryable(aggregateOptions)
        );
    }

    public virtual async Task<IAggregateFluent<TEntity>> GetAggregateAsync(CancellationToken cancellationToken = default, AggregateOptions aggregateOptions = null)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var dbContext = await GetDbContextAsync(cancellationToken);
        var collection = await GetCollectionAsync(cancellationToken);

        var aggregate = dbContext.SessionHandle != null
                ? collection.Aggregate(dbContext.SessionHandle, aggregateOptions)
                : collection.Aggregate(aggregateOptions);

        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)) && DataFilter.IsEnabled<ISoftDelete>())
        {
            aggregate = aggregate.Match(e => ((ISoftDelete)e).IsDeleted == false);
        }

        if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)) && DataFilter.IsEnabled<IMultiTenant>())
        {
            var tenantId = CurrentTenant.Id;
            aggregate = aggregate.Match(e => ((IMultiTenant)e).TenantId == tenantId);
        }

        return aggregate;
    }

    protected virtual bool IsHardDeleted(TEntity entity)
    {
        var hardDeletedEntities = UnitOfWorkManager?.Current?.Items.GetOrDefault(UnitOfWorkItemNames.HardDeletedEntities) as HashSet<IEntity>;
        if (hardDeletedEntities == null)
        {
            return false;
        }

        return hardDeletedEntities.Contains(entity);
    }

    protected virtual Task<FilterDefinition<TEntity>> CreateEntityFilterAsync(TEntity entity, bool withConcurrencyStamp = false, string concurrencyStamp = null)
    {
        throw new NotImplementedException(
            $"{nameof(CreateEntityFilterAsync)} is not implemented for MongoDB by default. It should be overriden and implemented by the deriving class!"
        );
    }

    protected virtual Task<FilterDefinition<TEntity>> CreateEntitiesFilterAsync(IEnumerable<TEntity> entities, bool withConcurrencyStamp = false)
    {
        throw new NotImplementedException(
          $"{nameof(CreateEntitiesFilterAsync)} is not implemented for MongoDB by default. It should be overriden and implemented by the deriving class!"
      );
    }

    protected virtual Task ApplyAbpConceptsForAddedEntityAsync(TEntity entity)
    {
        CheckAndSetId(entity);
        SetCreationAuditProperties(entity);
        TriggerEntityCreateEvents(entity);
        TriggerDomainEvents(entity);
        return Task.CompletedTask;
    }

    private void TriggerEntityCreateEvents(TEntity entity)
    {
        EntityChangeEventHelper.PublishEntityCreatedEvent(entity);
    }

    protected virtual void TriggerEntityUpdateEvents(TEntity entity)
    {
        EntityChangeEventHelper.PublishEntityUpdatedEvent(entity);
    }

    protected virtual void ApplyAbpConceptsForDeletedEntity(TEntity entity)
    {
        SetDeletionAuditProperties(entity);
        TriggerEntityDeleteEvents(entity);
        TriggerDomainEvents(entity);
    }

    protected virtual void TriggerEntityDeleteEvents(TEntity entity)
    {
        EntityChangeEventHelper.PublishEntityDeletedEvent(entity);
    }

    protected virtual void CheckAndSetId(TEntity entity)
    {
        if (entity is IEntity<Guid> entityWithGuidId)
        {
            TrySetGuidId(entityWithGuidId);
        }
    }

    protected virtual void TrySetGuidId(IEntity<Guid> entity)
    {
        if (entity.Id != default)
        {
            return;
        }

        EntityHelper.TrySetId(
            entity,
            () => GuidGenerator.Create(),
            true
        );
    }

    protected virtual void SetCreationAuditProperties(TEntity entity)
    {
        AuditPropertySetter.SetCreationProperties(entity);
    }

    protected virtual void SetModificationAuditProperties(TEntity entity)
    {
        AuditPropertySetter.SetModificationProperties(entity);
    }

    protected virtual void SetDeletionAuditProperties(TEntity entity)
    {
        AuditPropertySetter.SetDeletionProperties(entity);
    }

    protected virtual void TriggerDomainEvents(object entity)
    {
        var generatesDomainEventsEntity = entity as IGeneratesDomainEvents;
        if (generatesDomainEventsEntity == null)
        {
            return;
        }

        var localEvents = generatesDomainEventsEntity.GetLocalEvents()?.ToArray();
        if (localEvents != null && localEvents.Any())
        {
            foreach (var localEvent in localEvents)
            {
                UnitOfWorkManager.Current?.AddOrReplaceLocalEvent(
                    new UnitOfWorkEventRecord(
                        localEvent.EventData.GetType(),
                        localEvent.EventData,
                        localEvent.EventOrder
                    )
                );
            }

            generatesDomainEventsEntity.ClearLocalEvents();
        }

        var distributedEvents = generatesDomainEventsEntity.GetDistributedEvents()?.ToArray();
        if (distributedEvents != null && distributedEvents.Any())
        {
            foreach (var distributedEvent in distributedEvents)
            {
                UnitOfWorkManager.Current?.AddOrReplaceDistributedEvent(
                    new UnitOfWorkEventRecord(
                        distributedEvent.EventData.GetType(),
                        distributedEvent.EventData,
                        distributedEvent.EventOrder
                    )
                );
            }

            generatesDomainEventsEntity.ClearDistributedEvents();
        }
    }

    /// <summary>
    /// Sets a new <see cref="IHasConcurrencyStamp.ConcurrencyStamp"/> value
    /// if given entity implements <see cref="IHasConcurrencyStamp"/> interface.
    /// Returns the old <see cref="IHasConcurrencyStamp.ConcurrencyStamp"/> value.
    /// </summary>
    protected virtual string SetNewConcurrencyStamp(TEntity entity)
    {
        if (!(entity is IHasConcurrencyStamp concurrencyStampEntity))
        {
            return null;
        }

        var oldConcurrencyStamp = concurrencyStampEntity.ConcurrencyStamp;
        concurrencyStampEntity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
        return oldConcurrencyStamp;
    }

    protected virtual void ThrowOptimisticConcurrencyException()
    {
        throw new AbpDbConcurrencyException("Database operation expected to affect 1 row but actually affected 0 row. Data may have been modified or deleted since entities were loaded. This exception has been thrown on optimistic concurrency check.");
    }
}

public class MongoDbRepository<TMongoDbContext, TEntity, TKey>
    : MongoDbRepository<TMongoDbContext, TEntity>,
    IMongoDbRepository<TEntity, TKey>
    where TMongoDbContext : IAbpMongoDbContext
    where TEntity : class, IEntity<TKey>
{
    public IMongoDbRepositoryFilterer<TEntity, TKey> RepositoryFilterer { get; set; }

    public MongoDbRepository(IMongoDbContextProvider<TMongoDbContext> dbContextProvider)
        : base(dbContextProvider)
    {

    }

    public virtual async Task<TEntity> GetAsync(
        TKey id,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(id, includeDetails, GetCancellationToken(cancellationToken));

        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }

        return entity;
    }

    public virtual async Task<TEntity> FindAsync(
        TKey id,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        return await ApplyDataFilters(await GetMongoQueryableAsync(cancellationToken)).Where(x => x.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);
    }

    public virtual Task DeleteAsync(
        TKey id,
        bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        return DeleteAsync(x => x.Id.Equals(id), autoSave, cancellationToken);
    }

    public virtual async Task DeleteManyAsync([NotNull] IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);

        var entities = await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        await DeleteManyAsync(entities, autoSave, cancellationToken);
    }

    protected async override Task<FilterDefinition<TEntity>> CreateEntityFilterAsync(TEntity entity, bool withConcurrencyStamp = false, string concurrencyStamp = null)
    {
        return await RepositoryFilterer.CreateEntityFilterAsync(entity, withConcurrencyStamp, concurrencyStamp);
    }

    protected async override Task<FilterDefinition<TEntity>> CreateEntitiesFilterAsync(IEnumerable<TEntity> entities, bool withConcurrencyStamp = false)
    {
        return await RepositoryFilterer.CreateEntitiesFilterAsync(entities, withConcurrencyStamp);
    }
}
