using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Repositories.EntityFrameworkCore
{
    //TODO: Override async versions and others

    public class EfCoreRepository<TDbContext, TEntity, TPrimaryKey> : QueryableRepositoryBase<TEntity, TPrimaryKey>
        where TDbContext : AbpDbContext<TDbContext>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected virtual TDbContext DbContext { get; }

        protected virtual DbSet<TEntity> DbSet => DbContext.Set<TEntity>();
        
        public EfCoreRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public override IQueryable<TEntity> GetQueryable()
        {
            return DbSet;
        }

        public override Task<List<TEntity>> GetListAsync()
        {
            return GetQueryable().ToListAsync();
        }

        public override async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var entity = await FirstOrDefaultAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public override TEntity FirstOrDefault(TPrimaryKey id)
        {
            return DbSet.Find(id);
        }

        public override Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return DbSet.FindAsync(id);
        }

        public override Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return GetQueryable().FirstOrDefaultAsync(predicate);
        }

        public override TEntity Insert(TEntity entity)
        {
            return DbSet.Add(entity).Entity;
        }

        public override TPrimaryKey InsertAndGetId(TEntity entity)
        {
            var insertedEntity = Insert(entity);
            DbContext.SaveChanges();
            return insertedEntity.Id;
        }

        public override async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            var insertedEntity = await InsertAsync(entity);
            await DbContext.SaveChangesAsync();
            return insertedEntity.Id;
        }

        public override TEntity Update(TEntity entity)
        {
            return DbSet.Update(entity).Entity;
        }

        public override void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public override Task<int> CountAsync()
        {
            return DbSet.CountAsync();
        }

        public override Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.CountAsync(predicate);
        }
    }
}
