using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Modularity
{
    internal static class AbpModuleHelper
    {
        public static List<Type> FindAllModuleTypes(Type startupModuleType, ILogger logger)
        {
            var moduleTypes = new List<Type>();
            logger.Log(LogLevel.Information, "Loaded ABP modules:");
            AddModuleAndDependenciesResursively(moduleTypes, startupModuleType, logger);
            return moduleTypes;
        }

        public static List<Type> FindDependedModuleTypes(Type moduleType)
        {
            AbpModule.CheckAbpModuleType(moduleType);

            var dependencies = new List<Type>();

            var dependencyDescriptors = moduleType
                .GetCustomAttributes()
                .OfType<IDependedTypesProvider>();

            foreach (var descriptor in dependencyDescriptors)
            {
                foreach (var dependedModuleType in descriptor.GetDependedTypes())
                {
                    dependencies.AddIfNotContains(dependedModuleType);
                }
            }

            return dependencies;
        }

        private static void AddModuleAndDependenciesResursively(
            List<Type> moduleTypes,
            Type moduleType,
            ILogger logger,
            int depth = 0)
        {
            AbpModule.CheckAbpModuleType(moduleType);

            if (moduleTypes.Contains(moduleType))
            {
                return;
            }

            moduleTypes.Add(moduleType);
            logger.Log(LogLevel.Information, $"{new string(' ', depth * 2)}- {moduleType.FullName}");

            foreach (var dependedModuleType in FindDependedModuleTypes(moduleType))
            {
                AddModuleAndDependenciesResursively(moduleTypes, dependedModuleType, logger, depth + 1);
            }
        }
    }
}
