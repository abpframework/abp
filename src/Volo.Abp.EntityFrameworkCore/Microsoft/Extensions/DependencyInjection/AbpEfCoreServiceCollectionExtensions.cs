using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Repositories.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpEfCoreServiceCollectionExtensions
    {
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
