using System;
using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Authorization;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.Localization.Resource;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Localization.Resources.AbpValidation;
using Volo.Abp.MemoryDb;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp;
using Volo.Abp.VirtualFileSystem;

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
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(MvcTestResource),
                    typeof(AbpAspNetCoreMvcTestModule).Assembly
                );
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAuthorization(options =>
            {
                options.AddPolicy("MyClaimTestPolicy", policy =>
                {
                    policy.RequireClaim("MyCustomClaimType", "42");
                });
            });

            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(TestAppModule).Assembly, opts =>
                {
                    opts.UrlActionNameNormalizer = urlActionNameNormalizerContext =>
                        string.Equals(urlActionNameNormalizerContext.ActionNameInUrl, "phone", StringComparison.OrdinalIgnoreCase)
                            ? "phones"
                            : urlActionNameNormalizerContext.ActionNameInUrl;
                });
            });

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpAspNetCoreMvcTestModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<MvcTestResource>("en")
                    .AddBaseTypes(
                        typeof(AbpUiResource),
                        typeof(AbpValidationResource)
                    ).AddVirtualJson("/Volo/Abp/AspNetCore/Mvc/Localization/Resource");

                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseAbpRequestLocalization();
            app.UseRouting();
            app.UseMiddleware<FakeAuthenticationMiddleware>();
            app.UseAuthorization();
            app.UseAuditing();
            app.UseUnitOfWork();
            app.UseMvcWithDefaultRouteAndArea();
        }
    }
}
