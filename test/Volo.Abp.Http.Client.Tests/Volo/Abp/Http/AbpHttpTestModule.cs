using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.App;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.DynamicProxying;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp;

namespace Volo.Abp.Http
{
    [DependsOn(typeof(AbpAspNetCoreMvcTestModule), typeof(AbpHttpClientModule))]
    public class AbpHttpTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpHttpTestModule>();

            services.AddHttpClientProxies(typeof(TestAppModule).Assembly, "Default");
            services.AddHttpClientProxy<IRegularTestController>("Default");

            services.Configure<RemoteServiceOptions>(options =>
            {
                options.RemoteServices.Default = new RemoteServiceConfiguration("/");
            });
        }
    }
}
