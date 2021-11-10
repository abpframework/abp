using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal;
using Volo.Abp.Logging;
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

            services.AddCoreServices();
            services.AddCoreAbpServices(this, options);

            Modules = LoadModules(services, options);
            ConfigureServices();
        }

        public virtual void Shutdown()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<IModuleManager>()
                    .ShutdownModules(new ApplicationShutdownContext(scope.ServiceProvider));
            }
        }

        public virtual void Dispose()
        {
            //TODO: Shutdown if not done before?
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
                WriteInitLogs(scope.ServiceProvider);
                scope.ServiceProvider
                    .GetRequiredService<IModuleManager>()
                    .InitializeModules(new ApplicationInitializationContext(scope.ServiceProvider));
            }
        }

        protected virtual void WriteInitLogs(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger<AbpApplicationBase>>();
            if (logger == null)
            {
                return;
            }

            var initLogger = serviceProvider.GetRequiredService<IInitLoggerFactory>().Create<AbpApplicationBase>();

            foreach (var entry in initLogger.Entries)
            {
                logger.Log(entry.LogLevel, entry.EventId, entry.State, entry.Exception, entry.Formatter);
            }

            initLogger.Entries.Clear();
        }

        protected virtual IReadOnlyList<IAbpModuleDescriptor> LoadModules(IServiceCollection services, AbpApplicationCreationOptions options)
        {
            return services
                .GetSingletonInstance<IModuleLoader>()
                .LoadModules(
                    services,
                    StartupModuleType,
                    options.PlugInSources
                );
        }

        //TODO: We can extract a new class for this
        protected virtual void ConfigureServices()
        {
            var context = new ServiceConfigurationContext(Services);
            Services.AddSingleton(context);

            foreach (var module in Modules)
            {
                if (module.Instance is AbpModule abpModule)
                {
                    abpModule.ServiceConfigurationContext = context;
                }
            }

            //PreConfigureServices
            foreach (var module in Modules.Where(m => m.Instance is IPreConfigureServices))
            {
                try
                {
                    ((IPreConfigureServices)module.Instance).PreConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new AbpInitializationException($"An error occurred during {nameof(IPreConfigureServices.PreConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }

            var assemblies = new HashSet<Assembly>();

            //ConfigureServices
            foreach (var module in Modules)
            {
                if (module.Instance is AbpModule abpModule)
                {
                    if (!abpModule.SkipAutoServiceRegistration)
                    {
                        var assembly = module.Type.Assembly;
                        if (!assemblies.Contains(assembly))
                        {
                            Services.AddAssembly(assembly);
                            assemblies.Add(assembly);
                        }
                    }
                }

                try
                {
                    module.Instance.ConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new AbpInitializationException($"An error occurred during {nameof(IAbpModule.ConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }

            //PostConfigureServices
            foreach (var module in Modules.Where(m => m.Instance is IPostConfigureServices))
            {
                try
                {
                    ((IPostConfigureServices)module.Instance).PostConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new AbpInitializationException($"An error occurred during {nameof(IPostConfigureServices.PostConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }

            foreach (var module in Modules)
            {
                if (module.Instance is AbpModule abpModule)
                {
                    abpModule.ServiceConfigurationContext = null;
                }
            }
        }
    }
}
