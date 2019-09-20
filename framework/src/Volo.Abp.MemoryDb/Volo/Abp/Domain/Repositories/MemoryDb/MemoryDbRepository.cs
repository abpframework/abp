using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
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

        public MemoryDbRepository(IMemoryDatabaseProvider<TMemoryDbContext> databaseProvider)
        {
            DatabaseProvider = databaseProvider;
        }

        public override TEntity Insert(TEntity entity, bool autoSave = false)
        {
            Collection.Add(entity);
            return entity;
        }

        public override TEntity Update(TEntity entity, bool autoSave = false)
        {
            Collection.Update(entity);
            return entity;
        }

        public override void Delete(TEntity entity, bool autoSave = false)
        {
            Collection.Remove(entity);
        }

        public override List<TEntity> GetList(bool includeDetails = false)
        {
            return Collection.ToList();
        }

        public override long GetCount()
        {
            return Collection.Count();
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return ApplyDataFilters(Collection.AsQueryable());
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

        public override TEntity Insert(TEntity entity, bool autoSave = false)
        {
            SetIdIfNeeded(entity);
            return base.Insert(entity, autoSave);
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

        public virtual TEntity Find(TKey id, bool includeDetails = true)
        {
            return GetQueryable().FirstOrDefault(e => e.Id.Equals(id));
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

        public virtual Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Get(id, includeDetails));
        }

        public virtual Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Find(id, includeDetails));
        }

        public virtual void Delete(TKey id, bool autoSave = false)
        {
            var entity = Find(id);
            if (entity == null)
            {
                return;
            }

            Delete(entity);
        }

        public virtual Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Delete(id);
            return Task.CompletedTask;
        }
    }
}