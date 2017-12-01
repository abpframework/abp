using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.MemoryDb;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp;

namespace Volo.Abp.AspNetCore.Mvc
{
    [DependsOn(
        typeof(AbpAspNetCoreTestBaseModule),
        typeof(AbpMemoryDbTestModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpAspNetCoreMvcTestModule : AbpModule
    {
        public override void PreConfigureServices(IServiceCollection services)
        {
            services.PreConfigure<IMvcBuilder>(builder =>
            {
                builder.AddViewLocalization();
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(); //TODO: Move to the framework..?

            services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(TestAppModule).Assembly, opts =>
                {
                    opts.UrlActionNameNormalizer = context =>
                        string.Equals(context.ActionNameInUrl, "phone", StringComparison.OrdinalIgnoreCase)
                            ? "phones"
                            : context.ActionNameInUrl;
                });
            });

            services.AddAssemblyOf<AbpAspNetCoreMvcTestModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseUnitOfWork();
            app.UseMvcWithDefaultRoute();
        }
    }
}
