using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.MemoryDb;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp;

namespace Volo.Abp.AspNetCore.App
{
    [DependsOn(
        typeof(AbpAspNetCoreTestBaseModule),
        typeof(AbpMemoryDbTestModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpAspNetCoreMvcTestModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.AppServiceControllers.CreateFor(typeof(TestAppModule).Assembly);
                options.AppServiceControllers.UrlActionNameNormalizers.Add(new PhoneBookUrlActionNameNormalizer());
            });

            services.AddAssemblyOf<AbpAspNetCoreMvcTestModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseMvcWithDefaultRoute();
        }
    }
}
