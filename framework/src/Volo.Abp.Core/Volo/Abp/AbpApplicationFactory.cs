using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp;

public static class AbpApplicationFactory
{
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
