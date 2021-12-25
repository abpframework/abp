using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Internal;
using Volo.Abp.Logging;
using Volo.Abp.Modularity;

namespace Volo.Abp;

public abstract class AbpApplicationBase : IAbpApplication
{
    [NotNull]
    public Type StartupModuleType { get; }

    public IServiceProvider ServiceProvider { get; private set; }

    public IServiceCollection Services { get; }

    public IReadOnlyList<IAbpModuleDescriptor> Modules { get; }

    private bool _configuredServices;

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

        if (!options.SkipConfigureServices)
        {
            ConfigureServices();
        }
    }

    public virtual async Task ShutdownAsync()
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            await scope.ServiceProvider
                .GetRequiredService<IModuleManager>()
                .ShutdownModulesAsync(new ApplicationShutdownContext(scope.ServiceProvider));
        }
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

    protected virtual async Task InitializeModulesAsync()
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            WriteInitLogs(scope.ServiceProvider);
            await scope.ServiceProvider
                .GetRequiredService<IModuleManager>()
                .InitializeModulesAsync(new ApplicationInitializationContext(scope.ServiceProvider));
        }
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
    public virtual async Task ConfigureServicesAsync()
    {
        CheckMultipleConfigureServices();
        
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
                await ((IPreConfigureServices)module.Instance).PreConfigureServicesAsync(context);
            }
            catch (Exception ex)
            {
                throw new AbpInitializationException($"An error occurred during {nameof(IPreConfigureServices.PreConfigureServicesAsync)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
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
                await module.Instance.ConfigureServicesAsync(context);
            }
            catch (Exception ex)
            {
                throw new AbpInitializationException($"An error occurred during {nameof(IAbpModule.ConfigureServicesAsync)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }

        //PostConfigureServices
        foreach (var module in Modules.Where(m => m.Instance is IPostConfigureServices))
        {
            try
            {
                await ((IPostConfigureServices)module.Instance).PostConfigureServicesAsync(context);
            }
            catch (Exception ex)
            {
                throw new AbpInitializationException($"An error occurred during {nameof(IPostConfigureServices.PostConfigureServicesAsync)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
            }
        }

        foreach (var module in Modules)
        {
            if (module.Instance is AbpModule abpModule)
            {
                abpModule.ServiceConfigurationContext = null;
            }
        }

        _configuredServices = true;
    }

    private void CheckMultipleConfigureServices()
    {
        if (_configuredServices)
        {
            throw new AbpInitializationException("Services have already been configured! If you call ConfigureServicesAsync method, you must have set AbpApplicationCreationOptions.SkipConfigureServices tu true before.");
        }
    }

    //TODO: We can extract a new class for this
    public virtual void ConfigureServices()
    {
        CheckMultipleConfigureServices();
        
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

        _configuredServices = true;
    }
}
