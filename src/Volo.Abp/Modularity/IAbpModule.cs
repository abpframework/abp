using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public interface IAbpModule
    {
        void ConfigureServices(IServiceCollection services);
    }
}
