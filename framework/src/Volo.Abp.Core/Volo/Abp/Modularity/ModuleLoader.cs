using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity.PlugIns;

namespace Volo.Abp.Modularity;

public class ModuleLoader : IModuleLoader
{
    public IAbpModuleDescriptor[] LoadModules(
        IServiceCollection services,
        Type startupModuleType,
        PlugInSourceList plugInSources)
    {
        Check.NotNull(services, nameof(services));
        Check.NotNull(startupModuleType, nameof(startupModuleType));
        Check.NotNull(plugInSources, nameof(plugInSources));

        var modules = GetDescriptors(services, startupModuleType, plugInSources);

        modules = SortByDependency(modules, startupModuleType);

        return modules.ToArray();
    }

    private List<IAbpModuleDescriptor> GetDescriptors(
        IServiceCollection services,
        Type startupModuleType,
        PlugInSourceList plugInSources)
    {
        var modules = new List<AbpModuleDescriptor>();

        FillModules(modules, services, startupModuleType, plugInSources);
        SetDependencies(modules);

        return modules.Cast<IAbpModuleDescriptor>().ToList();
    }

    protected virtual void FillModules(
        List<AbpModuleDescriptor> modules,
        IServiceCollection services,
        Type startupModuleType,
        PlugInSourceList plugInSources)
    {
        var logger = services.GetInitLogger<AbpApplicationBase>();

        //All modules starting from the startup module
        foreach (var moduleType in AbpModuleHelper.FindAllModuleTypes(startupModuleType, logger))
        {
            modules.Add(CreateModuleDescriptor(services, moduleType));
        }

        //Plugin modules
        foreach (var moduleType in plugInSources.GetAllModules(logger))
        {
            if (modules.Any(m => m.Type == moduleType))
            {
                continue;
            }

            modules.Add(CreateModuleDescriptor(services, moduleType, isLoadedAsPlugIn: true));
        }
    }

    protected virtual void SetDependencies(List<AbpModuleDescriptor> modules)
    {
        foreach (var module in modules)
        {
            SetDependencies(modules, module);
        }
    }

    protected virtual List<IAbpModuleDescriptor> SortByDependency(List<IAbpModuleDescriptor> modules, Type startupModuleType)
    {
        var sortedModules = modules.SortByDependencies(m => m.Dependencies);
        sortedModules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
        return sortedModules;
    }

    protected virtual AbpModuleDescriptor CreateModuleDescriptor(IServiceCollection services, Type moduleType, bool isLoadedAsPlugIn = false)
    {
        return new AbpModuleDescriptor(moduleType, CreateAndRegisterModule(services, moduleType), isLoadedAsPlugIn);
    }

    protected virtual IAbpModule CreateAndRegisterModule(IServiceCollection services, Type moduleType)
    {
        var module = (IAbpModule)Activator.CreateInstance(moduleType);
        services.AddSingleton(moduleType, module);
        return module;
    }

    protected virtual void SetDependencies(List<AbpModuleDescriptor> modules, AbpModuleDescriptor module)
    {
        foreach (var dependedModuleType in AbpModuleHelper.FindDependedModuleTypes(module.Type))
        {
            var dependedModule = modules.FirstOrDefault(m => m.Type == dependedModuleType);
            if (dependedModule == null)
            {
                throw new AbpException("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + module.Type.AssemblyQualifiedName);
            }

            module.AddDependency(dependedModule);
        }
    }
}
