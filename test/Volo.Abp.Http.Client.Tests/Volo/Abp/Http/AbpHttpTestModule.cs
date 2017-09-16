using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.App;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.DynamicProxying;
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
            services.AddHttpClientProxy<IRegularTestController>("/");

            services.Configure<MvcOptions>(options =>
            {
                
            });
        }
    }
}
