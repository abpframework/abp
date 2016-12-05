using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp
{
    public class AbpApplication : IDisposable
    {
        public Type StartupModuleType { get; }

        private IServiceProvider _serviceProvider;

        private AbpApplication(Type startupModuleType, IServiceCollection services)
        {
            StartupModuleType = startupModuleType;
            
            services.AddSingleton(this);
            services.AddCoreAbpServices();

            services.GetSingletonInstance<IModuleLoader>().LoadAll(services, StartupModuleType);
        }

        public static AbpApplication Create<TStartupModule>(IServiceCollection services)
            where TStartupModule : IAbpModule
        {
            return new AbpApplication(typeof(TStartupModule), services);
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceProvider.GetRequiredService<IModuleManager>().Initialize();
        }

        public void Dispose()
        {

        }
    }
}
