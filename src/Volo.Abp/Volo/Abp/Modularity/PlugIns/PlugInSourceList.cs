using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Modularity.PlugIns
{
    public class PlugInSourceList : List<IPlugInSource>
    {
        internal List<Type> GetAllModules()
        {
            return this
                .SelectMany(pluginSource => pluginSource.GetModulesWithAllDependencies())
                .Distinct()
                .ToList();
        }
    }
}