using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        protected override IQueryable<TEntity> GetQueryable()
        {
            return ApplyDataFilters(Collection.AsQueryable());
        }

        public override Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = Collection.AsQueryable().Where(predicate).ToList();
            foreach (var entity in entities)
            {
                Collection.Remove(entity);
            }

            return Task.CompletedTask;
        }

        public override Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Collection.Add(entity);
            return Task.FromResult(entity);
        }

        public override Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Collection.Update(entity);
            return Task.FromResult(entity);
        }

        public override Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            Collection.Remove(entity);
            return Task.CompletedTask;
        }

        public override Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Collection.ToList());
        }

        public override Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Collection.LongCount());
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
            var entity = await FindAsync(id, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return;
            }

            await DeleteAsync(entity, autoSave, cancellationToken);
        }
    }
}