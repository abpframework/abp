using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.App
{
    [DependsOn(
        typeof(AbpAspNetCoreTestBaseModule)
        )]
    public class AppModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseMvcWithDefaultRoute();
        }
    }
}
