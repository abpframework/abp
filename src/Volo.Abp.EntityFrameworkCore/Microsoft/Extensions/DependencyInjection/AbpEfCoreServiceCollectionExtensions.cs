using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpEfCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddAbpDbContext<TDbContext>(this IServiceCollection services, Action<AddAbpDbContextOptions> optionsAction = null) //Created overload instead of default parameter
            where TDbContext : AbpDbContext<TDbContext>
        {
            services //TODO: This code is copied from EntityFrameworkServiceCollectionExtensions, we should think on that later
                .AddMemoryCache()
                .AddLogging();

            services.TryAddTransient<TDbContext>();
            services.TryAddSingleton(DbContextOptionsFactory.Create<TDbContext>);

            var options = new AddAbpDbContextOptions();
            optionsAction?.Invoke(options);

            services.AddRepositories<TDbContext>(options.RepositoryOptions);

            return services;
        }

        private static void AddRepositories<TDbContext>(this IServiceCollection services, RepositoryRegistrationOptions options)
            where TDbContext : AbpDbContext<TDbContext>
        {
            //TODO: Refactor!!!

            var dbContextType = typeof(TDbContext);

            foreach (var customRepository in options.CustomRepositories)
            {
                Register<TDbContext>(services, customRepository.Key, customRepository.Value);
            }

            if (options.RegisterDefaultRepositories)
            {
                foreach (var entityType in DbContextHelper.GetEntityTypes(dbContextType))
                {
                    if (options.CustomRepositories.ContainsKey(entityType))
                    {
                        continue;
                    }

                    if (!options.IncludeAllEntitiesForDefaultRepositories && !ReflectionHelper.IsAssignableToGenericType(entityType, typeof(IAggregateRoot<>)))
                    {
                        continue;
                    }

                    var primaryKeyType = EntityHelper.GetPrimaryKeyType(entityType);

                    var repositoryImplementationType = typeof(IEntity).GetTypeInfo().IsAssignableFrom(entityType)
                        ? typeof(EfCoreRepository<,>).MakeGenericType(dbContextType, entityType)
                        : typeof(EfCoreRepository<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType);

                    Register<TDbContext>(services, entityType, repositoryImplementationType);
                }
            }
        }

        private static void Register<TDbContext>(IServiceCollection services, Type entityType, Type repositoryImplementationType)
            where TDbContext : AbpDbContext<TDbContext>
        {
            var primaryKeyType = EntityHelper.GetPrimaryKeyType(entityType);

            var repositoryInterfaceType = typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType);
            var queryableRepositoryInterfaceType = typeof(IQueryableRepository<,>).MakeGenericType(entityType, primaryKeyType);

            services.TryAddTransient(repositoryInterfaceType, repositoryImplementationType);
            services.TryAddTransient(queryableRepositoryInterfaceType, repositoryImplementationType);

            //Supports Default PK?
            if (typeof(IEntity).GetTypeInfo().IsAssignableFrom(entityType) &&
                ReflectionHelper.IsAssignableToGenericType(repositoryImplementationType, typeof(IRepository<>)))
            {
                var defaultPkRepositoryInterfaceType = typeof(IRepository<>).MakeGenericType(entityType);
                var defaultPkQueryableRepositoryInterfaceType = typeof(IQueryableRepository<>).MakeGenericType(entityType);

                services.TryAddTransient(defaultPkRepositoryInterfaceType, repositoryImplementationType);
                services.TryAddTransient(defaultPkQueryableRepositoryInterfaceType, repositoryImplementationType);
            }
        }
    }
}
