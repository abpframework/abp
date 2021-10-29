using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Guids;
using Volo.Abp.MemoryDb;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public class MemoryDbRepository<TMemoryDbContext, TEntity> : RepositoryBase<TEntity>, IMemoryDbRepository<TEntity>
        where TMemoryDbContext : MemoryDbContext
        where TEntity : class, IEntity
    {
        //TODO: Add dbcontext just like mongodb implementation!

        [Obsolete("Use GetCollectionAsync method.")]
        public virtual IMemoryDatabaseCollection<TEntity> Collection => Database.Collection<TEntity>();

        public async Task<IMemoryDatabaseCollection<TEntity>> GetCollectionAsync()
        {
            return (await GetDatabaseAsync()).Collection<TEntity>();
        }

        [Obsolete("Use GetDatabaseAsync method.")]
        public virtual IMemoryDatabase Database => DatabaseProvider.GetDatabase();

        public Task<IMemoryDatabase> GetDatabaseAsync()
        {
            return DatabaseProvider.GetDatabaseAsync();
        }

        protected IMemoryDatabaseProvider<TMemoryDbContext> DatabaseProvider { get; }

        public ILocalEventBus LocalEventBus => LazyServiceProvider.LazyGetService<ILocalEventBus>(NullLocalEventBus.Instance);

        public IDistributedEventBus DistributedEventBus => LazyServiceProvider.LazyGetService<IDistributedEventBus>(NullDistributedEventBus.Instance);

        public IEntityChangeEventHelper EntityChangeEventHelper => LazyServiceProvider.LazyGetService<IEntityChangeEventHelper>(NullEntityChangeEventHelper.Instance);

        public IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

        public IAuditPropertySetter AuditPropertySetter => LazyServiceProvider.LazyGetRequiredService<IAuditPropertySetter>();

        public MemoryDbRepository(IMemoryDatabaseProvider<TMemoryDbContext> databaseProvider)
        {
            DatabaseProvider = databaseProvider;
        }

        [Obsolete("This method will be removed in future versions.")]
        protected override IQueryable<TEntity> GetQueryable()
        {
            return ApplyDataFilters(Collection.AsQueryable());
        }

        public override async Task<IQueryable<TEntity>> GetQueryableAsync()
        {
            return ApplyDataFilters((await GetCollectionAsync()).AsQueryable());
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
                            distributedEvent.EventOrder,
                            useOutbox: true
                        )
                    );
                }

                generatesDomainEventsEntity.ClearDistributedEvents();
            }
        }

        protected virtual bool IsHardDeleted(TEntity entity)
        {
            if (!(UnitOfWorkManager?.Current?.Items.GetOrDefault(UnitOfWorkItemNames.HardDeletedEntities) is HashSet<IEntity> hardDeletedEntities))
            {
                return false;
            }

            return hardDeletedEntities.Contains(entity);
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

        protected virtual void TriggerEntityCreateEvents(TEntity entity)
        {
            EntityChangeEventHelper.PublishEntityCreatingEvent(entity);
            EntityChangeEventHelper.PublishEntityCreatedEvent(entity);
        }

        protected virtual void TriggerEntityUpdateEvents(TEntity entity)
        {
            EntityChangeEventHelper.PublishEntityUpdatingEvent(entity);
            EntityChangeEventHelper.PublishEntityUpdatedEvent(entity);
        }

        protected virtual void TriggerEntityDeleteEvents(TEntity entity)
        {
            EntityChangeEventHelper.PublishEntityDeletingEvent(entity);
            EntityChangeEventHelper.PublishEntityDeletedEvent(entity);
        }

        protected virtual void ApplyAbpConceptsForAddedEntity(TEntity entity)
        {
            CheckAndSetId(entity);
            SetCreationAuditProperties(entity);
            TriggerEntityCreateEvents(entity);
            TriggerDomainEvents(entity);
        }

        protected virtual void ApplyAbpConceptsForDeletedEntity(TEntity entity)
        {
            SetDeletionAuditProperties(entity);
            TriggerEntityDeleteEvents(entity);
            TriggerDomainEvents(entity);
        }

        public override async Task<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return (await GetQueryableAsync()).Where(predicate).SingleOrDefault();
        }

        public override async Task DeleteAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            var entities = (await GetQueryableAsync()).Where(predicate).ToList();

            await DeleteManyAsync(entities, autoSave, cancellationToken);
        }

        public override async Task<TEntity> InsertAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            ApplyAbpConceptsForAddedEntity(entity);

            (await GetCollectionAsync()).Add(entity);

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
                TriggerEntityDeleteEvents(entity);
            }
            else
            {
                TriggerEntityUpdateEvents(entity);
            }

            TriggerDomainEvents(entity);

            (await GetCollectionAsync()).Update(entity);

            return entity;
        }

        public override async Task DeleteAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            ApplyAbpConceptsForDeletedEntity(entity);

            if (entity is ISoftDelete softDeleteEntity && !IsHardDeleted(entity))
            {
                softDeleteEntity.IsDeleted = true;
                (await GetCollectionAsync()).Update(entity);
            }
            else
            {
                (await GetCollectionAsync()).Remove(entity);
            }
        }

        public override async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return (await GetQueryableAsync()).ToList();
        }

        public override async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return (await GetQueryableAsync()).Where(predicate).ToList();
        }

        public override async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return (await GetQueryableAsync()).LongCount();
        }

        public override async Task<List<TEntity>> GetPagedListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return (await GetQueryableAsync())
                .OrderBy(sorting)
                .PageBy(skipCount, maxResultCount)
                .ToList();
        }

        public override async Task<List<TEntity>> GetPagedListAsync(
            Expression<Func<TEntity, bool>> predicate,
            int skipCount,
            int maxResultCount,
            string sorting,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return (await GetQueryableAsync())
                .Where(predicate)
                .OrderBy(sorting)
                .PageBy(skipCount, maxResultCount)
                .ToList();
        }
    }

    public class MemoryDbRepository<TMemoryDbContext, TEntity, TKey> : MemoryDbRepository<TMemoryDbContext, TEntity>, IMemoryDbRepository<TEntity, TKey>
        where TMemoryDbContext : MemoryDbContext
        where TEntity : class, IEntity<TKey>
    {
        public MemoryDbRepository(IMemoryDatabaseProvider<TMemoryDbContext> databaseProvider)
            : base(databaseProvider)
        {
        }

        public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await SetIdIfNeededAsync(entity);
            return await base.InsertAsync(entity, autoSave, cancellationToken);
        }

        protected virtual async Task SetIdIfNeededAsync(TEntity entity)
        {
            if (typeof(TKey) == typeof(int) ||
                typeof(TKey) == typeof(long) ||
                typeof(TKey) == typeof(Guid))
            {
                if (EntityHelper.HasDefaultId(entity))
                {
                    var nextId = (await GetDatabaseAsync()).GenerateNextId<TEntity, TKey>();
                    EntityHelper.TrySetId(entity, () => nextId);
                }
            }
        }

        public virtual async Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, includeDetails, cancellationToken);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual async Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return (await GetQueryableAsync()).FirstOrDefault(e => e.Id.Equals(id));
        }

        public virtual async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(x => x.Id.Equals(id), autoSave, cancellationToken);
        }

        public virtual async Task DeleteManyAsync(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = await AsyncExecuter.ToListAsync((await GetQueryableAsync()).Where(x => ids.Contains(x.Id)), cancellationToken);
            await DeleteManyAsync(entities, autoSave, cancellationToken);
        }
    }
}
