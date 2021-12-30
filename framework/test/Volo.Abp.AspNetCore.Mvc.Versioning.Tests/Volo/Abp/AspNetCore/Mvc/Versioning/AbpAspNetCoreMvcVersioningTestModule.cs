using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<AbpAspNetCoreMvcOptions>(options =>
            {
                //2.0 Version
                options.ConventionalControllers.Create(typeof(AbpAspNetCoreMvcVersioningTestModule).Assembly, opts =>
                {
                    opts.TypePredicate = t => t.Namespace == typeof(Volo.Abp.AspNetCore.Mvc.Versioning.App.v2.TodoAppService).Namespace;
                    opts.ApiVersions.Add(new ApiVersion(2, 0));
                });

                //1.0 Compatibility version
                options.ConventionalControllers.Create(typeof(AbpAspNetCoreMvcVersioningTestModule).Assembly, opts =>
                {
                    opts.TypePredicate = t => t.Namespace == typeof(Volo.Abp.AspNetCore.Mvc.Versioning.App.v1.TodoAppService).Namespace;
                    opts.ApiVersions.Add(new ApiVersion(1, 0));
                });
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var preActions = context.Services.GetPreConfigureActions<AbpAspNetCoreMvcOptions>();
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                preActions.Configure(options);
            });

            context.Services.AddAbpApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;

                //options.ApiVersionReader = new HeaderApiVersionReader("api-version"); //Supports header too
                //options.ApiVersionReader = new MediaTypeApiVersionReader(); //Supports accept header too

                options.ConfigureAbp(preActions.Configure());
            });

            context.Services.AddHttpClientProxies(typeof(AbpAspNetCoreMvcVersioningTestModule).Assembly);

            Configure<AbpRemoteServiceOptions>(options =>
            {
                options.RemoteServices.Default = new RemoteServiceConfiguration("/");
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            app.UseRouting();
            app.UseConfiguredEndpoints();
        }
    }
}
