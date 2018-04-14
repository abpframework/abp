using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.MongoDB;
using Volo.Abp.MultiTenancy;

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

        public IEventBus EventBus { get; set; }

        public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        public MongoDbRepository(IMongoDbContextProvider<TMongoDbContext> dbContextProvider)
        {
            DbContextProvider = dbContextProvider;

            EventBus = NullEventBus.Instance;
            EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
        }

        public override TEntity Insert(TEntity entity, bool autoSave = false)
        {
            /* EntityCreatedEvent (OnUowCompleted) is triggered as the first because it should be
             * triggered before other events triggered inside an EntityCreating event handler.
             * This is also true for other "ed" & "ing" events.
             */

            ApplyAbpConceptsForAddedEntity(entity);

            Collection.InsertOne(entity);

            return entity;
        }

        public override async Task<TEntity> InsertAsync(
            TEntity entity, 
            bool autoSave = false, 
            CancellationToken cancellationToken = default)
        {
            ApplyAbpConceptsForAddedEntity(entity); //TODO: async?

            await Collection.InsertOneAsync(
                entity,
                cancellationToken: GetCancellationToken(cancellationToken)
            );

            return entity;
        }

        public override TEntity Update(TEntity entity, bool autoSave = false)
        {
            ApplyAbpConceptsForUpdatedEntity(entity);

            Collection.ReplaceOne(
                CreateEntityFilter(entity),
                entity
            );

            return entity;
        }

        public override async Task<TEntity> UpdateAsync(
            TEntity entity, 
            bool autoSave = false, 
            CancellationToken cancellationToken = default)
        {
            ApplyAbpConceptsForUpdatedEntity(entity);

            await Collection.ReplaceOneAsync(
                CreateEntityFilter(entity),
                entity,
                cancellationToken: GetCancellationToken(cancellationToken)
            );

            return entity;
        }

        public override void Delete(TEntity entity, bool autoSave = false)
        {
            ApplyAbpConceptsForDeletedEntity(entity);

            Collection.DeleteOne(
                CreateEntityFilter(entity)
            );
        }

        public override async Task DeleteAsync(
            TEntity entity, 
            bool autoSave = false, 
            CancellationToken cancellationToken = default)
        {
            ApplyAbpConceptsForDeletedEntity(entity);

            await Collection.DeleteOneAsync(
                CreateEntityFilter(entity),
                GetCancellationToken(cancellationToken)
            );
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
                await DeleteAsync(entity, autoSave, GetCancellationToken(cancellationToken));
            }
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return ApplyDataFilters(
                Collection.AsQueryable()
            );
        }

        protected virtual IMongoQueryable<TEntity> GetMongoQueryable()
        {
            return ApplyDataFilters(
                Collection.AsQueryable()
            );
        }

        protected virtual FilterDefinition<TEntity> CreateEntityFilter(TEntity entity)
        {
            throw new NotImplementedException(
                $"{nameof(CreateEntityFilter)} is not implemented for MongoDB by default. " +
                $"It should be overrided and implemented by the deriving class!"
            );
        }

        protected virtual void ApplyAbpConceptsForAddedEntity(TEntity entity)
        {
            EntityChangeEventHelper.TriggerEntityCreatedEventOnUowCompleted(entity);
            EntityChangeEventHelper.TriggerEntityCreatingEvent(entity);
            TriggerDomainEvents(entity);
        }

        protected virtual void ApplyAbpConceptsForUpdatedEntity(TEntity entity)
        {
            EntityChangeEventHelper.TriggerEntityUpdatedEventOnUowCompleted(entity);
            EntityChangeEventHelper.TriggerEntityUpdatingEvent(entity);
            TriggerDomainEvents(entity);
        }

        private void ApplyAbpConceptsForDeletedEntity(TEntity entity)
        {
            EntityChangeEventHelper.TriggerEntityDeletedEventOnUowCompleted(entity);
            EntityChangeEventHelper.TriggerEntityDeletingEvent(entity);
            TriggerDomainEvents(entity);
        }

        protected virtual void TriggerDomainEvents(object entity) //TODO: TriggerDomainEventsAsync..?
        {
            var generatesDomainEventsEntity = entity as IGeneratesDomainEvents;
            if (generatesDomainEventsEntity == null)
            {
                return;
            }

            var entityEvents = generatesDomainEventsEntity.GetDomainEvents().ToArray();
            if (entityEvents.IsNullOrEmpty())
            {
                return;
            }

            foreach (var entityEvent in entityEvents)
            {
                EventBus.Trigger(entityEvent.GetType(), entityEvent);
            }

            generatesDomainEventsEntity.ClearDomainEvents();
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

        protected override FilterDefinition<TEntity> CreateEntityFilter(TEntity entity)
        {
            return Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
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
                filters.Add(Builders<TEntity>.Filter.Eq(e => ((ISoftDelete) e).IsDeleted, false));
            }

            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                var tenantId = CurrentTenant.Id;
                filters.Add(Builders<TEntity>.Filter.Eq(e => ((IMultiTenant) e).TenantId, tenantId));
            }
        }
    }
}