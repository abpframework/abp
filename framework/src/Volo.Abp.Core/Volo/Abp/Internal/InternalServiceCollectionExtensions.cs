using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Configuration;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;

namespace Volo.Abp.Internal
{
    internal static class InternalServiceCollectionExtensions
    {
        internal static void AddCoreServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddLogging();
            services.AddLocalization();
        }

        internal static void AddCoreAbpServices(this IServiceCollection services,
            IAbpApplication abpApplication, 
            AbpApplicationCreationOptions applicationCreationOptions)
        {
            var moduleLoader = new ModuleLoader();
            var assemblyFinder = new AssemblyFinder(abpApplication);
            var typeFinder = new TypeFinder(assemblyFinder);

            services.TryAddSingleton<IConfigurationAccessor>(
                new DefaultConfigurationAccessor(
                    ConfigurationHelper.BuildConfiguration(
                        applicationCreationOptions.Configuration
                    )
                )
            );

            services.TryAddSingleton<IModuleLoader>(moduleLoader);
            services.TryAddSingleton<IAssemblyFinder>(assemblyFinder);
            services.TryAddSingleton<ITypeFinder>(typeFinder);

            services.AddAssemblyOf<IAbpApplication>();

            services.Configure<ModuleLifecycleOptions>(options =>
            {
                options.Contributers.Add<OnPreApplicationInitializationModuleLifecycleContributer>();
                options.Contributers.Add<OnApplicationInitializationModuleLifecycleContributer>();
                options.Contributers.Add<OnPostApplicationInitializationModuleLifecycleContributer>();
                options.Contributers.Add<OnApplicationShutdownModuleLifecycleContributer>();
            });
        }
    }
}