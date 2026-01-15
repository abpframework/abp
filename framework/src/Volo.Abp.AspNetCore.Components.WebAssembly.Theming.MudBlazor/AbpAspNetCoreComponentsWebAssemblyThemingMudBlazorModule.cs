using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor;
using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Bundling;
using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor.Bundling;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingBundlingModule),
    typeof(AbpAspNetCoreComponentsWebThemingMudBlazorModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyModule)
)]
public class AbpAspNetCoreComponentsWebAssemblyThemingMudBlazorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(BlazorWebAssemblyStandardBundles.Styles.Global, bundle =>
                {
                    bundle.AddContributors(typeof(BlazorWebAssemblyMudBlazorStyleContributor));
                });

            options
                .ScriptBundles
                .Add(BlazorWebAssemblyStandardBundles.Scripts.Global, bundle =>
                {
                    bundle.AddContributors(typeof(BlazorWebAssemblyMudBlazorScriptContributor));
                });
        });
    }
}
