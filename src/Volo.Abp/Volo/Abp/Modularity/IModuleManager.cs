using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Modularity
{
    public interface IModuleManager
    {
        [NotNull]
        IReadOnlyList<AbpModuleDescriptor> Modules { get; }

        void InitializeModules([NotNull] ApplicationInitializationContext context);

        void ShutdownModules([NotNull] ApplicationShutdownContext context);
    }
}
