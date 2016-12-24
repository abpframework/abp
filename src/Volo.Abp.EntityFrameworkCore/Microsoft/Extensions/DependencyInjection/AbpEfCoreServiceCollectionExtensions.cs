using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    //TODO: By default, only create repositories for Aggregate Roots.
    //TODO: Move AddDefaultEfCoreRepositories into AddAbpDbContext as optional which will have it's own options
    //TODO: Add options to use a provided type as default repository.
    //TODO: Register default PK type if available!

    public static class AbpEfCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddAbpDbContext<TDbContext>(
            this IServiceCollection services)
            where TDbContext : AbpDbContext<TDbContext>
        {
            services //TODO: This code is copied from EntityFrameworkServiceCollectionExtensions, we should think on that later
                .AddMemoryCache()
                .AddLogging();

            services.TryAddTransient<TDbContext>();
            services.TryAddSingleton(DbContextOptionsFactory.Create<TDbContext>);

            services.AddDefaultEfCoreRepositories<TDbContext>();

            return services;
        }

        private static void AddDefaultEfCoreRepositories<TDbContext>(this IServiceCollection services)
            where TDbContext : AbpDbContext<TDbContext>
        {
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
        }
    }
}
