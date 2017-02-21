using System;
using Volo.Abp.MongoDB;
using Volo.Abp.MongoDB.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpMongoDbServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDbContext<TMongoDbContext>(this IServiceCollection services, Action<IMongoDbContextRegistrationOptionsBuilder> optionsBuilder = null) //Created overload instead of default parameter
            where TMongoDbContext : AbpMongoDbContext
        {
            var options = new MongoDbContextRegistrationOptions();
            optionsBuilder?.Invoke(options);

            services.AddSingleton<TMongoDbContext>();

            new MongoDbRepositoryRegistrar(options)
                .AddRepositories(services, typeof(TMongoDbContext));

            return services;
        }
    }
}
