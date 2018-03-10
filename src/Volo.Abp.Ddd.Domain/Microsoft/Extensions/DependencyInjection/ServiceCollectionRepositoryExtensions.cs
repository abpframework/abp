using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionRepositoryExtensions
    {
        public static IServiceCollection AddDefaultRepository(this IServiceCollection services, Type entityType, Type repositoryImplementationType)
        {
            //IRepository<TEntity>
            var repositoryInterfaceWithoutPk = typeof(IBasicRepository<>).MakeGenericType(entityType);
            if (!repositoryInterfaceWithoutPk.IsAssignableFrom(repositoryImplementationType))
            {
                throw new AbpException($"Given repositoryImplementationType ({repositoryImplementationType}) must implement {repositoryInterfaceWithoutPk}");
            }

            services.TryAddTransient(repositoryInterfaceWithoutPk, repositoryImplementationType);

            //IQueryableRepository<TEntity>
            var queryableRepositoryInterfaceWithPk = typeof(IRepository<>).MakeGenericType(entityType);
            if (repositoryInterfaceWithoutPk.IsAssignableFrom(repositoryImplementationType))
            {
                services.TryAddTransient(queryableRepositoryInterfaceWithPk, repositoryImplementationType);
            }

            var primaryKeyType = EntityHelper.FindPrimaryKeyType(entityType);

            if (primaryKeyType != null)
            {
                //IRepository<TEntity, TKey>
                var repositoryInterface = typeof(IBasicRepository<,>).MakeGenericType(entityType, primaryKeyType);
                if (repositoryInterface.GetTypeInfo().IsAssignableFrom(repositoryImplementationType))
                {
                    services.TryAddTransient(repositoryInterface, repositoryImplementationType);
                }

                //IQueryableRepository<TEntity, TKey>
                var queryableRepositoryInterface = typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType);
                if (queryableRepositoryInterface.GetTypeInfo().IsAssignableFrom(repositoryImplementationType))
                {
                    services.TryAddTransient(queryableRepositoryInterface, repositoryImplementationType);
                }
            }

            return services;
        }
    }
}