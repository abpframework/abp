using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Internal;
using Volo.Abp.Modularity;

namespace Volo.Abp
{
    public class AbpApplication
    {
        [NotNull]
        public Type StartupModuleType { get; }

        public IServiceProvider ServiceProvider { get; private set; }

        [NotNull]
        internal AbpModuleDescriptor[] Modules { get; }

        private AbpApplication(
            [NotNull] Type startupModuleType,
            [NotNull] IServiceCollection services,
            [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction)
        {
            Check.NotNull(startupModuleType, nameof(startupModuleType));
            Check.NotNull(services, nameof(services));

            StartupModuleType = startupModuleType;

            var options = new AbpApplicationCreationOptions(services);
            optionsAction?.Invoke(options);

            services.AddCoreAbpServices(this);
            Modules = LoadModules(services, options);
        }

        internal static AbpApplication Create<TStartupModule>([NotNull] IServiceCollection services)
            where TStartupModule : IAbpModule
        {
            return Create<TStartupModule>(services, null);
        }

        internal static AbpApplication Create<TStartupModule>(
            [NotNull] IServiceCollection services,
            [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction)
            where TStartupModule : IAbpModule
        {
            return new AbpApplication(typeof(TStartupModule), services, optionsAction);
        }

        internal static AbpApplication Create(
            [NotNull] Type startupModuleType, 
            [NotNull] IServiceCollection services)
        {
            return Create(startupModuleType, services, null);
        }

        internal static AbpApplication Create(
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

            ServiceProvider
                .GetRequiredService<IModuleManager>()
                .InitializeModules(new ApplicationInitializationContext(serviceProvider));
        }

        private AbpModuleDescriptor[] LoadModules(IServiceCollection services, AbpApplicationCreationOptions options)
        {
            return services
                .GetSingletonInstance<IModuleLoader>()
                .LoadModules(
                    services,
                    StartupModuleType,
                    options.PlugInSources
                );
        }

        public void Shutdown()
        {
            ServiceProvider
                .GetRequiredService<IModuleManager>()
                .ShutdownModules(new ApplicationShutdownContext());
        }
    }
}
