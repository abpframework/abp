using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Threading;

namespace Volo.Abp.Domain.Repositories
{
    public static class RepositoryExtensions
    {
        public static async Task EnsureCollectionLoadedAsync<TEntity, TPrimaryKey, TProperty>(
            this IRepository<TEntity, TPrimaryKey> repository,
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression,
            CancellationToken cancellationToken = default(CancellationToken)
        )
            where TEntity : class, IEntity<TPrimaryKey>
            where TProperty : class
        {
            var repo = ProxyHelper.UnProxy(repository) as ISupportsExplicitLoading<TEntity, TPrimaryKey>;
            if (repo != null)
            {
                await repo.EnsureCollectionLoadedAsync(entity, propertyExpression, cancellationToken);
            }
        }

        public static void EnsureCollectionLoaded<TEntity, TPrimaryKey, TProperty>(
            this IRepository<TEntity, TPrimaryKey> repository,
            TEntity entity,
            Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression
        )
            where TEntity : class, IEntity<TPrimaryKey>
            where TProperty : class
        {
            AsyncHelper.RunSync(() => repository.EnsureCollectionLoadedAsync(entity, propertyExpression));
        }

        public static async Task EnsurePropertyLoadedAsync<TEntity, TPrimaryKey, TProperty>(
            this IRepository<TEntity, TPrimaryKey> repository,
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            CancellationToken cancellationToken = default(CancellationToken)
        )
            where TEntity : class, IEntity<TPrimaryKey>
            where TProperty : class
        {
            var repo = ProxyHelper.UnProxy(repository) as ISupportsExplicitLoading<TEntity, TPrimaryKey>;
            if (repo != null)
            {
                await repo.EnsurePropertyLoadedAsync(entity, propertyExpression, cancellationToken);
            }
        }

        public static void EnsurePropertyLoaded<TEntity, TPrimaryKey, TProperty>(
            this IRepository<TEntity, TPrimaryKey> repository,
            TEntity entity,
            Expression<Func<TEntity, TProperty>> propertyExpression
        )
            where TEntity : class, IEntity<TPrimaryKey>
            where TProperty : class
        {
            AsyncHelper.RunSync(() => repository.EnsurePropertyLoadedAsync(entity, propertyExpression));
        }
    }
}
