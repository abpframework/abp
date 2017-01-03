using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionRepositoryExtensions
    {
        public static void AddDefaultRepository(this IServiceCollection services, Type entityType, Type repositoryImplementationType)
        {
            AddDefaultRepositoryForGenericPrimaryKey(services, entityType, repositoryImplementationType);

            if (BothSupportsDefaultPrimaryKey(entityType, repositoryImplementationType))
            {
                AddDefaultRepositoryForDefaultPrimaryKey(services, entityType, repositoryImplementationType);
            }
        }

        private static void AddDefaultRepositoryForGenericPrimaryKey(IServiceCollection services, Type entityType, Type repositoryImplementationType)
        {
            var primaryKeyType = EntityHelper.GetPrimaryKeyType(entityType);

            //IRepository<TEntity, TPrimaryKey>
            var repositoryInterface = typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType);
            if (!repositoryInterface.GetTypeInfo().IsAssignableFrom(repositoryImplementationType))
            {
                throw new AbpException($"Given repositoryImplementationType ({repositoryImplementationType}) must implement {repositoryInterface}");
            }

            services.TryAddTransient(repositoryInterface, repositoryImplementationType);

            //IQueryableRepository<TEntity, TPrimaryKey>
            var queryableRepositoryInterface = typeof(IQueryableRepository<,>).MakeGenericType(entityType, primaryKeyType);
            if (queryableRepositoryInterface.GetTypeInfo().IsAssignableFrom(repositoryImplementationType))
            {
                services.TryAddTransient(queryableRepositoryInterface, repositoryImplementationType);
            }
        }

        private static void AddDefaultRepositoryForDefaultPrimaryKey(IServiceCollection services, Type entityType, Type repositoryImplementationType)
        {
            //IRepository<TEntity>
            var repositoryInterfaceWithDefaultPrimaryKey = typeof(IRepository<>).MakeGenericType(entityType);
            if (!repositoryInterfaceWithDefaultPrimaryKey.GetTypeInfo().IsAssignableFrom(repositoryImplementationType))
            {
                throw new AbpException($"Given repositoryImplementationType ({repositoryImplementationType}) must implement {repositoryInterfaceWithDefaultPrimaryKey}");
            }

            services.TryAddTransient(repositoryInterfaceWithDefaultPrimaryKey, repositoryImplementationType);

            //IQueryableRepository<TEntity>
            var queryableRepositoryInterfaceWithDefaultPrimaryKey = typeof(IQueryableRepository<>).MakeGenericType(entityType);
            if (queryableRepositoryInterfaceWithDefaultPrimaryKey.GetTypeInfo().IsAssignableFrom(repositoryImplementationType))
            {
                services.TryAddTransient(queryableRepositoryInterfaceWithDefaultPrimaryKey, repositoryImplementationType);
            }
        }

        private static bool BothSupportsDefaultPrimaryKey(Type entityType, Type repositoryImplementationType)
        {
            return typeof(IEntity<string>).GetTypeInfo().IsAssignableFrom(entityType) &&
                   ReflectionHelper.IsAssignableToGenericType(repositoryImplementationType, typeof(IRepository<>));
        }
    }
}