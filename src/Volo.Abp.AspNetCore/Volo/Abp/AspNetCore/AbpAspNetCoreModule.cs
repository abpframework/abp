using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore
{
    public class AbpAspNetCoreModule : IAbpModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddObjectAccessor<IApplicationBuilder>();
            services.AddAssemblyOf<AbpAspNetCoreModule>();
        }
    }
}
