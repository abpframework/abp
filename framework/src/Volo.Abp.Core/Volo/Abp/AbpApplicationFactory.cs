using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp;

public static class AbpApplicationFactory
{
    public async static Task<IAbpApplicationWithInternalServiceProvider> CreateAsync<TStartupModule>(
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IAbpModule
    {
        var app = Create(typeof(TStartupModule), options =>
        {
            optionsAction?.Invoke(options);
            options.ManualConfigureServices = true;
        });
        await app.ConfigureServicesAsync();
        return app;
    }

    public async static Task<IAbpApplicationWithInternalServiceProvider> CreateAsync(
        [NotNull] Type startupModuleType,
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
    {
        var app = new AbpApplicationWithInternalServiceProvider(startupModuleType, options =>
        {
            optionsAction?.Invoke(options);
            options.ManualConfigureServices = true;
        });
        await app.ConfigureServicesAsync();
        return app;
    }

    public async static Task<IAbpApplicationWithExternalServiceProvider> CreateAsync<TStartupModule>(
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IAbpModule
    {
        var app = Create(typeof(TStartupModule), services, options =>
        {
            optionsAction?.Invoke(options);
            options.ManualConfigureServices = true;
        });
        await app.ConfigureServicesAsync();
        return app;
    }

    public async static Task<IAbpApplicationWithExternalServiceProvider> CreateAsync(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
    {
        var app = new AbpApplicationWithExternalServiceProvider(startupModuleType, services, options =>
        {
            optionsAction?.Invoke(options);
            options.ManualConfigureServices = true;
        });
        await app.ConfigureServicesAsync();
        return app;
    }

    public static IAbpApplicationWithInternalServiceProvider Create<TStartupModule>(
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IAbpModule
    {
        return Create(typeof(TStartupModule), optionsAction);
    }

    public static IAbpApplicationWithInternalServiceProvider Create(
        [NotNull] Type startupModuleType,
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
    {
        return new AbpApplicationWithInternalServiceProvider(startupModuleType, optionsAction);
    }

    public static IAbpApplicationWithExternalServiceProvider Create<TStartupModule>(
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IAbpModule
    {
        return Create(typeof(TStartupModule), services, optionsAction);
    }

    public static IAbpApplicationWithExternalServiceProvider Create(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
    {
        return new AbpApplicationWithExternalServiceProvider(startupModuleType, services, optionsAction);
    }
}
