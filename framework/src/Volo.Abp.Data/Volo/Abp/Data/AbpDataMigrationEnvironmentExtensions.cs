using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Data;

public static class AbpDataMigrationEnvironmentExtensions
{
    public static void AddDataMigrationEnvironment(this AbpApplicationCreationOptions options, AbpDataMigrationEnvironment environment = null)
    {
        options.Services.AddDataMigrationEnvironment(environment ?? new AbpDataMigrationEnvironment());
    }

    public static void AddDataMigrationEnvironment(this IServiceCollection services, AbpDataMigrationEnvironment environment = null)
    {
        services.AddObjectAccessor<AbpDataMigrationEnvironment>(environment ?? new AbpDataMigrationEnvironment());
    }

    public static AbpDataMigrationEnvironment GetDataMigrationEnvironment(this IServiceCollection services)
    {
        return services.GetObjectOrNull<AbpDataMigrationEnvironment>();
    }

    public static bool IsDataMigrationEnvironment(this IServiceCollection services)
    {
        return services.GetDataMigrationEnvironment() != null;
    }

    public static AbpDataMigrationEnvironment GetDataMigrationEnvironment(this IServiceProvider serviceProvider)
    {
        return serviceProvider.GetService<IObjectAccessor<AbpDataMigrationEnvironment>>()?.Value;
    }

    public static bool IsDataMigrationEnvironment(this IServiceProvider serviceProvider)
    {
        return serviceProvider.GetDataMigrationEnvironment() != null;
    }
}
