using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
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

        public virtual void LoadAll(IServiceCollection services, Type startupModuleType)
        {
            if (_modules.Any())
            {
                throw new InvalidOperationException($"{nameof(LoadAll)} should be called only once!");
            }

            FillModules(services, startupModuleType);
            SetModuleDependencies();
            SortByDependency(startupModuleType);
            ConfigureServices(services);
        }

        private void FillModules(IServiceCollection services, Type startupModuleType)
        {
            foreach (var moduleType in FindAllModuleTypes(startupModuleType).Distinct())
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

        protected virtual IEnumerable<Type> FindAllModuleTypes(Type startupModuleType)
        {
            var moduleTypes = new List<Type>();
            AddModuleAndDependenciesResursively(moduleTypes, startupModuleType);
            moduleTypes.AddIfNotContains(typeof(AbpKernelModule));
            return moduleTypes;
        }

        protected virtual AbpModuleDescriptor CreateModuleDescriptor(IServiceCollection services, Type moduleType)
        {
            return new AbpModuleDescriptor(moduleType, CreateAndRegisterModule(services, moduleType));
        }

        private IAbpModule CreateAndRegisterModule(IServiceCollection services, Type moduleType)
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

        protected virtual void AddModuleAndDependenciesResursively(List<Type> moduleTypes, Type moduleType)
        {
            CheckAbpModuleType(moduleType);

            if (moduleTypes.Contains(moduleType))
            {
                return;
            }

            moduleTypes.Add(moduleType);

            foreach (var dependedModuleType in FindDependedModuleTypes(moduleType))
            {
                AddModuleAndDependenciesResursively(moduleTypes, dependedModuleType);
            }
        }

        protected virtual List<Type> FindDependedModuleTypes(Type moduleType)
        {
            CheckAbpModuleType(moduleType);

            var dependencies = new List<Type>();

            var dependencyDescriptors = moduleType
                .GetTypeInfo()
                .GetCustomAttributes()
                .OfType<IDependedModuleTypesProvider>();

            foreach (var descriptor in dependencyDescriptors)
            {
                foreach (var dependedModuleType in descriptor.GetDependedModuleTypes())
                {
                    dependencies.AddIfNotContains(dependedModuleType);
                }
            }

            return dependencies;
        }

        protected virtual void SetModuleDependencies(AbpModuleDescriptor module)
        {
            foreach (var dependedModuleType in FindDependedModuleTypes(module.Type))
            {
                var dependedModule = _modules.FirstOrDefault(m => m.Type == dependedModuleType);
                if (dependedModule == null)
                {
                    throw new AbpException("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + module.Type.AssemblyQualifiedName);
                }

                module.Dependencies.AddIfNotContains(dependedModule);
            }
        }

        protected static void CheckAbpModuleType(Type moduleType)
        {
            if (!IsAbpModule(moduleType))
            {
                throw new ArgumentException("Given type is not an ABP module: " + moduleType.AssemblyQualifiedName);
            }
        }

        protected static bool IsAbpModule(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(IAbpModule).GetTypeInfo().IsAssignableFrom(type);
        }
    }
}