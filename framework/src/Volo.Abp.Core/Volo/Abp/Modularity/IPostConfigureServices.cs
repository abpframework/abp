using System.Threading.Tasks;

namespace Volo.Abp.Modularity;

public interface IPostConfigureServices
{
    Task PostConfigureServicesAsync(ServiceConfigurationContext context);

    void PostConfigureServices(ServiceConfigurationContext context);
}
