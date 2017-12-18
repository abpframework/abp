using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Volo.Abp.Modularity
{
    internal static class AbpModuleHelper
    {
        public static List<Type> FindAllModuleTypes(Type startupModuleType)
        {
            var moduleTypes = new List<Type>();
            AddModuleAndDependenciesResursively(moduleTypes, startupModuleType);
            moduleTypes.AddIfNotContains(typeof(AbpKernelModule));
            return moduleTypes;
        }

        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            AbpModule.CheckAbpModuleType(moduleType);

            var dependencies = new List<Type>();

            var dependencyDescriptors = moduleType
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

        private static void AddModuleAndDependenciesResursively(List<Type> moduleTypes, Type moduleType)
        {
            AbpModule.CheckAbpModuleType(moduleType);

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
    }
}
