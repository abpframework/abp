using System.Collections.Generic;

namespace Volo.Abp.Modularity
{
    public interface IModuleManager
    {
        IReadOnlyList<AbpModuleDescriptor> Modules { get; }

        void InitializeModules();

        void ShutdownModules();
    }
}
