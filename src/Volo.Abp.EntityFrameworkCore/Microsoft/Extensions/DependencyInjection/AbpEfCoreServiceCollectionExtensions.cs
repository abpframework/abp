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

            services.TryAddTransient<TDbContext>();
            services.TryAddTransient(DbContextOptionsFactory.Create<TDbContext>);

            var options = new AbpDbContextRegistrationOptions();
            optionsBuilder?.Invoke(options);

            new EfCoreRepositoryRegistrar(options)
                .AddRepositories(services, typeof(TDbContext));

            return services;
        }
    }
}
