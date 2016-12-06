using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.DependencyInjection;

namespace Volo.Abp
{
    public class AbpApplication : IDisposable
    {
        public Type StartupModuleType { get; }

        public IServiceProvider ServiceProvider { get; private set; }

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
            ServiceProvider = serviceProvider;
            ServiceProvider.GetRequiredService<IModuleManager>().Initialize();
        }

        public void Dispose()
        {

        }
    }
}
