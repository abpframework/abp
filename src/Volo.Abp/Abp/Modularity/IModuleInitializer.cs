using Volo.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public interface IModuleInitializer : ISingletonDependency
    {
        void Initialize(IAbpModule module);
    }
}
