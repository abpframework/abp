using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.Domain.Repositories.EntityFrameworkCore
{
    public class EfCoreRepository<TDbContext, TEntity> : RepositoryBase<TEntity>, IEfCoreRepository<TEntity>, IAsyncEnumerable<TEntity>
        where TDbContext : IEfCoreDbContext
        where TEntity : class, IEntity
    {
        [Obsolete("Use GetDbContextAsync() method.")]
        protected virtual TDbContext DbContext => _dbContextProvider.GetDbContext();

        [Obsolete("Use GetDbContextAsync() method.")]
        DbContext IEfCoreRepository<TEntity>.DbContext => DbContext.As<DbContext>();

        async Task<DbContext> IEfCoreRepository<TEntity>.GetDbContextAsync()
        {
            return await GetDbContextAsync() as DbContext;
        }

        protected virtual Task<TDbContext> GetDbContextAsync()
        {
            return _dbContextProvider.GetDbContextAsync();
        }

        [Obsolete("Use GetDbSetAsync() method.")]
        public virtual DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        Task<DbSet<TEntity>> IEfCoreRepository<TEntity>.GetDbSetAsync()
        {
            return GetDbSetAsync();
        }

        protected async Task<DbSet<TEntity>> GetDbSetAsync()
        {
            return (await GetDbContextAsync()).Set<TEntity>();
        }

        protected virtual AbpEntityOptions<TEntity> AbpEntityOptions => _entityOptionsLazy.Value;

        private readonly IDbContextProvider<TDbContext> _dbContextProvider;
        private readonly Lazy<AbpEntityOptions<TEntity>> _entityOptionsLazy;

        public virtual IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

        public IEfCoreBulkOperationProvider BulkOperationProvider => LazyServiceProvider.LazyGetService<IEfCoreBulkOperationProvider>();

        public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;

            _entityOptionsLazy = new Lazy<AbpEntityOptions<TEntity>>(
                () => ServiceProvider
                          .GetRequiredService<IOptions<AbpEntityOptions>>()
                          .Value
                          .GetOrNull<TEntity>() ?? AbpEntityOptions<TEntity>.Empty
            );
        }

        public override async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            CheckAndSetId(entity);

            var dbContext = await GetDbContextAsync();

            var savedEntity = (await dbContext.Set<TEntity>().AddAsync(entity, GetCancellationToken(cancellationToken))).Entity;

            if (autoSave)
            {
                await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }

            return savedEntity;
        }

        public override async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entityArray = entities.ToArray();
            var dbContext = await GetDbContextAsync();
            cancellationToken = GetCancellationToken(cancellationToken);

            foreach (var entity in entityArray)
            {
                CheckAndSetId(entity);
            }

            if (BulkOperationProvider != null)
            {
                await BulkOperationProvider.InsertManyAsync<TDbContext, TEntity>(
                    this,
                    entityArray,
                    autoSave,
                    GetCancellationToken(cancellationToken)
                );
                return;
            }

            await dbContext.Set<TEntity>().AddRangeAsync(entityArray, cancellationToken);

            if (autoSave)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            dbContext.Attach(entity);

            var updatedEntity = dbContext.Update(entity).Entity;

            if (autoSave)
            {
                await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }

            return updatedEntity;
        }

        public override async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);

            if (BulkOperationProvider != null)
            {
                await BulkOperationProvider.UpdateManyAsync<TDbContext, TEntity>(
                    this,
                    entities,
                    autoSave,
                    GetCancellationToken(cancellationToken)
                    );

                return;
            }

            var dbContext = await GetDbContextAsync();

            dbContext.Set<TEntity>().UpdateRange(entities);

            if (autoSave)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public override async Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            dbContext.Set<TEntity>().Remove(entity);

            if (autoSave)
            {
                await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }

        public override async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);

            if (BulkOperationProvider != null)
            {
                await BulkOperationProvider.DeleteManyAsync<TDbContext, TEntity>(
                    this,
                    entities,
                    autoSave,
                    cancellationToken
                );

                return;
            }

            var dbContext = await GetDbContextAsync();

            dbContext.RemoveRange(entities);

            if (autoSave)
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public override async Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await (await WithDetailsAsync()).ToListAsync(GetCancellationToken(cancellationToken))
                : await (await GetDbSetAsync()).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<List<TEntity>> GetPagedListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var queryable = includeDetails
                ? await WithDetailsAsync()
                : await GetDbSetAsync();

            return await queryable
                .OrderBy(sorting)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        [Obsolete("Use GetQueryableAsync method.")]
        protected override IQueryable<TEntity> GetQueryable()
        {
            return DbSet.AsQueryable();
        }

        public override async Task<IQueryable<TEntity>> GetQueryableAsync()
        {
            return (await GetDbSetAsync()).AsQueryable();
        }

        protected override async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await (await GetDbContextAsync()).SaveChangesAsync(cancellationToken);
        }

        public override async Task<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await (await WithDetailsAsync())
                    .Where(predicate)
                    .SingleOrDefaultAsync(GetCancellationToken(cancellationToken))
                : await (await GetDbSetAsync())
                    .Where(predicate)
                    .SingleOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var dbSet = dbContext.Set<TEntity>();

            var entities = await dbSet
                .Where(predicate)
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var entity in entities)
            {
                dbSet.Remove(entity);
            }

            if (autoSave)
            {
                await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }

        public virtual async Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await (await GetDbContextAsync())
                .Entry(entity)
                .Collection(propertyExpression)
                .LoadAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task EnsurePropertyLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await (await GetDbContextAsync())
                .Entry(entity)
                .Reference(propertyExpression)
                .LoadAsync(GetCancellationToken(cancellationToken));
        }

        [Obsolete("Use WithDetailsAsync")]
        public override IQueryable<TEntity> WithDetails()
        {
            if (AbpEntityOptions.DefaultWithDetailsFunc == null)
            {
                return base.WithDetails();
            }

            return AbpEntityOptions.DefaultWithDetailsFunc(GetQueryable());
        }

        public override async Task<IQueryable<TEntity>> WithDetailsAsync()
        {
            if (AbpEntityOptions.DefaultWithDetailsFunc == null)
            {
                return await base.WithDetailsAsync();
            }

            return AbpEntityOptions.DefaultWithDetailsFunc(await GetQueryableAsync());
        }

        [Obsolete("Use WithDetailsAsync method.")]
        public override IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return IncludeDetails(
                GetQueryable(),
                propertySelectors
            );
        }

        public override async Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return IncludeDetails(
                await GetQueryableAsync(),
                propertySelectors
            );
        }

        private static IQueryable<TEntity> IncludeDetails(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        [Obsolete("This method will be deleted in future versions.")]
        public IAsyncEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return DbSet.AsAsyncEnumerable().GetAsyncEnumerator(cancellationToken);
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

        public virtual async Task<TEntity> GetAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var entity = await FindAsync(id, includeDetails, GetCancellationToken(cancellationToken));

            if (entity == null)
            {
                throw new EntityNotFoundException(typeof(TEntity), id);
            }

            return entity;
        }

        public virtual async Task<TEntity> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await (await WithDetailsAsync()).FirstOrDefaultAsync(e => e.Id.Equals(id), GetCancellationToken(cancellationToken))
                : await (await GetDbSetAsync()).FindAsync(new object[] {id}, GetCancellationToken(cancellationToken));
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

        public virtual async Task DeleteManyAsync(IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);

            var entities = await (await GetDbSetAsync()).Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

            await DeleteManyAsync(entities, autoSave, cancellationToken);
        }
    }
}
