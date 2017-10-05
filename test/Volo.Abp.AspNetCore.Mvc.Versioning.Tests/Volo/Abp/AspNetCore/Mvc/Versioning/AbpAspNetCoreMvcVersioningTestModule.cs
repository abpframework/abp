using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc.Versioning.App;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.Versioning
{
    [DependsOn(
        typeof(AbpAspNetCoreTestBaseModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        typeof(AbpHttpClientModule)
        )]
    public class AbpAspNetCoreMvcVersioningTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.AppServiceControllers.Create(typeof(AbpAspNetCoreMvcVersioningTestModule).Assembly, opts =>
                {
                    
                });
            });

            services.AddAssemblyOf<AbpAspNetCoreMvcVersioningTestModule>();

            services.AddHttpClientProxy<ITodoAppService>();

            services.Configure<RemoteServiceOptions>(options =>
            {
                options.RemoteServices.Default = new RemoteServiceConfiguration("/");
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseMvcWithDefaultRoute();
        }
    }
}
