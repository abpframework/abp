using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.MemoryDb;
using Volo.Abp.MemoryDb.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpMemoryDbServiceCollectionExtensions
    {
        public static IServiceCollection AddMemoryDbContext<TMemoryDbContext>(this IServiceCollection services, Action<IAbpMemoryDbContextRegistrationOptionsBuilder> optionsBuilder = null)
            where TMemoryDbContext : MemoryDbContext
        {
            var options = new AbpMemoryDbContextRegistrationOptions(typeof(TMemoryDbContext), services);
            optionsBuilder?.Invoke(options);

            if (options.DefaultRepositoryDbContextType != typeof(TMemoryDbContext))
            {
                services.TryAddSingleton(options.DefaultRepositoryDbContextType, sp => sp.GetRequiredService<TMemoryDbContext>());
            }

            foreach (var dbContextType in options.ReplacedDbContextTypes)
            {
                services.Replace(ServiceDescriptor.Singleton(dbContextType, sp => sp.GetRequiredService<TMemoryDbContext>()));
            }

            new MemoryDbRepositoryRegistrar(options).AddRepositories();

            return services;
        }
    }
}
