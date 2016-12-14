using Volo.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public interface IModuleLifecycleContributer : ISingletonDependency
    {
        void Initialize(IAbpModule module);

        void Shutdown(IAbpModule module);
    }
}
