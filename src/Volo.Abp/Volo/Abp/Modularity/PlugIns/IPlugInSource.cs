using System;
using JetBrains.Annotations;

namespace Volo.Abp.Modularity.PlugIns
{
    public interface IPlugInSource
    {
        [NotNull]
        Type[] GetModules();
    }
}