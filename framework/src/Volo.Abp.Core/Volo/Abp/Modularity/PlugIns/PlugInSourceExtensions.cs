using System;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Logging;

namespace Volo.Abp.Modularity.PlugIns
{
    public static class PlugInSourceExtensions
    {
        [NotNull]
        public static Type[] GetModulesWithAllDependencies([NotNull] this IPlugInSource plugInSource, IInitLogger logger)
        {
            Check.NotNull(plugInSource, nameof(plugInSource));

            return plugInSource
                .GetModules()
                .SelectMany(type => AbpModuleHelper.FindAllModuleTypes(type, logger))
                .Distinct()
                .ToArray();
        }
    }
}