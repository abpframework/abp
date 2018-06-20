using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Modularity
{
    public interface IModuleContainer
    {
        [NotNull]
        IReadOnlyList<IAbpModuleDescriptor> Modules { get; }
    }
}