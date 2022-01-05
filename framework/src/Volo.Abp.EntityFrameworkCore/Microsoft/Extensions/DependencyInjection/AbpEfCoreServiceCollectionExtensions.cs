using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpEfCoreServiceCollectionExtensions
{
    public static IServiceCollection AddAbpDbContext<TDbContext>(
        this IServiceCollection services,
        Action<IAbpDbContextRegistrationOptionsBuilder> optionsBuilder = null)
        where TDbContext : AbpDbContext<TDbContext>
    {
        services.AddMemoryCache();

        var options = new AbpDbContextRegistrationOptions(typeof(TDbContext), services);

        var replacedDbContextTypes = typeof(TDbContext).GetCustomAttributes<ReplaceDbContextAttribute>(true)
            .SelectMany(x => x.ReplacedDbContextTypes).ToList();

        foreach (var dbContextType in replacedDbContextTypes)
        {
            options.ReplaceDbContext(dbContextType);
        }

        optionsBuilder?.Invoke(options);

        services.TryAddTransient(DbContextOptionsFactory.Create<TDbContext>);

        foreach (var entry in options.ReplacedDbContextTypes)
        {
            var originalDbContextType = entry.Key;
            var targetDbContextType = entry.Value ?? typeof(TDbContext);

            services.Replace(
                ServiceDescriptor.Transient(
                    originalDbContextType,
                    sp => sp.GetRequiredService(targetDbContextType)
                )
            );

            services.Configure<AbpDbContextOptions>(opts =>
            {
                opts.DbContextReplacements[originalDbContextType] = targetDbContextType;
            });
        }

        new EfCoreRepositoryRegistrar(options).AddRepositories();

        return services;
    }
}
