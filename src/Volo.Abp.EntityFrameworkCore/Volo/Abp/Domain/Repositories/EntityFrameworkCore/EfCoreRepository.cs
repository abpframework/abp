using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Domain.Repositories.EntityFrameworkCore
{
    public class EfCoreRepository<TDbContext, TEntity> : EfCoreRepository<TDbContext, TEntity, string>, IEfCoreRepository<TEntity>
        where TDbContext : AbpDbContext<TDbContext>
        where TEntity : class, IEntity<string>
    {
        public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }

    public class EfCoreRepository<TDbContext, TEntity, TPrimaryKey> : QueryableRepositoryBase<TEntity, TPrimaryKey>, IEfCoreRepository<TEntity, TPrimaryKey>
        where TDbContext : AbpDbContext<TDbContext>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public virtual DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        DbContext IEfCoreRepository<TEntity, TPrimaryKey>.DbContext => DbContext;

        protected virtual TDbContext DbContext => _dbContextProvider.GetDbContext();

        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return DbSet.AsQueryable();
        }

        public override Task<List<TEntity>> GetListAsync()
        {
            return GetQueryable().ToListAsync();
        }

        public override async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var entity = await FindAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        //TODO: Find by multiple primary key

        public override TEntity Find(TPrimaryKey id)
        {
            return DbSet.Find(id);
        }

        public override Task<TEntity> FindAsync(TPrimaryKey id)
        {
            return DbSet.FindAsync(id);
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
            await DbContext.SaveChangesAsync(true);
            return insertedEntity.Id;
        }

        public override TEntity Update(TEntity entity)
        {
            //TODO: This code is got from UserStore.UpdateAsync and revised Update method based on that, but we should be sure that it's valid
            //Context.Attach(user);
            //user.ConcurrencyStamp = Guid.NewGuid().ToString();
            //Context.Update(user);
            
            DbContext.Attach(entity); //TODO: What is different for DbSet.Attach(entity)?

            if (entity is IHasConcurrencyStamp)
            {
                (entity as IHasConcurrencyStamp).ConcurrencyStamp = Guid.NewGuid().ToString(); //TODO: Use IGuidGenerator!
            }

            return DbContext.Update(entity).Entity; //TODO: or DbSet.Update(entity) ?
        }

        public override void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public override Task<int> CountAsync()
        {
            return DbSet.CountAsync();
        }
    }
}
