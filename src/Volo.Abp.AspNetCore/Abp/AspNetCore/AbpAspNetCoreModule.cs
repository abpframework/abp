using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.Modularity;
using Volo.DependencyInjection;

namespace Volo.Abp.AspNetCore
{
    public class AbpAspNetCoreModule : IAbpModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAspNetCoreModule>();
            services.AddSingleton<IModuleInitializer, AspNetCoreModuleInitializer>();
        }
    }
}
