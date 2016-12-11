using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity.PlugIns;
using Volo.ExtensionMethods.Collections.Generic;

namespace Volo.Abp.Modularity
{
    public class ModuleLoader : IModuleLoader
    {
        public IReadOnlyList<AbpModuleDescriptor> Modules => _modules.ToImmutableList();
        private readonly List<AbpModuleDescriptor> _modules;

        public ModuleLoader()
        {
            _modules = new List<AbpModuleDescriptor>();
        }

        public virtual void LoadAll(IServiceCollection services, Type startupModuleType, PlugInSourceList plugInSources)
        {
            if (_modules.Any())
            {
                throw new InvalidOperationException($"{nameof(LoadAll)} should be called only once!");
            }

            FillModules(services, startupModuleType, plugInSources);
            SetModuleDependencies();
            SortByDependency(startupModuleType);
            ConfigureServices(services);
        }

        private void FillModules(IServiceCollection services, Type startupModuleType, PlugInSourceList plugInSources)
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
                _modules.Add(CreateModuleDescriptor(services, moduleType));
            }
        }

        private void SetModuleDependencies()
        {
            foreach (var module in Modules)
            {
                SetModuleDependencies(module);
            }
        }
        
        private void SortByDependency(Type startupModuleType)
        {
            _modules.SortByDependencies(m => m.Dependencies);
            _modules.MoveItem(m => m.Type == typeof(AbpKernelModule), 0);
            _modules.MoveItem(m => m.Type == startupModuleType, _modules.Count - 1);
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

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            foreach (var module in _modules)
            {
                module.Instance.ConfigureServices(services);
            }
        }
        
        protected virtual void SetModuleDependencies(AbpModuleDescriptor module)
        {
            foreach (var dependedModuleType in AbpModuleFinder.FindDependedModuleTypes(module.Type))
            {
                var dependedModule = _modules.FirstOrDefault(m => m.Type == dependedModuleType);
                if (dependedModule == null)
                {
                    throw new AbpException("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + module.Type.AssemblyQualifiedName);
                }

                module.Dependencies.AddIfNotContains(dependedModule);
            }
        }
    }
}