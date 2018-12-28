using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public interface IModuleLifecycleContributor : ISingletonDependency
    {
        void Initialize([NotNull] ApplicationInitializationContext context, [NotNull] IAbpModule module);

        void Shutdown([NotNull] ApplicationShutdownContext context, [NotNull] IAbpModule module);
    }
}
