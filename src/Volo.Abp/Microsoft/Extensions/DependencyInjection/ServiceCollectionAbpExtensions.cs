using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionAbpExtensions
    {
        public static AbpApplication AddApplication<TStartupModule>(
            [NotNull] this IServiceCollection services)
            where TStartupModule : IAbpModule
        {
            return AbpApplication.Create<TStartupModule>(services);
        }

        public static AbpApplication AddApplication<TStartupModule>(
            [NotNull] this IServiceCollection services, 
            [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction)
            where TStartupModule : IAbpModule
        {
            return AbpApplication.Create<TStartupModule>(services, optionsAction);
        }

        public static AbpApplication AddApplication(
            [NotNull] this IServiceCollection services,
            [NotNull] Type startupModuleType)
        {
            return AbpApplication.Create(startupModuleType, services);
        }

        public static AbpApplication AddApplication(
            [NotNull] this IServiceCollection services,
            [NotNull] Type startupModuleType,
            [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction)
        {
            return AbpApplication.Create(startupModuleType, services, optionsAction);
        }
    }
}