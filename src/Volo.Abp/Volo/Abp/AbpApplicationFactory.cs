using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp
{
    public static class AbpApplicationFactory
    {
        public static IAbpApplicationWithExternalServiceProvider Create<TStartupModule>(
            [NotNull] IServiceCollection services)
            where TStartupModule : IAbpModule
        {
            return Create<TStartupModule>(services, null);
        }

        public static IAbpApplicationWithExternalServiceProvider Create<TStartupModule>(
            [NotNull] IServiceCollection services,
            [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction)
            where TStartupModule : IAbpModule
        {
            return new AbpApplicationWithExternalServiceProvider(typeof(TStartupModule), services, optionsAction);
        }

        public static IAbpApplicationWithExternalServiceProvider Create(
            [NotNull] Type startupModuleType,
            [NotNull] IServiceCollection services)
        {
            return Create(startupModuleType, services, null);
        }

        public static IAbpApplicationWithExternalServiceProvider Create(
            [NotNull] Type startupModuleType,
            [NotNull] IServiceCollection services,
            [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction)
        {
            return new AbpApplicationWithExternalServiceProvider(startupModuleType, services, optionsAction);
        }
    }
}