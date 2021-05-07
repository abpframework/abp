using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MongoDB;
using Volo.Abp.MongoDB.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AbpMongoDbServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDbContext<TMongoDbContext>(this IServiceCollection services, Action<IAbpMongoDbContextRegistrationOptionsBuilder> optionsBuilder = null) //Created overload instead of default parameter
            where TMongoDbContext : AbpMongoDbContext
        {
            var options = new AbpMongoDbContextRegistrationOptions(typeof(TMongoDbContext), services);

            var replaceDbContextAttribute = typeof(TMongoDbContext).GetCustomAttribute<ReplaceDbContextAttribute>(true);
            if (replaceDbContextAttribute != null)
            {
                options.ReplacedDbContextTypes.AddRange(replaceDbContextAttribute.ReplacedDbContextTypes);
            }

            optionsBuilder?.Invoke(options);

            foreach (var dbContextType in options.ReplacedDbContextTypes)
            {
                services.Replace(ServiceDescriptor.Transient(dbContextType, typeof(TMongoDbContext)));
            }

            foreach (var dbContextType in options.ReplacedDbContextTypes)
            {
                services.Replace(
                    ServiceDescriptor.Transient(
                        dbContextType,
                        sp => sp.GetRequiredService(typeof(TMongoDbContext))
                    )
                );

                services.Configure<AbpMongoDbContextOptions>(opts =>
                {
                    opts.DbContextReplacements[dbContextType] = typeof(TMongoDbContext);
                });
            }

            new MongoDbRepositoryRegistrar(options).AddRepositories();

            return services;
        }
    }
}
