using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nito.AsyncEx;
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
        public virtual DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        DbContext IEfCoreRepository<TEntity>.DbContext => DbContext.As<DbContext>();

        protected virtual TDbContext DbContext => _dbContextProvider.GetDbContext();

        protected virtual AbpEntityOptions<TEntity> AbpEntityOptions => _entityOptionsLazy.Value;

        private readonly IDbContextProvider<TDbContext> _dbContextProvider;
        private readonly Lazy<AbpEntityOptions<TEntity>> _entityOptionsLazy;

        public virtual IGuidGenerator GuidGenerator { get; set; }

        public IEfCoreBulkOperationProvider BulkOperationProvider { get; set; }

        public EfCoreRepository(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            GuidGenerator = SimpleGuidGenerator.Instance;

            _entityOptionsLazy = new Lazy<AbpEntityOptions<TEntity>>(
                () => ServiceProvider
                          .GetRequiredService<IOptions<AbpEntityOptions>>()
                          .Value
                          .GetOrNull<TEntity>() ?? AbpEntityOptions<TEntity>.Empty
            );
        }

        public async override Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            CheckAndSetId(entity);

            var savedEntity = DbSet.Add(entity).Entity;

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }

            return savedEntity;
        }

        public override async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                CheckAndSetId(entity);
            }

            if (BulkOperationProvider != null)
            {
                await BulkOperationProvider.InsertManyAsync<TDbContext, TEntity>(
                    this,
                    entities,
                    autoSave,
                    cancellationToken
                );
                return;
            }

            await DbSet.AddRangeAsync(entities);

            if (autoSave)
            {
                await DbContext.SaveChangesAsync();
            }
        }

        public async override Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbContext.Attach(entity);

            var updatedEntity = DbContext.Update(entity).Entity;

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }

            return updatedEntity;
        }

        public override async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (BulkOperationProvider != null)
            {
                await BulkOperationProvider.UpdateManyAsync<TDbContext, TEntity>(
                    this,
                    entities,
                    autoSave,
                    cancellationToken
                    );

                return;
            }

            DbSet.UpdateRange(entities);

            if (autoSave)
            {
                await DbContext.SaveChangesAsync();
            }
        }

        public async override Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }

        public override async Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            if (BulkOperationProvider != null)
            {
                await BulkOperationProvider.DeleteManyAsync<TDbContext, TEntity>(
                    this,
                    entities,
                    autoSave,
                    cancellationToken);

                return;
            }

            DbSet.RemoveRange(entities);

            if (autoSave)
            {
                await DbContext.SaveChangesAsync();
            }
        }

        public async override Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await WithDetails().ToListAsync(GetCancellationToken(cancellationToken))
                : await DbSet.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async override Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public async override Task<List<TEntity>> GetPagedListAsync(
            int skipCount,
            int maxResultCount,
            string sorting,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var queryable = includeDetails ? WithDetails() : DbSet;

            return await queryable
                .OrderBy(sorting)
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        protected override IQueryable<TEntity> GetQueryable()
        {
            return DbSet.AsQueryable();
        }

        protected override Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return DbContext.SaveChangesAsync(cancellationToken);
        }

        public async override Task<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return includeDetails
                ? await WithDetails()
                    .Where(predicate)
                    .SingleOrDefaultAsync(GetCancellationToken(cancellationToken))
                : await DbSet
                    .Where(predicate)
                    .SingleOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public async override Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = await GetQueryable()
                .Where(predicate)
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var entity in entities)
            {
                DbSet.Remove(entity);
            }

            if (autoSave)
            {
                await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
            }
        }

        public virtual async Task EnsureCollectionLoadedAsync<TProperty>(
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            CancellationToken cancellationToken = default)
            where TProperty : class
        {
            await DbContext
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
            await DbContext
                .Entry(entity)
                .Reference(propertyExpression)
                .LoadAsync(GetCancellationToken(cancellationToken));
        }

        public override IQueryable<TEntity> WithDetails()
        {
            if (AbpEntityOptions.DefaultWithDetailsFunc == null)
            {
                return base.WithDetails();
            }

            return AbpEntityOptions.DefaultWithDetailsFunc(GetQueryable());
        }

        public override IQueryable<TEntity> WithDetails(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetQueryable();

            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

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
                ? await WithDetails().FirstOrDefaultAsync(e => e.Id.Equals(id), GetCancellationToken(cancellationToken))
                : await DbSet.FindAsync(new object[] { id }, GetCancellationToken(cancellationToken));
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

        public async virtual Task DeleteManyAsync([NotNull] IEnumerable<TKey> ids, bool autoSave = false, CancellationToken cancellationToken = default)
        {
            var entities = await DbSet.Where(x => ids.Contains(x.Id)).ToListAsync();

            await DeleteManyAsync(entities, autoSave, cancellationToken);
        }
    }
}
