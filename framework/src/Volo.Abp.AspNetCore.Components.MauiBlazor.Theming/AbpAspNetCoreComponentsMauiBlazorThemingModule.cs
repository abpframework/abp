using Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Theming;

[DependsOn(
    typeof(AbpAspNetCoreComponentsMauiBlazorThemingBundlingModule),
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpAspNetCoreComponentsMauiBlazorModule)
)]
public class AbpAspNetCoreComponentsMauiBlazorThemingModule : AbpModule
{

}
