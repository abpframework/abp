using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.AspNetCore.Routing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Localization;

[DependsOn(typeof(AbpAspNetCoreTestModule))]
public class RouteBasedCultureTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpRequestLocalizationOptions>(options =>
        {
            options.UseRouteBasedCulture = true;
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseRouting();
        app.UseAbpRequestLocalization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("{culture}/culture", async ctx =>
            {
                await ctx.Response.WriteAsync(CultureInfo.CurrentCulture.Name);
            });

            endpoints.MapGet("culture", async ctx =>
            {
                await ctx.Response.WriteAsync(CultureInfo.CurrentCulture.Name);
            });

            endpoints.MapGet("api/data", async ctx =>
            {
                await ctx.Response.WriteAsync(CultureInfo.CurrentCulture.Name);
            });
        });
    }
}
