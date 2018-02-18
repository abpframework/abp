using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc.Authorization;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Authorization.Permissions;
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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MyClaimTestPolicy", policy =>
                {
                    policy.RequireClaim("MyCustomClaimType", "42");
                });
            });

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

            services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<TestPermissionDefinitionProvider>();
            });

            services.AddAssemblyOf<AbpAspNetCoreMvcTestModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseMiddleware<FakeAuthenticationMiddleware>();
            app.UseUnitOfWork();
            app.UseMvcWithDefaultRoute();
        }
    }
}
