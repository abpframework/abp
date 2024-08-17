using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.Domain.Repositories.EntityFrameworkCore;

public class EfCoreRepository<TDbContext, TEntity> : RepositoryBase<TEntity>, IEfCoreRepository<TEntity>
    where TDbContext : IEfCoreDbContext
    where TEntity : class, IEntity
{
    [Obsolete("Use GetDbContextAsync() method.")]
    protected virtual TDbContext DbContext => GetDbContext();

    [Obsolete("Use GetDbContextAsync() method.")]
    DbContext IEfCoreRepository<TEntity>.DbContext => (GetDbContext() as DbContext)!;

    async Task<DbContext> IEfCoreRepository<TEntity>.GetDbContextAsync()
    {
        return (await GetDbContextAsync() as DbContext)!;
    }

    [Obsolete("Use GetDbContextAsync() method.")]
    private TDbContext GetDbContext()
    {
        // Multi-tenancy unaware entities should always use the host connection string
        if (!EntityHelper.IsMultiTenant<TEntity>())
        {
            using (CurrentTenant.Change(null))
            {
                return _dbContextProvider.GetDbContext();
            }
        }

        return _dbContextProvider.GetDbContext();
    }

    protected virtual Task<TDbContext> GetDbContextAsync()
    {
        // Multi-tenancy unaware entities should always use the host connection string
        if (!EntityHelper.IsMultiTenant<TEntity>())
        {
            using (CurrentTenant.Change(null))
            {
                return _dbContextProvider.GetDbContextAsync();
            }
        }

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

    protected async Task<IDbConnection> GetDbConnectionAsync()
    {
        return (await GetDbContextAsync()).Database.GetDbConnection();
    }

    protected async Task<IDbTransaction?> GetDbTransactionAsync()
    {
        return (await GetDbContextAsync()).Database.CurrentTransaction?.GetDbTransaction();
    }

    protected virtual AbpEntityOptions<TEntity> AbpEntityOptions => _entityOptionsLazy.Value;

    private readonly IDbContextProvider<TDbContext> _dbContextProvider;
    private readonly Lazy<AbpEntityOptions<TEntity>> _entityOptionsLazy;

    public virtual IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

    public IEfCoreBulkOperationProvider? BulkOperationProvider => LazyServiceProvider.LazyGetService<IEfCoreBulkOperationProvider>();

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

    public async override Task<TEntity> InsertAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
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

    public async override Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entityArray = entities.ToArray();
        if (entityArray.IsNullOrEmpty())
        {
            return;
        }

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

    public async override Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        if (dbContext.Set<TEntity>().Local.All(e => e != entity))
        {
            dbContext.Set<TEntity>().Attach(entity);
            dbContext.Update(entity);
        }

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }

        return entity;
    }

    public async override Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entityArray = entities.ToArray();
        if (entityArray.IsNullOrEmpty())
        {
            return;
        }

        cancellationToken = GetCancellationToken(cancellationToken);

        if (BulkOperationProvider != null)
        {
            await BulkOperationProvider.UpdateManyAsync<TDbContext, TEntity>(
                this,
                entityArray,
                autoSave,
                GetCancellationToken(cancellationToken)
                );

            return;
        }

        var dbContext = await GetDbContextAsync();

        dbContext.Set<TEntity>().UpdateRange(entityArray);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async override Task DeleteAsync(TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        dbContext.Set<TEntity>().Remove(entity);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }
    }

    public async override Task DeleteManyAsync(IEnumerable<TEntity> entities, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var entityArray = entities.ToArray();
        if (entityArray.IsNullOrEmpty())
        {
            return;
        }

        cancellationToken = GetCancellationToken(cancellationToken);

        if (BulkOperationProvider != null)
        {
            await BulkOperationProvider.DeleteManyAsync<TDbContext, TEntity>(
                this,
                entityArray,
                autoSave,
                cancellationToken
            );

            return;
        }

        var dbContext = await GetDbContextAsync();

        dbContext.RemoveRange(entityArray.Select(x => x));

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async override Task<List<TEntity>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await WithDetailsAsync()).ToListAsync(GetCancellationToken(cancellationToken))
            : await (await GetQueryableAsync()).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async override Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await WithDetailsAsync()).Where(predicate).ToListAsync(GetCancellationToken(cancellationToken))
            : await (await GetQueryableAsync()).Where(predicate).ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async override Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).LongCountAsync(GetCancellationToken(cancellationToken));
    }

    public async override Task<List<TEntity>> GetPagedListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
    {
        var queryable = includeDetails
            ? await WithDetailsAsync()
            : await GetQueryableAsync();

        return await queryable
            .OrderByIf<TEntity, IQueryable<TEntity>>(!sorting.IsNullOrWhiteSpace(), sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    [Obsolete("Use GetQueryableAsync method.")]
    protected override IQueryable<TEntity> GetQueryable()
    {
        return DbSet.AsQueryable().AsNoTrackingIf(!ShouldTrackingEntityChange());
    }

    public async override Task<IQueryable<TEntity>> GetQueryableAsync()
    {
        return (await GetDbSetAsync()).AsQueryable().AsNoTrackingIf(!ShouldTrackingEntityChange());
    }

    protected async override Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await (await GetDbContextAsync()).SaveChangesAsync(cancellationToken);
    }

    public async override Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool includeDetails = true,
        CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await WithDetailsAsync())
                .Where(predicate)
                .SingleOrDefaultAsync(GetCancellationToken(cancellationToken))
            : await (await GetQueryableAsync())
                .Where(predicate)
                .SingleOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public async override Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<TEntity>();

        var entities = await dbSet
            .Where(predicate)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(entities, autoSave, cancellationToken);

        if (autoSave)
        {
            await dbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }
    }

    public async override Task DeleteDirectAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<TEntity>();
        await dbSet.Where(predicate).ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
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
        Expression<Func<TEntity, TProperty?>> propertyExpression,
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

    public async override Task<IQueryable<TEntity>> WithDetailsAsync()
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

    public async override Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
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
    ISupportsExplicitLoading<TEntity>

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

    public virtual async Task<TEntity?> FindAsync(TKey id, bool includeDetails = true, CancellationToken cancellationToken = default)
    {
        return includeDetails
            ? await (await WithDetailsAsync()).OrderBy(e => e.Id).FirstOrDefaultAsync(e => e.Id!.Equals(id), GetCancellationToken(cancellationToken))
            : !ShouldTrackingEntityChange()
                ? await (await GetQueryableAsync()).OrderBy(e => e.Id).FirstOrDefaultAsync(e => e.Id!.Equals(id), GetCancellationToken(cancellationToken))
                : await (await GetDbSetAsync()).FindAsync(new object[] {id!}, GetCancellationToken(cancellationToken));
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
