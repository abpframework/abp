using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
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
