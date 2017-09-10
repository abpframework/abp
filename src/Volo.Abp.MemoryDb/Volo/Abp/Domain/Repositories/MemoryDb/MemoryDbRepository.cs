using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MemoryDb;

namespace Volo.Abp.Domain.Repositories.MemoryDb
{
    public class MemoryDbRepository<TMemoryDbContext, TEntity> : MemoryDbRepository<TMemoryDbContext, TEntity, Guid>, IMemoryDbRepository<TEntity>
        where TMemoryDbContext : MemoryDbContext
        where TEntity : class, IEntity<Guid>
    {
        public MemoryDbRepository(IMemoryDatabaseProvider<TMemoryDbContext> databaseProvider)
            : base(databaseProvider)
        {
        }
    }

    public class MemoryDbRepository<TMemoryDbContext, TEntity, TPrimaryKey> : QueryableRepositoryBase<TEntity, TPrimaryKey>, IMemoryDbRepository<TEntity, TPrimaryKey> 
        where TMemoryDbContext : MemoryDbContext
        where TEntity : class, IEntity<TPrimaryKey>
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
            SetIdIfNeeded(entity);
            Collection.Add(entity);
            return entity;
        }

        private void SetIdIfNeeded(TEntity entity)
        {
            if (typeof(TPrimaryKey) == typeof(int) || typeof(TPrimaryKey) == typeof(long) || typeof(TPrimaryKey) == typeof(Guid))
            {
                if (entity.IsTransient())
                {
                    entity.Id = Database.GenerateNextId<TEntity, TPrimaryKey>();
                }
            }
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
            return Collection.AsQueryable();
        }
    }
}