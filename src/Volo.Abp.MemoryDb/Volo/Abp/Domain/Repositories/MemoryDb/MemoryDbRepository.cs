using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MemoryDb;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public class MemoryDbRepository<TMemoryDbContext, TEntity> : QueryableRepositoryBase<TEntity>, IMemoryDbRepository<TEntity>
        where TMemoryDbContext : MemoryDbContext
        where TEntity : class, IEntity
    {
        public virtual List<TEntity> Collection => Database.Collection<TEntity>();

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

        public override TEntity Update(TEntity entity)
        {
            return entity;
        }

        public override void Delete(TEntity entity)
        {
            Collection.Remove(entity);
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return ApplyDataFilters(Collection.AsQueryable());
        }
    }

    public class MemoryDbRepository<TMemoryDbContext, TEntity, TPrimaryKey> : MemoryDbRepository<TMemoryDbContext, TEntity>, IMemoryDbRepository<TEntity, TPrimaryKey> 
        where TMemoryDbContext : MemoryDbContext
        where TEntity : class, IEntity<TPrimaryKey>
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

        private void SetIdIfNeeded(TEntity entity)
        {
            if (typeof(TPrimaryKey) == typeof(int) || typeof(TPrimaryKey) == typeof(long) || typeof(TPrimaryKey) == typeof(Guid))
            {
                if (EntityHelper.IsTransient(entity))
                {
                    entity.Id = Database.GenerateNextId<TEntity, TPrimaryKey>();
                }
            }
        }

        public virtual TEntity Find(TPrimaryKey id)
        {
            return GetQueryable().FirstOrDefault(EntityHelper.CreateEqualityExpressionForId<TEntity, TPrimaryKey>(id));
        }

        public virtual TEntity Get(TPrimaryKey id)
        {
            var entity = Find(id);

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual Task<TEntity> GetAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Get(id));
        }

        public virtual Task<TEntity> FindAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Find(id));
        }

        public virtual void Delete(TPrimaryKey id)
        {
            var entity = Find(id);
            if (entity == null)
            {
                return;
            }

            Delete(entity);
        }

        public virtual Task DeleteAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            Delete(id);
            return Task.CompletedTask;
        }
    }
}