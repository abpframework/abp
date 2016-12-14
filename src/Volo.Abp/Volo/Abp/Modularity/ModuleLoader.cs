using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity.PlugIns;
using Volo.ExtensionMethods.Collections.Generic;

namespace Volo.Abp.Modularity
{
    public class ModuleLoader : IModuleLoader
    {
        public AbpModuleDescriptor[] LoadModules(IServiceCollection services, Type startupModuleType, PlugInSourceList plugInSources)
        {
            var modules = new List<AbpModuleDescriptor>();

            FillModules(modules, services, startupModuleType, plugInSources);
            SetModuleDependencies(modules);
            SortByDependency(modules, startupModuleType);
            ConfigureServices(modules, services);

            return modules.ToArray();
        }

        protected virtual void FillModules(List<AbpModuleDescriptor> modules, IServiceCollection services, Type startupModuleType, PlugInSourceList plugInSources)
        {
            //All modules starting from the startup module
            var moduleTypes = AbpModuleFinder.FindAllModuleTypes(startupModuleType);

            //Add plugin modules
            foreach (var moduleType in plugInSources.GetAllModules())
            {
                moduleTypes.AddIfNotContains(moduleType);
            }

            //Create all modules
            foreach (var moduleType in moduleTypes)
            {
                modules.Add(CreateModuleDescriptor(services, moduleType));
            }
        }

        protected virtual void SetModuleDependencies(List<AbpModuleDescriptor> modules)
        {
            foreach (var module in modules)
            {
                SetModuleDependencies(modules, module);
            }
        }
        
        protected virtual void SortByDependency(List<AbpModuleDescriptor> modules, Type startupModuleType)
        {
            modules.SortByDependencies(m => m.Dependencies);
            modules.MoveItem(m => m.Type == typeof(AbpKernelModule), 0);
            modules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
        }

        protected virtual AbpModuleDescriptor CreateModuleDescriptor(IServiceCollection services, Type moduleType)
        {
            return new AbpModuleDescriptor(moduleType, CreateAndRegisterModule(services, moduleType));
        }

        protected virtual IAbpModule CreateAndRegisterModule(IServiceCollection services, Type moduleType)
        {
            var module = (IAbpModule) Activator.CreateInstance(moduleType);
            services.AddSingleton(moduleType, module);
            return module;
        }

        protected virtual void ConfigureServices(List<AbpModuleDescriptor> modules, IServiceCollection services)
        {
            foreach (var module in modules)
            {
                module.Instance.ConfigureServices(services);
            }
        }
        
        protected virtual void SetModuleDependencies(List<AbpModuleDescriptor> modules, AbpModuleDescriptor module)
        {
            foreach (var dependedModuleType in AbpModuleFinder.FindDependedModuleTypes(module.Type))
            {
                var dependedModule = modules.FirstOrDefault(m => m.Type == dependedModuleType);
                if (dependedModule == null)
                {
                    throw new AbpException("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + module.Type.AssemblyQualifiedName);
                }

                module.Dependencies.AddIfNotContains(dependedModule);
            }
        }
    }
}