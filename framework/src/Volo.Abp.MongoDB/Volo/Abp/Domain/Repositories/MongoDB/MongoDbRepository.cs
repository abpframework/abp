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
using Volo.Abp.MongoDB.Volo.Abp.Domain.Repositories.MongoDB;

namespace Volo.Abp.Domain.Repositories.MongoDB
{
    public class MongoDbRepository<TMongoDbContext, TEntity>
        : RepositoryBase<TEntity>,
        IMongoDbRepository<TEntity>,
        IMongoQueryable<TEntity>
        where TMongoDbContext : IAbpMongoDbContext
        where TEntity : class, IEntity
    {
        public virtual IMongoCollection<TEntity> Collection => DbContext.Collection<TEntity>();

        public virtual IMongoDatabase Database => DbContext.Database;

        public virtual IClientSessionHandle SessionHandle => DbContext.SessionHandle;

        public virtual TMongoDbContext DbContext => DbContextProvider.GetDbContext();

        protected IMongoDbContextProvider<TMongoDbContext> DbContextProvider { get; }

        public ILocalEventBus LocalEventBus { get; set; }

        public IDistributedEventBus DistributedEventBus { get; set; }

        public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        public IAuditPropertySetter AuditPropertySetter { get; set; }

        public IMongoDbBulkOperationProvider BulkOperationProvider { get; set; }

        public MongoDbRepository(IMongoDbContextProvider<TMongoDbContext> dbContextProvider)
        {
            DbContextProvider = dbContextProvider;

            LocalEventBus = NullLocalEventBus.Instance;
            DistributedEventBus = NullDistributedEventBus.Instance;
            EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
            GuidGenerator = SimpleGuidGenerator.Instance;
        }

        public async override Task<TEntity> InsertAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            await ApplyAbpConceptsForAddedEntityAsync(entity);

            if (SessionHandle != null)
            {
                await Collection.InsertOneAsync(
                    SessionHandle,
                    entity,
                    cancellationToken: GetCancellationToken(cancellationToken)
                );
            }
            else
            {
                await Collection.InsertOneAsync(
                    entity,
                    cancellationToken: GetCancellationToken(cancellationToken)
                );
            }

            return entity;
        }

