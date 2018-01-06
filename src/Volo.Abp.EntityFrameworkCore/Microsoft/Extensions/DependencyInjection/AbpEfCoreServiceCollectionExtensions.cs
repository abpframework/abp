using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpEfCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddAbpDbContext<TDbContext>(
            this IServiceCollection services, 
            Action<IAbpDbContextRegistrationOptionsBuilder> optionsBuilder = null)
            where TDbContext : AbpDbContext<TDbContext>
        {
            services.AddMemoryCache();

            var options = new AbpDbContextRegistrationOptions(typeof(TDbContext));
            optionsBuilder?.Invoke(options);

            services.TryAddTransient<TDbContext>();
            services.TryAddTransient(DbContextOptionsFactory.Create<TDbContext>);

            if (options.DefaultRepositoryDbContextType != typeof(TDbContext))
            {
                services.TryAddTransient(options.DefaultRepositoryDbContextType, typeof(TDbContext));
            }

            foreach (var dbContextType in options.ReplacedDbContextTypes)
            {
                services.Replace(ServiceDescriptor.Transient(dbContextType, typeof(TDbContext)));
            }

            new EfCoreRepositoryRegistrar(options)
                .AddRepositories(services);

            return services;
        }
    }
}
