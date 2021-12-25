using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Modularity;

public interface IModuleLifecycleContributor : ITransientDependency
{
    Task InitializeAsync([NotNull] ApplicationInitializationContext context, [NotNull] IAbpModule module);

    void Initialize([NotNull] ApplicationInitializationContext context, [NotNull] IAbpModule module);

    Task ShutdownAsync([NotNull] ApplicationShutdownContext context, [NotNull] IAbpModule module);

    void Shutdown([NotNull] ApplicationShutdownContext context, [NotNull] IAbpModule module);
}
