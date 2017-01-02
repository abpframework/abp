using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionRepositoryExtensions
    {
        //TODO: validate repository and entity if they match!

        public static void AddDefaultRepository(this IServiceCollection services, Type entityType, Type repositoryImplementationType)
        {
            var primaryKeyType = EntityHelper.GetPrimaryKeyType(entityType);

            services.TryAddTransient(
                typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType),
                repositoryImplementationType
            );

            services.TryAddTransient( //TODO: May not support IQueryableRepository
                typeof(IQueryableRepository<,>).MakeGenericType(entityType, primaryKeyType),
                repositoryImplementationType
            );

            if (BothSupportsDefaultPrimaryKey(entityType, repositoryImplementationType))
            {
                services.TryAddTransient(
                    typeof(IRepository<>).MakeGenericType(entityType),
                    repositoryImplementationType
                );

                services.TryAddTransient( //TODO: May not support IQueryableRepository
                    typeof(IQueryableRepository<>).MakeGenericType(entityType),
                    repositoryImplementationType
                );
            }
        }

        private static bool BothSupportsDefaultPrimaryKey(Type entityType, Type repositoryImplementationType)
        {
            return typeof(IEntity<string>).GetTypeInfo().IsAssignableFrom(entityType) &&
                   ReflectionHelper.IsAssignableToGenericType(repositoryImplementationType, typeof(IRepository<>));
        }
    }
}