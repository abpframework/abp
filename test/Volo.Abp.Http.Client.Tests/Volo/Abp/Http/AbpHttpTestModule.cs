using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.App;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp.Application;

namespace Volo.Abp.Http
{
    [DependsOn(typeof(AbpAspNetCoreMvcTestModule), typeof(AbpHttpClientModule))]
    public class AbpHttpTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpHttpTestModule>();
            services.AddHttpClientProxy<IPeopleAppService>("/");
        }
    }
}
