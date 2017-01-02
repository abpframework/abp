using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

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

            new EfCoreRepositoryRegistrar(options)
                .AddRepositories(services, typeof(TDbContext));

            return services;
        }
    }
}
