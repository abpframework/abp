using System;
using System.Collections.Generic;

namespace Volo.Abp.Modularity
{
    public interface IAbpModuleDescriptor
    {
        Type Type { get; }

        IAbpModule Instance { get; }

        bool IsLoadedAsPlugIn { get; }

        IReadOnlyList<IAbpModuleDescriptor> Dependencies { get; }
    }
}