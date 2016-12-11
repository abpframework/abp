using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc
{
    [DependsOn(typeof(AbpAspNetCoreModule))]
    public class AbpAspNetCoreMvcModule : IAbpModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddObjectAccessor<IApplicationBuilder>();
            services.AddAssemblyOf<AbpAspNetCoreMvcModule>();
        }
    }
}
