using System.Threading.Tasks;

namespace Volo.Abp.Modularity;

public interface IAbpModule
{
    Task ConfigureServicesAsync(ServiceConfigurationContext context);

    void ConfigureServices(ServiceConfigurationContext context);
}
