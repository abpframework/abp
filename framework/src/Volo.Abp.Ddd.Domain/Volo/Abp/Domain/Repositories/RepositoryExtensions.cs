using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Uow;

namespace Volo.Abp.Domain.Repositories
{
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

        public static async Task HardDeleteAsync<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity)
            where TEntity : class, IEntity<TPrimaryKey>, ISoftDelete
        {
            var repo = ProxyHelper.UnProxy(repository) as IRepository<TEntity, TPrimaryKey>;
            if (repo != null)
            {
                var uow = ((IUnitOfWorkManagerAccessor)repo).UnitOfWorkManager;
                var baseRepository = ((RepositoryBase<TEntity>)repo);

                var items = ((IUnitOfWorkManagerAccessor)repo).UnitOfWorkManager.Current.Items;
                var hardDeleteEntities = items.GetOrAdd(UnitOfWorkExtensionDataTypes.HardDelete, () => new HashSet<string>()) as HashSet<string>;

                var hardDeleteKey = EntityHelper.GetHardDeleteKey(entity, baseRepository.CurrentTenant?.Id?.ToString());
                hardDeleteEntities.Add(hardDeleteKey);

                await repo.DeleteAsync(entity);
            }
        }
        public static async Task HardDeleteAsync<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, Expression<Func<TEntity, bool>> predicate)
            where TEntity : class, IEntity<TPrimaryKey>, ISoftDelete
        {
            foreach (var entity in repository.Where(predicate).ToList())
            {
                await repository.HardDeleteAsync(entity);
            }
        }
    }
}
