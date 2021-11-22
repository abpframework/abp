using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Modularity;

public interface IPreConfigureServices
{
    void PreConfigureServices(ServiceConfigurationContext context);
}
