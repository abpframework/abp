using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Logging;

namespace Volo.Abp.Modularity.PlugIns
{
    public class PlugInSourceList : List<IPlugInSource>
    {
        [NotNull]
        internal Type[] GetAllModules(IInitLogger logger)
        {
            return this
                .SelectMany(pluginSource => pluginSource.GetModulesWithAllDependencies(logger))
                .Distinct()
                .ToArray();
        }
    }
}