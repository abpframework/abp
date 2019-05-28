using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public interface IAbpModule : ISingletonDependency
    {
        void ConfigureServices(ServiceConfigurationContext context);
    }
}
