using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace Volo.Abp
{
    public class AbpApplication : IDisposable
    {
        public Type StartupModuleType { get; }

        private readonly IServiceCollection _services;
        private IServiceProvider _serviceProvider;

        private AbpApplication(Type startupModuleType, IServiceCollection services)
        {
            StartupModuleType = startupModuleType;
            _services = services;
            _services.AddCoreAbp();
        }

        public static AbpApplication Create<TStartupModule>(IServiceCollection services)
            where TStartupModule : IAbpModule
        {
            return new AbpApplication(typeof(TStartupModule), services);
        }

        public void Start(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serviceProvider.GetRequiredService<IModuleRunner>().Start();
        }

        public void Dispose()
        {
            _serviceProvider.GetRequiredService<IModuleRunner>().Stop();
        }
    }

    internal static class AbpServiceCollectionExtensions
    {
        public static void AddCoreAbp(this IServiceCollection services)
        {
            services.TryAddSingleton<IModuleRunner, ModuleRunner>();
        }
    }
}
