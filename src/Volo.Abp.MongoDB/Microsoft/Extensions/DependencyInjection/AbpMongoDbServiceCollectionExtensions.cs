using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.MongoDB;
using Volo.Abp.MongoDB.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpMongoDbServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDbContext<TMongoDbContext>(this IServiceCollection services, Action<IMongoDbContextRegistrationOptionsBuilder> optionsBuilder = null) //Created overload instead of default parameter
            where TMongoDbContext : AbpMongoDbContext
        {
            var options = new MongoDbContextRegistrationOptions(typeof(TMongoDbContext));
            optionsBuilder?.Invoke(options);

            services.TryAddSingleton<TMongoDbContext>();

            if (options.DefaultRepositoryDbContextType != typeof(TMongoDbContext))
            {
                services.TryAddSingleton(options.DefaultRepositoryDbContextType, sp => sp.GetRequiredService<TMongoDbContext>());
            }

            foreach (var dbContextType in options.ReplacedDbContextTypes)
            {
                services.Replace(ServiceDescriptor.Singleton(dbContextType, sp => sp.GetRequiredService<TMongoDbContext>()));
            }

            new MongoDbRepositoryRegistrar(options)
                .AddRepositories(services);

            return services;
        }
    }
}
