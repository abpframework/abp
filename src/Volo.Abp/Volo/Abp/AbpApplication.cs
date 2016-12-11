using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp
{
    public class AbpApplication : IDisposable
    {
        public Type StartupModuleType { get; }

        public IServiceProvider ServiceProvider { get; private set; }

        private AbpApplication(Type startupModuleType, IServiceCollection services, Action<AbpApplicationOptions> optionsAction)
        {
            StartupModuleType = startupModuleType;

            var options = new AbpApplicationOptions();

            optionsAction?.Invoke(options);

            AddServices(services);
            LoadModules(services, options);
        }

        public static AbpApplication Create<TStartupModule>([NotNull] IServiceCollection services)
            where TStartupModule : IAbpModule
        {
            return Create<TStartupModule>(services, null);
        }

        public static AbpApplication Create<TStartupModule>(
            [NotNull] IServiceCollection services,
            [CanBeNull] Action<AbpApplicationOptions> optionsAction)
            where TStartupModule : IAbpModule
        {
            Check.NotNull(services, nameof(services));

            return new AbpApplication(typeof(TStartupModule), services, optionsAction);
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ServiceProvider.GetRequiredService<IModuleManager>().Initialize();
        }

        private void AddServices(IServiceCollection services)
        {
            services.AddSingleton(this);
            services.AddCoreAbpServices();
        }

        private void LoadModules(IServiceCollection services, AbpApplicationOptions options)
        {
            services.GetSingletonInstance<IModuleLoader>().LoadAll(services, StartupModuleType, options.PlugInSources);
        }

        public void Dispose()
        {

        }
    }
}
