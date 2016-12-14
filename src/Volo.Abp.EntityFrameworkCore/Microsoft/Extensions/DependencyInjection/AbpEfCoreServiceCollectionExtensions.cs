using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Repositories.EntityFrameworkCore;
using Volo.ExtensionMethods.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpEfCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddAbpDbContext<TDbContext>(
            this IServiceCollection services)
            where TDbContext : AbpDbContext<TDbContext>
        {
            services //TODO: This are copied from EntityFrameworkServiceCollectionExtensions, we should think on that later
                .AddMemoryCache()
                .AddLogging();

            services.TryAddTransient<TDbContext>();
            services.TryAddSingleton(serviceProvider =>
            {
                const string moduleName = ""; //TODO: Use AbpModuleDescriptor instead of module name?

                var connInfoResolver = serviceProvider.GetRequiredService<IConnectionStringResolver>();

                var context = new AbpDbContextConfigurationContext<TDbContext>(connInfoResolver.Resolve(moduleName), moduleName);

                var dbContextOptions = serviceProvider.GetRequiredService<IOptions<AbpDbContextOptions>>().Value;

                var configureAction = dbContextOptions.ConfigureActions.GetOrDefault(typeof(TDbContext));
                if (configureAction != null)
                {
                    ((Action<AbpDbContextConfigurationContext<TDbContext>>) configureAction).Invoke(context);
                }
                else if(dbContextOptions.DefaultConfigureAction != null)
                {
                    dbContextOptions.DefaultConfigureAction.Invoke(context);
                }
                else
                {
                    throw new AbpException("Should set a configure action for dbcontext"); //TODO: Better message
                }

                return context.DbContextOptions.Options;
            });

            return services;
        }

        public static IServiceCollection AddDefaultEfCoreRepositories<TDbContext>(this IServiceCollection services)
            where TDbContext : AbpDbContext<TDbContext>
        {
            //TODO: Add options to use a provided type as default repository.

            var dbContextType = typeof(TDbContext);

            foreach (var entityType in DbContextHelper.GetEntityTypes(dbContextType))
            {
                var primaryKeyType = EntityHelper.GetPrimaryKeyType(entityType);

                var repositoryInterfaceType = typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType);
                var queryableRepositoryInterfaceType = typeof(IQueryableRepository<,>).MakeGenericType(entityType, primaryKeyType);
                var repositoryImplementationType = typeof(EfCoreRepository<,,>).MakeGenericType(dbContextType, entityType, primaryKeyType);

                services.TryAddTransient(repositoryInterfaceType, repositoryImplementationType);
                services.TryAddTransient(queryableRepositoryInterfaceType, repositoryImplementationType);
            }

            return services;
        }
    }
}
