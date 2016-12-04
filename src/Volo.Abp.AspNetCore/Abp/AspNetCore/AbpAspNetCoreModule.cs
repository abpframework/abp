using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.DependencyInjection;

namespace Volo.Abp.AspNetCore
{
    public class AbpAspNetCoreModule : IAbpModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAspNetCoreModule>();
        }
    }
}