        public override async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await ApplyAbpConceptsForAddedEntityAsync(entity);
            }

            if (BulkOperationProvider != null)
            {
                await BulkOperationProvider.InsertManyAsync(this, entities, SessionHandle, autoSave, cancellationToken);
                return;
            }

            if (SessionHandle != null)
            {
                await Collection.InsertManyAsync(
                    SessionHandle,
                    entities,
                    cancellationToken: cancellationToken);
            }
            else
            {
                await Collection.InsertManyAsync(
                    entities,
                    cancellationToken: cancellationToken);
            }
        }

        public async override Task<TEntity> UpdateAsync(
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
            ReplaceOneResult result;

            if (SessionHandle != null)
            {
                result = await Collection.ReplaceOneAsync(
                    SessionHandle,
                    CreateEntityFilter(entity, true, oldConcurrencyStamp),
                    entity,
                    cancellationToken: GetCancellationToken(cancellationToken)
                );


            }
            else
            {
                result = await Collection.ReplaceOneAsync(
                    CreateEntityFilter(entity, true, oldConcurrencyStamp),
                    entity,
                    cancellationToken: GetCancellationToken(cancellationToken)
                );
            }

            if (result.MatchedCount <= 0)
            {
                ThrowOptimisticConcurrencyException();
            }

            return entity;
        }

        public override async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var isSoftDeleteEntity = typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity));

            foreach (var entity in entities)
            {
                SetModificationAuditProperties(entity);

                if (isSoftDeleteEntity)
                {
                    SetDeletionAuditProperties(entity);
                    await TriggerEntityDeleteEventsAsync(entity);
                }
                else
                {
                    await TriggerEntityUpdateEventsAsync(entity);
                }

                await TriggerDomainEventsAsync(entity);

                SetNewConcurrencyStamp(entity);
            }

            if (BulkOperationProvider != null)
            {
                await BulkOperationProvider.UpdateManyAsync(this, entities, SessionHandle, autoSave, cancellationToken);
                return;
            }

            var entitiesCount = entities.Count();
            BulkWriteResult result;

            List<WriteModel<TEntity>> replaceRequests = new List<WriteModel<TEntity>>();
            foreach (var entity in entities)
            {
                replaceRequests.Add(new ReplaceOneModel<TEntity>(CreateEntityFilter(entity), entity));
            }

            if (SessionHandle != null)
            {
                result = await Collection.BulkWriteAsync(SessionHandle, replaceRequests);
            }
            else
            {
                result = await Collection.BulkWriteAsync(replaceRequests);
            }

            if (result.MatchedCount < entitiesCount)
            {
                ThrowOptimisticConcurrencyException();
            }
        }

        public async override Task DeleteAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            await ApplyAbpConceptsForDeletedEntityAsync(entity);
            var oldConcurrencyStamp = SetNewConcurrencyStamp(entity);

            if (entity is ISoftDelete softDeleteEntity && !IsHardDeleted(entity))
            {
                softDeleteEntity.IsDeleted = true;
                ReplaceOneResult result;

                if (SessionHandle != null)
                {
                    result = await Collection.ReplaceOneAsync(
                        SessionHandle,
                        CreateEntityFilter(entity, true, oldConcurrencyStamp),
                        entity,
                        cancellationToken: GetCancellationToken(cancellationToken)
                    );
                }
                else
                {
                    result = await Collection.ReplaceOneAsync(
                        CreateEntityFilter(entity, true, oldConcurrencyStamp),
                        entity,
                        cancellationToken: GetCancellationToken(cancellationToken)
                    );
                }

                if (result.MatchedCount <= 0)
                {
                    ThrowOptimisticConcurrencyException();
                }
            }
            else
            {
                DeleteResult result;

                if (SessionHandle != null)
                {
                    result = await Collection.DeleteOneAsync(
                        SessionHandle,
                        CreateEntityFilter(entity, true, oldConcurrencyStamp),
                        cancellationToken: GetCancellationToken(cancellationToken)
                    );
                }
                else
                {
                    result = await Collection.DeleteOneAsync(
                        CreateEntityFilter(entity, true, oldConcurrencyStamp),
                        GetCancellationToken(cancellationToken)
                    );
                }

                if (result.DeletedCount <= 0)
                {
                    ThrowOptimisticConcurrencyException();
                }
            }
        }

        public override async Task DeleteManyAsync(
            IEnumerable<TEntity> entities,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await ApplyAbpConceptsForDeletedEntityAsync(entity);
                var oldConcurrencyStamp = SetNewConcurrencyStamp(entity);
            }

            if (BulkOperationProvider != null)
            {
                await BulkOperationProvider.DeleteManyAsync(this, entities, SessionHandle, autoSave, cancellationToken);
                return;
            }

            var entitiesCount = entities.Count();

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                UpdateResult updateResult;
                if (SessionHandle != null)
                {
                    updateResult = await Collection.UpdateManyAsync(
                        SessionHandle,
                        CreateEntitiesFilter(entities),
                        Builders<TEntity>.Update.Set(x => ((ISoftDelete)x).IsDeleted, true)
                        );
                }
                else
                {
                    updateResult = await Collection.UpdateManyAsync(
                        CreateEntitiesFilter(entities),
                        Builders<TEntity>.Update.Set(x => ((ISoftDelete)x).IsDeleted, true)
                        );
                }

                if (updateResult.MatchedCount < entitiesCount)
                {
                    ThrowOptimisticConcurrencyException();
                }
            }
            else
            {
                DeleteResult deleteResult;
                if (SessionHandle != null)
                {
                    deleteResult = await Collection.DeleteManyAsync(
                        SessionHandle,
                        CreateEntitiesFilter(entities)
                        );
                }
                else
                {
                    deleteResult = await Collection.DeleteManyAsync(
                        CreateEntitiesFilter(entities)
                        );
                }

                if (deleteResult.DeletedCount < entitiesCount)
                {
                    ThrowOptimisticConcurrencyException();
                }
            }
        }

        public async override Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async override Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async override Task<List<TEntity>> GetPagedListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .OrderBy(sorting)
                .As<IMongoQueryable<TEntity>>()
                .PageBy<TEntity, IMongoQueryable<TEntity>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async override Task DeleteAsync(
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

        public async override Task<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(predicate)
                .SingleOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual IMongoQueryable<TEntity> GetMongoQueryable()
        {
            return ApplyDataFilters(SessionHandle != null ? Collection.AsQueryable(SessionHandle) : Collection.AsQueryable());
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

        protected virtual FilterDefinition<TEntity> CreateEntityFilter(TEntity entity, bool withConcurrencyStamp = false, string concurrencyStamp = null)
        {
            throw new NotImplementedException(
                $"{nameof(CreateEntityFilter)} is not implemented for MongoDB by default. It should be overriden and implemented by the deriving class!"
            );
        }

        protected virtual FilterDefinition<TEntity> CreateEntitiesFilter(IEnumerable<TEntity> entities, bool withConcurrencyStamp = false)
        {
            throw new NotImplementedException(
              $"{nameof(CreateEntitiesFilter)} is not implemented for MongoDB by default. It should be overriden and implemented by the deriving class!"
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

            var localEvents = generatesDomainEventsEntity.GetLocalEvents()?.ToArray();
            if (localEvents != null && localEvents.Any())
            {
                foreach (var localEvent in localEvents)
                {
                    await LocalEventBus.PublishAsync(localEvent.GetType(), localEvent);
                }

                generatesDomainEventsEntity.ClearLocalEvents();
            }

            var distributedEvents = generatesDomainEventsEntity.GetDistributedEvents()?.ToArray();
            if (distributedEvents != null && distributedEvents.Any())
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

        /// <summary>
        /// IMongoQueryable<TEntity>
        /// </summary>
        /// <returns></returns>
        public QueryableExecutionModel GetExecutionModel()
        {
            return GetMongoQueryable().GetExecutionModel();
        }

        /// <summary>
        /// IMongoQueryable<TEntity>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IAsyncCursor<TEntity> ToCursor(CancellationToken cancellationToken = new CancellationToken())
        {
            return GetMongoQueryable().ToCursor(cancellationToken);
        }

        /// <summary>
        /// IMongoQueryable<TEntity>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<IAsyncCursor<TEntity>> ToCursorAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return GetMongoQueryable().ToCursorAsync(cancellationToken);
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
            if (SessionHandle != null)
            {
                return await Collection
                    .Find(SessionHandle, RepositoryFilterer.CreateEntityFilter(id, true))
                    .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
            }

            return await Collection
                .Find(RepositoryFilterer.CreateEntityFilter(id, true))
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
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
            var entities = await GetMongoQueryable()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));

            await DeleteManyAsync(entities, autoSave, cancellationToken);
        }

        protected override FilterDefinition<TEntity> CreateEntityFilter(TEntity entity, bool withConcurrencyStamp = false, string concurrencyStamp = null)
        {
            return RepositoryFilterer.CreateEntityFilter(entity, withConcurrencyStamp, concurrencyStamp);
        }

        protected override FilterDefinition<TEntity> CreateEntitiesFilter(IEnumerable<TEntity> entities, bool withConcurrencyStamp = false)
        {
            return RepositoryFilterer.CreateEntitiesFilter(entities, withConcurrencyStamp);
        }
    }
}
