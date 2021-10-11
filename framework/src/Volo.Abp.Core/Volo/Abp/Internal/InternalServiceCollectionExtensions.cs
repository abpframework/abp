﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Logging;
using Volo.Abp.Modularity;
using Volo.Abp.Reflection;
using Volo.Abp.SimpleStateChecking;

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

            if (!services.IsAdded<IConfiguration>())
            {
                services.ReplaceConfiguration(
                    ConfigurationHelper.BuildConfiguration(
                        applicationCreationOptions.Configuration
                    )
                );
            }

            services.TryAddSingleton<IModuleLoader>(moduleLoader);
            services.TryAddSingleton<IAssemblyFinder>(assemblyFinder);
            services.TryAddSingleton<ITypeFinder>(typeFinder);
            services.TryAddSingleton<IInitLoggerFactory>(new DefaultInitLoggerFactory());

            services.AddAssemblyOf<IAbpApplication>();

            services.AddTransient(typeof(ISimpleStateCheckerManager<>), typeof(SimpleStateCheckerManager<>));

            services.Configure<AbpModuleLifecycleOptions>(options =>
            {
                options.Contributors.Add<OnPreApplicationInitializationModuleLifecycleContributor>();
                options.Contributors.Add<OnApplicationInitializationModuleLifecycleContributor>();
                options.Contributors.Add<OnPostApplicationInitializationModuleLifecycleContributor>();
                options.Contributors.Add<OnApplicationShutdownModuleLifecycleContributor>();
            });
        }
    }
}
