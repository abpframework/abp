using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.TestBase
{
    [DependsOn(typeof(AbpAspNetCoreModule))]
    public class AbpAspNetCoreTestBaseModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAspNetCoreTestBaseModule>();
        }
    }
}