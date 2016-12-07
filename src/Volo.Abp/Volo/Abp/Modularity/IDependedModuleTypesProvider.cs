using System;

namespace Volo.Abp.Modularity
{
    public interface IDependedModuleTypesProvider
    {
        Type[] GetDependedModuleTypes();
    }
}