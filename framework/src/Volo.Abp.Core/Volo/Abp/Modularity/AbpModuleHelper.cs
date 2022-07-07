using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Modularity;

internal static class AbpModuleHelper
{
    public static List<Type> FindAllModuleTypes(Type startupModuleType, ILogger logger, IConfiguration config)
    {
        var moduleTypes = new List<Type>();
        logger.Log(LogLevel.Information, "Loaded ABP modules:");
        AddModuleAndDependenciesRecursively(moduleTypes, startupModuleType, logger, config);
        return moduleTypes;
    }

    public static List<Type> FindDependedModuleTypes(Type moduleType, IConfiguration config)
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

        if (typeof(IDynamicDependsOnProvider).IsAssignableFrom(moduleType))
        {
            var typesProvider = Activator.CreateInstance(moduleType) as IDynamicDependsOnProvider;
            var dependencyTypes = typesProvider.GetDependencyTypes(config);
            foreach (var dependencyType in dependencyTypes)
            {
                AbpModule.CheckAbpModuleType(moduleType);

                dependencies.AddIfNotContains(dependencyType);
            }
        }

        return dependencies;
    }

    private static void AddModuleAndDependenciesRecursively(
        List<Type> moduleTypes,
        Type moduleType,
        ILogger logger,
        IConfiguration config,
        int depth = 0)
    {
        AbpModule.CheckAbpModuleType(moduleType);

        if (moduleTypes.Contains(moduleType))
        {
            return;
        }

        moduleTypes.Add(moduleType);
        logger.Log(LogLevel.Information, $"{new string(' ', depth * 2)}- {moduleType.FullName}");

        foreach (var dependedModuleType in FindDependedModuleTypes(moduleType, config))
        {
            AddModuleAndDependenciesRecursively(moduleTypes, dependedModuleType, logger, config, depth + 1);
        }
    }
}
