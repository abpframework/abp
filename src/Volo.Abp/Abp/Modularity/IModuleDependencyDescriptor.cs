using System;

namespace Volo.Abp.Modularity
{
    public interface IModuleDependencyDescriptor
    {
        Type[] GetDependedModuleTypes();
    }
}