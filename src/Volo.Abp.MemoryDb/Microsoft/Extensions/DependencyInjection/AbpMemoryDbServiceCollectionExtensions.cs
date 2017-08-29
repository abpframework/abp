using System;
using Volo.Abp.MemoryDb;
using Volo.Abp.MemoryDb.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpMemoryDbServiceCollectionExtensions
    {
        public static IServiceCollection AddMemoryDbContext<TMemoryDbContext>(this IServiceCollection services, Action<IMemoryDbContextRegistrationOptionsBuilder> optionsBuilder = null)
            where TMemoryDbContext : MemoryDbContext
        {
            var options = new MemoryDbContextRegistrationOptions();
            optionsBuilder?.Invoke(options);

            services.AddSingleton<TMemoryDbContext>();

            new MemoryDbRepositoryRegistrar(options)
                .AddRepositories(services, typeof(TMemoryDbContext));

            return services;
        }
    }
}
