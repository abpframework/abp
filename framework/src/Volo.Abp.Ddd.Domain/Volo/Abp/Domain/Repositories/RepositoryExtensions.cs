using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Repositories;

public static class RepositoryExtensions
{
    public static async Task EnsureCollectionLoadedAsync<TEntity, TKey, TProperty>(
        this IBasicRepository<TEntity, TKey> repository,
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
        CancellationToken cancellationToken = default
    )
        where TEntity : class, IEntity<TKey>
        where TProperty : class
    {
        var repo = ProxyHelper.UnProxy(repository) as ISupportsExplicitLoading<TEntity, TKey>;
        if (repo != null)
        {
            await repo.EnsureCollectionLoadedAsync(entity, propertyExpression, cancellationToken);
        }
    }

    public static async Task EnsurePropertyLoadedAsync<TEntity, TKey, TProperty>(
        this IBasicRepository<TEntity, TKey> repository,
        TEntity entity,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        CancellationToken cancellationToken = default
    )
        where TEntity : class, IEntity<TKey>
        where TProperty : class
    {
        var repo = ProxyHelper.UnProxy(repository) as ISupportsExplicitLoading<TEntity, TKey>;
        if (repo != null)
        {
            await repo.EnsurePropertyLoadedAsync(entity, propertyExpression, cancellationToken);
        }
    }

    public static async Task HardDeleteAsync<TEntity>(
        this IRepository<TEntity> repository,
        Expression<Func<TEntity, bool>> predicate,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
        where TEntity : class, IEntity, ISoftDelete
    {
        var uowManager = repository.GetUnitOfWorkManager();

        if (uowManager.Current == null)
        {
            using (var uow = uowManager.Begin())
            {
                await HardDeleteWithUnitOfWorkAsync(repository, predicate, autoSave, cancellationToken, uowManager.Current);
                await uow.CompleteAsync(cancellationToken);
            }
        }
        else
        {
            await HardDeleteWithUnitOfWorkAsync(repository, predicate, autoSave, cancellationToken, uowManager.Current);
        }
    }

    public static async Task HardDeleteAsync<TEntity>(
        this IBasicRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
        where TEntity : class, IEntity, ISoftDelete
    {
        var uowManager = repository.GetUnitOfWorkManager();

        if (uowManager.Current == null)
        {
            using (var uow = uowManager.Begin())
            {
                await HardDeleteWithUnitOfWorkAsync(repository, entities, autoSave, cancellationToken, uowManager.Current);
                await uow.CompleteAsync(cancellationToken);
            }
        }
        else
        {
            await HardDeleteWithUnitOfWorkAsync(repository, entities, autoSave, cancellationToken, uowManager.Current);
        }
    }

    public static async Task HardDeleteAsync<TEntity>(
        this IBasicRepository<TEntity> repository,
        TEntity entity,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    )
        where TEntity : class, IEntity, ISoftDelete
    {
        var uowManager = repository.GetUnitOfWorkManager();

        if (uowManager.Current == null)
        {
            using (var uow = uowManager.Begin())
            {
                await HardDeleteWithUnitOfWorkAsync(repository, entity, autoSave, cancellationToken, uowManager.Current);
                await uow.CompleteAsync(cancellationToken);
            }
        }
        else
        {
            await HardDeleteWithUnitOfWorkAsync(repository, entity, autoSave, cancellationToken, uowManager.Current);
        }
    }

    private static IUnitOfWorkManager GetUnitOfWorkManager<TEntity>(
        this IBasicRepository<TEntity> repository,
        [CallerMemberName] string callingMethodName = nameof(GetUnitOfWorkManager)
    )
        where TEntity : class, IEntity
    {
        if (ProxyHelper.UnProxy(repository) is not IUnitOfWorkManagerAccessor unitOfWorkManagerAccessor)
        {
            throw new AbpException($"The given repository (of type {repository.GetType().AssemblyQualifiedName}) should implement the " +
                $"{typeof(IUnitOfWorkManagerAccessor).AssemblyQualifiedName} interface in order to invoke the {callingMethodName} method!");
        }

        if (unitOfWorkManagerAccessor.UnitOfWorkManager == null)
        {
            throw new AbpException($"{nameof(unitOfWorkManagerAccessor.UnitOfWorkManager)} property of the given {nameof(repository)} object is null!");
        }

        return unitOfWorkManagerAccessor.UnitOfWorkManager;
    }

    private static async Task HardDeleteWithUnitOfWorkAsync<TEntity>(
        IRepository<TEntity> repository,
        Expression<Func<TEntity, bool>> predicate,
        bool autoSave,
        CancellationToken cancellationToken,
        IUnitOfWork currentUow
    )
        where TEntity : class, IEntity, ISoftDelete
    {
        using (currentUow.ServiceProvider.GetRequiredService<IDataFilter<ISoftDelete>>().Disable())
        {
            var entities = await repository.AsyncExecuter.ToListAsync((await repository.GetQueryableAsync()).Where(predicate), cancellationToken);
            await HardDeleteWithUnitOfWorkAsync(repository, entities, autoSave, cancellationToken, currentUow);
        }
    }

    private static async Task HardDeleteWithUnitOfWorkAsync<TEntity>(
        IBasicRepository<TEntity> repository,
        IEnumerable<TEntity> entities,
        bool autoSave,
        CancellationToken cancellationToken,
        IUnitOfWork currentUow
    )
        where TEntity : class, IEntity, ISoftDelete
    {
        var hardDeleteEntities = (HashSet<IEntity>)currentUow.Items.GetOrAdd(
            UnitOfWorkItemNames.HardDeletedEntities,
            () => new HashSet<IEntity>()
        );

        hardDeleteEntities.UnionWith(entities);
        await repository.DeleteManyAsync(entities, autoSave, cancellationToken);
    }

    private static async Task HardDeleteWithUnitOfWorkAsync<TEntity>(
        IBasicRepository<TEntity> repository,
        TEntity entity,
        bool autoSave,
        CancellationToken cancellationToken,
        IUnitOfWork currentUow
    )
        where TEntity : class, IEntity, ISoftDelete
    {
        var hardDeleteEntities = (HashSet<IEntity>)currentUow.Items.GetOrAdd(
            UnitOfWorkItemNames.HardDeletedEntities,
            () => new HashSet<IEntity>()
        );

        hardDeleteEntities.Add(entity);
        await repository.DeleteAsync(entity, autoSave, cancellationToken);
    }
}
