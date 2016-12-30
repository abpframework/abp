using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpEfCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddAbpDbContext<TDbContext>(this IServiceCollection services, Action<IAbpDbContextRegistrationOptionsBuilder> optionsBuilder = null) //Created overload instead of default parameter
            where TDbContext : AbpDbContext<TDbContext>
        {
            services //TODO: This code is copied from EntityFrameworkServiceCollectionExtensions, we should think on that later
                .AddMemoryCache()
                .AddLogging();

            services.TryAddTransient<TDbContext>();
            services.TryAddSingleton(DbContextOptionsFactory.Create<TDbContext>);

            var options = new AbpDbContextRegistrationOptions();
            optionsBuilder?.Invoke(options);

            AddRepositories<TDbContext>(services, options);

            return services;
        }

        private static void AddRepositories<TDbContext>(IServiceCollection services, AbpDbContextRegistrationOptions options)
            where TDbContext : AbpDbContext<TDbContext>
        {
            foreach (var customRepository in options.CustomRepositories)
            {
                services.AddDefaultRepository(customRepository.Key, customRepository.Value);
            }

            if (options.RegisterDefaultRepositories)
            {
                RegisterDefaultRepositories(services, typeof(TDbContext), options);
            }
        }

        private static void RegisterDefaultRepositories(IServiceCollection services, Type dbContextType, AbpDbContextRegistrationOptions options)
        {
            foreach (var entityType in DbContextHelper.GetEntityTypes(dbContextType))
            {
                if (!options.ShouldRegisterDefaultRepositoryFor(entityType))
                {
                    continue;
                }

                RegisterDefaultRepository(services, dbContextType, entityType, options);
            }
        }

        private static void RegisterDefaultRepository(IServiceCollection services, Type dbContextType, Type entityType, AbpDbContextRegistrationOptions options)
        {
            var repositoryImplementationType = typeof(IEntity).GetTypeInfo().IsAssignableFrom(entityType)
                ? typeof(EfCoreRepository<,>).MakeGenericType(dbContextType, entityType)
                : typeof(EfCoreRepository<,,>).MakeGenericType(dbContextType, entityType, EntityHelper.GetPrimaryKeyType(entityType));

            services.AddDefaultRepository(entityType, repositoryImplementationType);
        }
    }
}
