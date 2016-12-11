using System;
using System.Collections.Generic;

namespace Volo.Abp.Modularity.PlugIns
{
    public interface IPlugInSource
    {
        List<Type> GetModules();
    }
}