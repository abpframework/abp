using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionApplicationExtensions
{
    public static IAbpApplicationWithExternalServiceProvider AddApplication<TStartupModule>(
        [NotNull] this IServiceCollection services,
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IAbpModule
    {
        return AbpApplicationFactory.Create<TStartupModule>(services, optionsAction);
    }

    public static IAbpApplicationWithExternalServiceProvider AddApplication(
        [NotNull] this IServiceCollection services,
        [NotNull] Type startupModuleType,
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
    {
        return AbpApplicationFactory.Create(startupModuleType, services, optionsAction);
    }

    public async static Task<IAbpApplicationWithExternalServiceProvider> AddApplicationAsync<TStartupModule>(
        [NotNull] this IServiceCollection services,
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IAbpModule
    {
        return await AbpApplicationFactory.CreateAsync<TStartupModule>(services,  optionsAction);
    }

    public async static Task<IAbpApplicationWithExternalServiceProvider> AddApplicationAsync(
        [NotNull] this IServiceCollection services,
        [NotNull] Type startupModuleType,
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
    {
        return await AbpApplicationFactory.CreateAsync(startupModuleType, services, optionsAction);
    }
}
