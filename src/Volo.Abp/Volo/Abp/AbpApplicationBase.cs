using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal;
using Volo.Abp.Modularity;

namespace Volo.Abp
{
    public abstract class AbpApplicationBase : IAbpApplication
    {
        [NotNull]
        public Type StartupModuleType { get; }

        public IServiceProvider ServiceProvider { get; private set; }

        public IServiceCollection Services { get; }

        public IReadOnlyList<IAbpModuleDescriptor> Modules { get; }

        internal AbpApplicationBase(
            [NotNull] Type startupModuleType,
            [NotNull] IServiceCollection services,
            [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction)
        {
            Check.NotNull(startupModuleType, nameof(startupModuleType));
            Check.NotNull(services, nameof(services));

            StartupModuleType = startupModuleType;
            Services = services;

            services.TryAddObjectAccessor<IServiceProvider>();

            var options = new AbpApplicationCreationOptions(services);
            optionsAction?.Invoke(options);

            services.AddSingleton<IAbpApplication>(this);
            services.AddSingleton<IModuleContainer>(this);
            services.AddCoreAbpServices(this);

            Modules = LoadModules(services, options);
        }

        public virtual void Shutdown()
        {
            ServiceProvider
                .GetRequiredService<IModuleManager>()
                .ShutdownModules(new ApplicationShutdownContext());
        }

        public virtual void Dispose()
        {

        }
        
        protected virtual void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = ServiceProvider;
        }

        protected virtual void InitializeModules()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                ServiceProvider
                    .GetRequiredService<IModuleManager>()
                    .InitializeModules(new ApplicationInitializationContext(scope.ServiceProvider));
            }
        }

        private IReadOnlyList<IAbpModuleDescriptor> LoadModules(IServiceCollection services, AbpApplicationCreationOptions options)
        {
            return services
                .GetSingletonInstance<IModuleLoader>()
                .LoadModules(
                    services,
                    StartupModuleType,
                    options.PlugInSources
                );
        }
    }
}