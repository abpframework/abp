using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Modularity;

public interface IModuleManager
{
    Task InitializeModulesAsync([NotNull] ApplicationInitializationContext context);

    void InitializeModules([NotNull] ApplicationInitializationContext context);

    Task ShutdownModulesAsync([NotNull] ApplicationShutdownContext context);

    void ShutdownModules([NotNull] ApplicationShutdownContext context);
}
