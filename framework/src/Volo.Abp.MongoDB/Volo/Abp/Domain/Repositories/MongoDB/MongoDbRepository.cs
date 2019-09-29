using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Volo.Abp.Reflection;
using Volo.Abp.Threading;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public class MongoDbRepository<TMongoDbContext, TEntity>
        : RepositoryBase<TEntity>,
        IMongoDbRepository<TEntity>
        where TMongoDbContext : IAbpMongoDbContext
        where TEntity : class, IEntity
    {
        public virtual IMongoCollection<TEntity> Collection => DbContext.Collection<TEntity>();

        public virtual IMongoDatabase Database => DbContext.Database;

        public virtual TMongoDbContext DbContext => DbContextProvider.GetDbContext();

        protected IMongoDbContextProvider<TMongoDbContext> DbContextProvider { get; }

        public ILocalEventBus LocalEventBus { get; set; }

        public IDistributedEventBus DistributedEventBus { get; set; }

        public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        public IAuditPropertySetter AuditPropertySetter { get; set; }

        public MongoDbRepository(IMongoDbContextProvider<TMongoDbContext> dbContextProvider)
        {
            DbContextProvider = dbContextProvider;

            LocalEventBus = NullLocalEventBus.Instance;
            DistributedEventBus = NullDistributedEventBus.Instance;
            EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
        }

        public override TEntity Insert(TEntity entity, bool autoSave = false)
        {
            /* EntityCreatedEvent (OnUowCompleted) is triggered as the first because it should be
             * triggered before other events triggered inside an EntityCreating event handler.
             * This is also true for other "ed" & "ing" events.
             */

            AsyncHelper.RunSync(() => ApplyAbpConceptsForAddedEntityAsync(entity));

            Collection.InsertOne(entity);

            return entity;
        }

        public override async Task<TEntity> InsertAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            await ApplyAbpConceptsForAddedEntityAsync(entity);

            await Collection.InsertOneAsync(
                entity,
                cancellationToken: GetCancellationToken(cancellationToken)
            );

            return entity;
        }

        public override TEntity Update(TEntity entity, bool autoSave = false)
        {
            SetModificationAuditProperties(entity);

            if (entity is ISoftDelete softDeleteEntity && softDeleteEntity.IsDeleted)
            {
                SetDeletionAuditProperties(entity);
                AsyncHelper.RunSync(() => TriggerEntityDeleteEventsAsync(entity));
            }
            else
            {
                AsyncHelper.RunSync(() => TriggerEntityUpdateEventsAsync(entity));
            }

            AsyncHelper.RunSync(() => TriggerDomainEventsAsync(entity));

            var oldConcurrencyStamp = SetNewConcurrencyStamp(entity);

            var result = Collection.ReplaceOne(
                CreateEntityFilter(entity, true, oldConcurrencyStamp),
                entity
            );

            if (result.MatchedCount <= 0)
            {
                ThrowOptimisticConcurrencyException();
            }

            return entity;
        }

        public override async Task<TEntity> UpdateAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            SetModificationAuditProperties(entity);

            if (entity is ISoftDelete softDeleteEntity && softDeleteEntity.IsDeleted)
            {
                SetDeletionAuditProperties(entity);
                await TriggerEntityDeleteEventsAsync(entity);
            }
            else
            {
                await TriggerEntityUpdateEventsAsync(entity);
            }

            await TriggerDomainEventsAsync(entity);

            var oldConcurrencyStamp = SetNewConcurrencyStamp(entity);

            var result = await Collection.ReplaceOneAsync(
                CreateEntityFilter(entity, true, oldConcurrencyStamp),
                entity,
                cancellationToken: GetCancellationToken(cancellationToken)
            );

            if (result.MatchedCount <= 0)
            {
                ThrowOptimisticConcurrencyException();
            }

            return entity;
        }

        public override void Delete(TEntity entity, bool autoSave = false)
        {
            AsyncHelper.RunSync(() => ApplyAbpConceptsForDeletedEntityAsync(entity));
            var oldConcurrencyStamp = SetNewConcurrencyStamp(entity);

            if (entity is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                var result = Collection.ReplaceOne(
                    CreateEntityFilter(entity, true, oldConcurrencyStamp),
                    entity
                );

                if (result.MatchedCount <= 0)
                {
                    ThrowOptimisticConcurrencyException();
                }
            }
            else
            {
                var result = Collection.DeleteOne(
                    CreateEntityFilter(entity, true, oldConcurrencyStamp)
                );

                if (result.DeletedCount <= 0)
                {
                    ThrowOptimisticConcurrencyException();
                }
            }
        }

        public override async Task DeleteAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            await ApplyAbpConceptsForDeletedEntityAsync(entity);
            var oldConcurrencyStamp = SetNewConcurrencyStamp(entity);

            if (entity is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                var result = await Collection.ReplaceOneAsync(
                    CreateEntityFilter(entity, true, oldConcurrencyStamp),
                    entity,
                    cancellationToken: GetCancellationToken(cancellationToken)
                );

                if (result.MatchedCount <= 0)
                {
                    ThrowOptimisticConcurrencyException();
                }
            }
            else
            {
                var result = await Collection.DeleteOneAsync(
                    CreateEntityFilter(entity, true, oldConcurrencyStamp),
                    GetCancellationToken(cancellationToken)
                );

                if (result.DeletedCount <= 0)
                {
                    ThrowOptimisticConcurrencyException();
                }
            }
        }

        public override List<TEntity> GetList(bool includeDetails = false)
        {
            return GetMongoQueryable().ToList();
        }

        public override async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override long GetCount()
        {
            return GetMongoQueryable().LongCount();
        }

        public override async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public override void Delete(Expression<Func<TEntity, bool>> predicate, bool autoSave = false)
        {
            var entities = GetMongoQueryable()
                .Where(predicate)
                .ToList();

            foreach (var entity in entities)
            {
                Delete(entity, autoSave);
            }
        }

        public override async Task DeleteAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            var entities = await GetMongoQueryable()
                .Where(predicate)
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var entity in entities)
            {
                await DeleteAsync(entity, autoSave, cancellationToken);
            }
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return GetMongoQueryable();
        }

        public virtual IMongoQueryable<TEntity> GetMongoQueryable()
        {
            return ApplyDataFilters(
                Collection.AsQueryable()
            );
        }

        protected virtual FilterDefinition<TEntity> CreateEntityFilter(TEntity entity, bool withConcurrencyStamp = false, string concurrencyStamp = null)
        {
            throw new NotImplementedException(
                $"{nameof(CreateEntityFilter)} is not implemented for MongoDB by default. It should be overrided and implemented by the deriving class!"
            );
        }

        protected virtual async Task ApplyAbpConceptsForAddedEntityAsync(TEntity entity)
        {
            CheckAndSetId(entity);
            SetCreationAuditProperties(entity);
            await TriggerEntityCreateEvents(entity);
            await TriggerDomainEventsAsync(entity);
        }

        private async Task TriggerEntityCreateEvents(TEntity entity)
        {
            await EntityChangeEventHelper.TriggerEntityCreatedEventOnUowCompletedAsync(entity);
            await EntityChangeEventHelper.TriggerEntityCreatingEventAsync(entity);
        }

        protected virtual async Task TriggerEntityUpdateEventsAsync(TEntity entity)
        {
            await EntityChangeEventHelper.TriggerEntityUpdatedEventOnUowCompletedAsync(entity);
            await EntityChangeEventHelper.TriggerEntityUpdatingEventAsync(entity);
        }

        protected virtual async Task ApplyAbpConceptsForDeletedEntityAsync(TEntity entity)
        {
            SetDeletionAuditProperties(entity);
            await TriggerEntityDeleteEventsAsync(entity);
            await TriggerDomainEventsAsync(entity);
        }

        protected virtual async Task TriggerEntityDeleteEventsAsync(TEntity entity)
        {
            await EntityChangeEventHelper.TriggerEntityDeletedEventOnUowCompletedAsync(entity);
            await EntityChangeEventHelper.TriggerEntityDeletingEventAsync(entity);
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

        protected virtual async Task TriggerDomainEventsAsync(object entity)
        {
            var generatesDomainEventsEntity = entity as IGeneratesDomainEvents;
            if (generatesDomainEventsEntity == null)
            {
                return;
            }

            var localEvents = generatesDomainEventsEntity.GetLocalEvents().ToArray();
            if (localEvents.Any())
            {
                foreach (var localEvent in localEvents)
                {
                    await LocalEventBus.PublishAsync(localEvent.GetType(), localEvent);
                }

                generatesDomainEventsEntity.ClearLocalEvents();
            }

            var distributedEvents = generatesDomainEventsEntity.GetDistributedEvents().ToArray();
            if (distributedEvents.Any())
            {
                foreach (var distributedEvent in distributedEvents)
                {
                    await DistributedEventBus.PublishAsync(distributedEvent.GetType(), distributedEvent);
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
        public MongoDbRepository(IMongoDbContextProvider<TMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual TEntity Get(TKey id, bool includeDetails = true)
        {
            var entity = Find(id, includeDetails);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual async Task<TEntity> GetAsync(
            TKey id,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, includeDetails, cancellationToken);

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
            return await Collection
                .Find(CreateEntityFilter(id, true))
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual TEntity Find(TKey id, bool includeDetails = true)
        {
            return Collection.Find(CreateEntityFilter(id, true)).FirstOrDefault();
        }

        public virtual void Delete(TKey id, bool autoSave = false)
        {
            Collection.DeleteOne(CreateEntityFilter(id));
        }

        public virtual Task DeleteAsync(
            TKey id,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            return Collection.DeleteOneAsync(
                CreateEntityFilter(id),
                GetCancellationToken(cancellationToken)
            );
        }

        protected override FilterDefinition<TEntity> CreateEntityFilter(TEntity entity, bool withConcurrencyStamp = false, string concurrencyStamp = null)
        {
            if (!withConcurrencyStamp || !(entity is IHasConcurrencyStamp entityWithConcurrencyStamp))
            {
                return Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            }

            if (concurrencyStamp == null)
            {
                concurrencyStamp = entityWithConcurrencyStamp.ConcurrencyStamp;
            }

            return Builders<TEntity>.Filter.And(
                Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id),
                Builders<TEntity>.Filter.Eq(e => ((IHasConcurrencyStamp)e).ConcurrencyStamp, concurrencyStamp)
            );
        }

        protected virtual FilterDefinition<TEntity> CreateEntityFilter(TKey id, bool applyFilters = false)
        {
            var filters = new List<FilterDefinition<TEntity>>
            {
                Builders<TEntity>.Filter.Eq(e => e.Id, id)
            };

            if (applyFilters)
            {
                AddGlobalFilters(filters);
            }

            return Builders<TEntity>.Filter.And(filters);
        }

        protected virtual void AddGlobalFilters(List<FilterDefinition<TEntity>> filters)
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)) && DataFilter.IsEnabled<ISoftDelete>())
            {
                filters.Add(Builders<TEntity>.Filter.Eq(e => ((ISoftDelete)e).IsDeleted, false));
            }

            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                var tenantId = CurrentTenant.Id;
                filters.Add(Builders<TEntity>.Filter.Eq(e => ((IMultiTenant)e).TenantId, tenantId));
            }
        }
    }
}