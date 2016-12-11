using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Repositories.EntityFrameworkCore
{
    //TODO: Override async versions

    public class EfCoreRepository<TDbContext, TEntity, TPrimaryKey> : RepositoryBase<TEntity, TPrimaryKey>
        where TDbContext : AbpDbContext<TDbContext>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected virtual TDbContext DbContext { get; }

        protected virtual DbSet<TEntity> DbSet => DbContext.Set<TEntity>();
        
        public EfCoreRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public override List<TEntity> GetAllList()
        {
            return GetQueryable().ToList();
        }

        public override Task<List<TEntity>> GetAllListAsync()
        {
            return GetQueryable().ToListAsync();
        }

        public override TEntity Get(TPrimaryKey id)
        {
            var entity = FirstOrDefault(id);
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

        public override TEntity Insert(TEntity entity)
        {
            return DbSet.Add(entity).Entity;
        }

        public override TEntity Update(TEntity entity)
        {
            return DbSet.Update(entity).Entity;
        }

        public override void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = FirstOrDefault(id);
            if (entity == null)
            {
                return;
            }

            Delete(entity);
        }

        public override int Count()
        {
            return GetQueryable().Count();
        }

        protected virtual IQueryable<TEntity> GetQueryable()
        {
            return DbSet;
        }
    }
}
