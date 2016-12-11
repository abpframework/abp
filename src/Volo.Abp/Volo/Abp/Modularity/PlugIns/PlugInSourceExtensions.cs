using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Modularity.PlugIns
{
    public static class PlugInSourceExtensions
    {
        public static List<Type> GetModulesWithAllDependencies(this IPlugInSource plugInSource)
        {
            return plugInSource
                .GetModules()
                .SelectMany(AbpModuleFinder.FindAllModuleTypes)
                .Distinct()
                .ToList();
        }
    }
}