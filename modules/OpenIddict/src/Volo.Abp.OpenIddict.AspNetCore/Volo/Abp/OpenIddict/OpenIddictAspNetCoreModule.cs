using Microsoft.AspNetCore.Mvc.Razor;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.OpenIddict;

[DependsOn(
    typeof(AbpAspNetCoreMvcModule),
    typeof(OpenIddictDomainModule)
)]
public class OpenIddictAspNetCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationFormats.Add("/Volo/Abp/OpenIddict/Views/{1}/{0}.cshtml");
            options.ViewLocationFormats.Add("/Volo/Abp/OpenIddict/Views/Authorization/{0}.cshtml");
        });
    }
}
