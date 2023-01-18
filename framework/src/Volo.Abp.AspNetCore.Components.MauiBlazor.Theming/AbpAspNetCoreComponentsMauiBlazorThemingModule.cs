using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Theming;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule),
    typeof(AbpAspNetCoreComponentsMauiBlazorModule)
)]
public class AbpAspNetCoreComponentsMauiBlazorThemingModule : AbpModule
{

}
