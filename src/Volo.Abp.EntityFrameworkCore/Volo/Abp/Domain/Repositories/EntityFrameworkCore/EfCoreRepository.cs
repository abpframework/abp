using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Domain.Repositories.EntityFrameworkCore
{
    public class EfCoreRepository<TDbContext, TEntity> : RepositoryBase<TEntity>, IEfCoreRepository<TEntity>
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity
    {
        public virtual DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        DbContext IEfCoreRepository<TEntity>.DbContext => DbContext.As<DbContext>();

        protected virtual TDbContext DbContext => _dbContextProvider.GetDbContext();

        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
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
                await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }

            return savedEntity;
        }

        public override TEntity Update(TEntity entity, bool autoSave = false)
        {
            DbContext.Attach(entity);

            var updatedEntity = DbContext.Update(entity).Entity;

            if (autoSave)
            {
                DbContext.SaveChanges();
            }

            return updatedEntity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbContext.Attach(entity);

            var updatedEntity = DbContext.Update(entity).Entity;

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }

            return updatedEntity;
        }

        public override void Delete(TEntity entity, bool autoSave = false)
        {
            DbSet.Remove(entity);

            if (autoSave)
            {
                DbContext.SaveChanges();
            }
        }

        public override async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return DbSet.AsQueryable();
        }

        public override void Delete(Expression<Func<TEntity, bool>> predicate, bool autoSave = false)
        {
            base.Delete(predicate, autoSave);

            if (autoSave)
            {
                DbContext.SaveChanges();
            }
        }

        public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = await GetQueryable().Where(predicate).ToListAsync(GetCancellationToken(cancellationToken));
            foreach (var entity in entities)
            {
                DbSet.Remove(entity);
            }

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public virtual async Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await DbContext.Entry(entity).Collection(propertyExpression).LoadAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await DbContext.Entry(entity).Reference(propertyExpression).LoadAsync(GetCancellationToken(cancellationToken));
        }
    }

    public class EfCoreRepository<TDbContext, TEntity, TKey> : EfCoreRepository<TDbContext, TEntity>, 
        IEfCoreRepository<TEntity, TKey>,
        ISupportsExplicitLoading<TEntity, TKey>

        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity<TKey>
    {
        public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {

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

        public virtual async Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, includeDetails, GetCancellationToken(cancellationToken));

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual TEntity Find(TKey id, bool includeDetails = true)
        {
            return includeDetails
                ? IncludeDetails(DbSet).FirstOrDefault(EntityHelper.CreateEqualityExpressionForId<TEntity, TKey>(id))
                : DbSet.Find(id);
        }

        public virtual async Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await IncludeDetails(DbSet).FirstOrDefaultAsync(EntityHelper.CreateEqualityExpressionForId<TEntity, TKey>(id), GetCancellationToken(cancellationToken))
                : await DbSet.FindAsync(new object[] { id }, GetCancellationToken(cancellationToken));
        }

        public virtual void Delete(TKey id, bool autoSave = false)
        {
            var entity = Find(id, includeDetails: false);
            if (entity == null)
            {
                return;
            }

            Delete(entity, autoSave);
        }

        public virtual async Task DeleteAsync(TKey id, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, includeDetails: false, cancellationToken: cancellationToken);
            if (entity == null)
            {
                return;
            }

            await DeleteAsync(entity, autoSave, cancellationToken);
        }
    }
}
