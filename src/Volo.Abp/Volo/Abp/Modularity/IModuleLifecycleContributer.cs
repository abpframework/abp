using JetBrains.Annotations;
using Volo.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public interface IModuleLifecycleContributer : ISingletonDependency
    {
        void Initialize([NotNull] IAbpModule module);

        void Shutdown([NotNull] IAbpModule module);
    }
}
