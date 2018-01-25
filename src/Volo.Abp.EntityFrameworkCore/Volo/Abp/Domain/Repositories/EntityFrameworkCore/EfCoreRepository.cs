using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Threading;

namespace Volo.Abp.Domain.Repositories.EntityFrameworkCore
{
    //public class EfCoreRepository<TDbContext, TEntity> : EfCoreRepository<TDbContext, TEntity, Guid>, IEfCoreRepository<TEntity>
    //    where TDbContext : IEfCoreDbContext
    //    where TEntity : class, IEntity<Guid>
    //{
    //    public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
    //        : base(dbContextProvider)
    //    {
    //    }
    //}

    public class EfCoreRepository<TDbContext, TEntity, TPrimaryKey> : QueryableRepositoryBase<TEntity, TPrimaryKey>, 
        IEfCoreRepository<TEntity, TPrimaryKey>,
        ISupportsExplicitLoading<TEntity, TPrimaryKey>

        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public virtual DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        DbContext IEfCoreRepository<TEntity, TPrimaryKey>.DbContext => DbContext.As<DbContext>();

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

        public override Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return DbSet.ToListAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
        }

        public override async Task<TEntity> GetAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, CancellationTokenProvider.FallbackToProvider(cancellationToken));

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public override TEntity Find(TPrimaryKey id)
        {
            return DbSet.Find(id);
        }

        public override Task<TEntity> FindAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            return DbSet.FindAsync(new object[] { id }, CancellationTokenProvider.FallbackToProvider(cancellationToken));
        }

        public override TEntity Insert(TEntity entity, bool autoSave = false)
        {
            var savedEntity = DbSet.Add(entity).Entity;

            if (autoSave)
            {
                DbContext.SaveChanges();
            }

            return savedEntity;
        }

        public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var savedEntity = DbSet.Add(entity).Entity;

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
            }

            return savedEntity;
        }

        public override TEntity Update(TEntity entity)
        {
            DbContext.Attach(entity);
            return DbContext.Update(entity).Entity;
        }

        public override void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var entities = await GetQueryable().Where(predicate).ToListAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
            foreach (var entity in entities)
            {
                DbSet.Remove(entity);
            }
        }

        public override Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return GetQueryable().LongCountAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
        }

        public virtual Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            return DbContext.Entry(entity).Collection(propertyExpression).LoadAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
        }

        public virtual Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            return DbContext.Entry(entity).Reference(propertyExpression).LoadAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
        }
    }
}
