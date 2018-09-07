using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Modularity;
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
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                //2.0 Version
                options.ConventionalControllers.Create(typeof(AbpAspNetCoreMvcVersioningTestModule).Assembly, opts =>
                {
                    opts.TypePredicate = t => t.Namespace == typeof(Volo.Abp.AspNetCore.Mvc.Versioning.App.TodoAppService).Namespace;
                    opts.ApiVersions.Add(new ApiVersion(2, 0));
                });

                //1.0 Compatability version
                options.ConventionalControllers.Create(typeof(AbpAspNetCoreMvcVersioningTestModule).Assembly, opts =>
                {
                    opts.TypePredicate = t => t.Namespace == typeof(Volo.Abp.AspNetCore.Mvc.Versioning.App.Compat.TodoAppService).Namespace;
                    opts.ApiVersions.Add(new ApiVersion(1, 0));
                });
            });

            context.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;

                //options.ApiVersionReader = new HeaderApiVersionReader("api-version"); //Supports header too
                //options.ApiVersionReader = new MediaTypeApiVersionReader(); //Supports accept header too

                options.ConfigureAbp(context.Services);
            });

            context.Services.AddHttpClientProxies(typeof(AbpAspNetCoreMvcVersioningTestModule).Assembly);

            context.Services.Configure<RemoteServiceOptions>(options =>
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
