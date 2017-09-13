using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.App;
using Volo.Abp.Modularity;

namespace Volo.Abp.Http
{
    [DependsOn(typeof(AbpAspNetCoreMvcTestModule), typeof(AbpHttpModule))]
    public class AbpHttpTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAspNetCoreMvcTestModule>();
        }
    }
}
