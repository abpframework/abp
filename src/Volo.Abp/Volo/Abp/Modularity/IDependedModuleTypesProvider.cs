using System;
using JetBrains.Annotations;

namespace Volo.Abp.Modularity
{
    public interface IDependedModuleTypesProvider
    {
        [NotNull]
        Type[] GetDependedModuleTypes();
    }
}