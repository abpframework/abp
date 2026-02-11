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
        Action<IAbpDbContextRegistrationOptionsBuilder>? optionsBuilder = null)
        where TDbContext : AbpDbContext<TDbContext>
    {
        services.AddMemoryCache();

        var options = new AbpDbContextRegistrationOptions(typeof(TDbContext), services);

        var replacedMultiTenantDbContextTypes = typeof(TDbContext).GetCustomAttributes<ReplaceDbContextAttribute>(true)
            .SelectMany(x => x.ReplacedDbContextTypes).ToList();

        foreach (var dbContextType in replacedMultiTenantDbContextTypes)
        {
            options.ReplaceDbContext(dbContextType.Type, multiTenancySides: dbContextType.MultiTenancySide);
        }

        optionsBuilder?.Invoke(options);

        services.TryAddTransient(DbContextOptionsFactory.Create<TDbContext>);

        foreach (var entry in options.ReplacedDbContextTypes)
        {
            var originalDbContextType = entry.Key.Type;
            var targetDbContextType = entry.Value ?? typeof(TDbContext);

            services.Replace(ServiceDescriptor.Transient(originalDbContextType, sp =>
            {
                var dbContextType = sp.GetRequiredService<IEfCoreDbContextTypeProvider>().GetDbContextType(originalDbContextType);
                return sp.GetRequiredService(dbContextType);
            }));

            services.Configure<AbpDbContextOptions>(opts =>
            {
                var multiTenantDbContextType = new MultiTenantDbContextType(originalDbContextType, entry.Key.MultiTenancySide);
                opts.DbContextReplacements[multiTenantDbContextType] = targetDbContextType;
            });
        }

        new EfCoreRepositoryRegistrar(options).AddRepositories();

        return services;
    }
}
