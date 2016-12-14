using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Internal;
using Volo.Abp.Modularity;

namespace Volo.Abp
{
    public class AbpApplication
    {
        public Type StartupModuleType { get; }

        public IServiceProvider ServiceProvider { get; private set; }

        private AbpApplication(
            [NotNull] Type startupModuleType,
            [NotNull] IServiceCollection services,
            [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction)
        {
            Check.NotNull(startupModuleType, nameof(startupModuleType));
            Check.NotNull(services, nameof(services));

            StartupModuleType = startupModuleType;

            var options = new AbpApplicationCreationOptions();
            optionsAction?.Invoke(options);

            services.AddCoreAbpServices(this);
            LoadModules(services, options);
        }

        public static AbpApplication Create<TStartupModule>([NotNull] IServiceCollection services)
            where TStartupModule : IAbpModule
        {
            return Create<TStartupModule>(services, null);
        }

        public static AbpApplication Create<TStartupModule>(
            [NotNull] IServiceCollection services,
            [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction)
            where TStartupModule : IAbpModule
        {
            return new AbpApplication(typeof(TStartupModule), services, optionsAction);
        }

        public static AbpApplication Create(
            [NotNull] Type startupModuleType, 
            [NotNull] IServiceCollection services)
        {
            return Create(startupModuleType, services, null);
        }

        public static AbpApplication Create(
            [NotNull] Type startupModuleType,
            [NotNull] IServiceCollection services,
            [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction)
        {
            return new AbpApplication(startupModuleType, services, optionsAction);
        }

        public void Initialize([NotNull] IServiceProvider serviceProvider)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            ServiceProvider = serviceProvider;
            ServiceProvider.GetRequiredService<IModuleInitializationManager>().InitializeModules();
        }

        private void LoadModules(IServiceCollection services, AbpApplicationCreationOptions options)
        {
            services
                .GetSingletonInstance<IModuleLoader>()
                .LoadAll(
                    services,
                    StartupModuleType,
                    options.PlugInSources
                );
        }

        public void Shutdown()
        {
            //TODO: Shutdown modules
        }
    }
}
