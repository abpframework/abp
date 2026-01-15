using Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor.Bundling;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor;

[DependsOn(
    typeof(AbpAspNetCoreComponentsServerModule),
    typeof(AbpAspNetCoreMvcUiPackagesModule),
    typeof(AbpAspNetCoreComponentsWebThemingMudBlazorModule),
    typeof(AbpAspNetCoreMvcUiBundlingModule)
    )]
public class AbpAspNetCoreComponentsServerThemingMudBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(BlazorStandardBundles.Styles.Global, bundle =>
                {
                    bundle.AddContributors(typeof(BlazorServerMudBlazorStyleContributor));
                });

            options
                .ScriptBundles
                .Add(BlazorStandardBundles.Scripts.Global, bundle =>
                {
                    bundle.AddContributors(typeof(BlazorServerMudBlazorScriptContributor));
                });
        });
    }
}
