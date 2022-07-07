using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.Modularity.PlugIns;

public class PlugInSourceList : List<IPlugInSource>
{
    [NotNull]
    internal Type[] GetAllModules(ILogger logger, IConfiguration config)
    {
        return this
            .SelectMany(pluginSource => pluginSource.GetModulesWithAllDependencies(logger, config))
            .Distinct()
            .ToArray();
    }
}
