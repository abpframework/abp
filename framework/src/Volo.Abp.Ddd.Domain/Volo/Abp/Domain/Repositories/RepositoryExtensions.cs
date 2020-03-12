using System;
using System.Collections.Generic;
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

        public static async Task HardDeleteAsync<TEntity>(
            this IBasicRepository<TEntity> repository,
            TEntity entity,
            bool autoSave = false,
            CancellationToken cancellationToken = default
        )
            where TEntity : class, IEntity, ISoftDelete
        {
            if (!(ProxyHelper.UnProxy(repository) is IUnitOfWorkManagerAccessor unitOfWorkManagerAccessor))
            {
                throw new AbpException($"The given repository (of type {repository.GetType().AssemblyQualifiedName}) should implement the {typeof(IUnitOfWorkManagerAccessor).AssemblyQualifiedName} interface in order to invoke the {nameof(HardDeleteAsync)} method!");
            }

            var uowManager = unitOfWorkManagerAccessor.UnitOfWorkManager;
            if (uowManager == null)
            {
                throw new AbpException($"{nameof(unitOfWorkManagerAccessor.UnitOfWorkManager)} property of the given {nameof(repository)} object is null!");
            }

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

        private static async Task HardDeleteWithUnitOfWorkAsync<TEntity>(
            IBasicRepository<TEntity> repository, 
            TEntity entity, 
            bool autoSave,
            CancellationToken cancellationToken, IUnitOfWork currentUow
        )
            where TEntity : class, IEntity, ISoftDelete
        {
            var hardDeleteEntities = (HashSet<IEntity>) currentUow.Items.GetOrAdd(
                UnitOfWorkItemNames.HardDeletedEntities,
                () => new HashSet<IEntity>()
            );

            hardDeleteEntities.Add(entity);

            await repository.DeleteAsync(entity, autoSave, cancellationToken);
        }
    }
}
