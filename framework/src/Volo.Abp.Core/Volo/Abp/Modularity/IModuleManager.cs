using JetBrains.Annotations;

namespace Volo.Abp.Modularity;

public interface IModuleManager
{
    void InitializeModules([NotNull] ApplicationInitializationContext context);

    void ShutdownModules([NotNull] ApplicationShutdownContext context);
}
