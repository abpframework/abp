using JetBrains.Annotations;
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

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public class MemoryDbRepository<TMemoryDbContext, TEntity> : RepositoryBase<TEntity>, IMemoryDbRepository<TEntity>
        where TMemoryDbContext : MemoryDbContext
        where TEntity : class, IEntity
    {
        //TODO: Add dbcontext just like mongodb implementation!

        public virtual IMemoryDatabaseCollection<TEntity> Collection => Database.Collection<TEntity>();

        public virtual IMemoryDatabase Database => DatabaseProvider.GetDatabase();

        protected IMemoryDatabaseProvider<TMemoryDbContext> DatabaseProvider { get; }

        public ILocalEventBus LocalEventBus { get; set; }

        public IDistributedEventBus DistributedEventBus { get; set; }

        public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        public IAuditPropertySetter AuditPropertySetter { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        public MemoryDbRepository(IMemoryDatabaseProvider<TMemoryDbContext> databaseProvider)
        {
            DatabaseProvider = databaseProvider;

            LocalEventBus = NullLocalEventBus.Instance;
            DistributedEventBus = NullDistributedEventBus.Instance;
            EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return ApplyDataFilters(Collection.AsQueryable());
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

        protected virtual async Task TriggerEntityCreateEvents(TEntity entity)
        {
            await EntityChangeEventHelper.TriggerEntityCreatedEventOnUowCompletedAsync(entity);
            await EntityChangeEventHelper.TriggerEntityCreatingEventAsync(entity);
        }

        protected virtual async Task TriggerEntityUpdateEventsAsync(TEntity entity)
        {
            await EntityChangeEventHelper.TriggerEntityUpdatedEventOnUowCompletedAsync(entity);
            await EntityChangeEventHelper.TriggerEntityUpdatingEventAsync(entity);
        }

        protected virtual async Task TriggerEntityDeleteEventsAsync(TEntity entity)
        {
            await EntityChangeEventHelper.TriggerEntityDeletedEventOnUowCompletedAsync(entity);
            await EntityChangeEventHelper.TriggerEntityDeletingEventAsync(entity);
        }

        protected virtual async Task ApplyAbpConceptsForAddedEntityAsync(TEntity entity)
        {
            CheckAndSetId(entity);
            SetCreationAuditProperties(entity);
            await TriggerEntityCreateEvents(entity);
            await TriggerDomainEventsAsync(entity);
        }

        protected virtual async Task ApplyAbpConceptsForDeletedEntityAsync(TEntity entity)
        {
            SetDeletionAuditProperties(entity);
            await TriggerEntityDeleteEventsAsync(entity);
            await TriggerDomainEventsAsync(entity);
        }

        public override Task<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(GetQueryable().Where(predicate).SingleOrDefault());
        }

        public async override Task DeleteAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            var entities = GetQueryable().Where(predicate).ToList();
            foreach (var entity in entities)
            {
                await DeleteAsync(entity, autoSave, cancellationToken);
            }
        }

        public async override Task<TEntity> InsertAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            await ApplyAbpConceptsForAddedEntityAsync(entity);

            Collection.Add(entity);

            return entity;
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

            Collection.Update(entity);

            return entity;
        }

        public async override Task DeleteAsync(
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default)
        {
            await ApplyAbpConceptsForDeletedEntityAsync(entity);

            if (entity is ISoftDelete softDeleteEntity && !IsHardDeleted(entity))
            {
                softDeleteEntity.IsDeleted = true;
                Collection.Update(entity);
            }
            else
            {
                Collection.Remove(entity);
            }
        }

        public override Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(GetQueryable().ToList());
        }

        public override Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(GetQueryable().LongCount());
        }

        public override Task<List<TEntity>> GetPagedListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(GetQueryable()
                .OrderBy(sorting)
                .PageBy(skipCount, maxResultCount)
                .ToList());
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

        public override Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            SetIdIfNeeded(entity);
            return base.InsertAsync(entity, autoSave, cancellationToken);
        }

        protected virtual void SetIdIfNeeded(TEntity entity)
        {
            if (typeof(TKey) == typeof(int) ||
                typeof(TKey) == typeof(long) ||
                typeof(TKey) == typeof(Guid))
            {
                if (EntityHelper.HasDefaultId(entity))
                {
                    EntityHelper.TrySetId(entity, () => Database.GenerateNextId<TEntity, TKey>());
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

        public virtual Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(GetQueryable().FirstOrDefault(e => e.Id.Equals(id)));
        }

        public virtual async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            await DeleteAsync(x => x.Id.Equals(id), autoSave, cancellationToken);
        }

        public virtual async Task DeleteManyAsync([NotNull] IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = await AsyncExecuter.ToListAsync(GetQueryable().Where(x => ids.Contains(x.Id)));
            DeleteManyAsync(entities, autoSave, cancellationToken);
        }
    }
}
