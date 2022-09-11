using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Data;

public static class AbpDataMigrationEnvironmentExtensions
{
    public static void AddMigrationEnvironment(this AbpApplicationCreationOptions options, AbpDataMigrationEnvironment environment = null)
    {
        options.Services.AddMigrationEnvironment(environment ?? new AbpDataMigrationEnvironment());
    }

    public static void AddMigrationEnvironment(this IServiceCollection services, AbpDataMigrationEnvironment environment = null)
    {
        services.AddObjectAccessor<AbpDataMigrationEnvironment>(environment ?? new AbpDataMigrationEnvironment());
    }

    public static AbpDataMigrationEnvironment GetMigrationEnvironment(this IServiceCollection services)
    {
        return services.GetObjectOrNull<IObjectAccessor<AbpDataMigrationEnvironment>>()?.Value;
    }

    public static bool IsMigrationEnvironment(this IServiceCollection services)
    {
        return services.GetMigrationEnvironment() != null;
    }

    public static AbpDataMigrationEnvironment GetMigrationEnvironment(this IServiceProvider serviceProvider)
    {
        return serviceProvider.GetService<IObjectAccessor<AbpDataMigrationEnvironment>>()?.Value;
    }

    public static bool IsMigrationEnvironment(this IServiceProvider serviceProvider)
    {
        return serviceProvider.GetMigrationEnvironment() != null;
    }
}
