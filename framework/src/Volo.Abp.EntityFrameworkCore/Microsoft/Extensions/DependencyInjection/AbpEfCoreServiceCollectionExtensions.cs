using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.DependencyInjection;
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

            var options = new AbpDbContextRegistrationOptions(typeof(TDbContext), services);

            var replaceDbContextAttribute = typeof(TDbContext).GetCustomAttribute<ReplaceDbContextAttribute>(true);
            if (replaceDbContextAttribute != null)
            {
                options.ReplacedDbContextTypes.AddRange(replaceDbContextAttribute.ReplacedDbContextTypes);
            }

            optionsBuilder?.Invoke(options);

            services.TryAddTransient(DbContextOptionsFactory.Create<TDbContext>);

            foreach (var dbContextType in options.ReplacedDbContextTypes)
            {
                services.Replace(
                    ServiceDescriptor.Transient(
                        dbContextType,
                        sp => sp.GetRequiredService(typeof(TDbContext))
                    )
                );

                services.Configure<AbpDbContextOptions>(opts =>
                {
                    opts.DbContextReplacements[dbContextType] = typeof(TDbContext);
                });
            }

            new EfCoreRepositoryRegistrar(options).AddRepositories();

            return services;
        }
    }
}
