using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Bundling;
using Volo.Abp.AspNetCore.Components.WebAssembly.Theming.MudBlazor;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;
using BlazorWebAssemblyStandardBundles = Volo.Abp.AspNetCore.Components.WebAssembly.Theming.Bundling.BlazorWebAssemblyStandardBundles;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.MudBlazorBasicTheme.Bundling;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingMudBlazorModule)
)]
public class AbpAspNetCoreComponentsWebAssemblyMudBlazorBasicThemeBundlingModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            var globalStyles = options.StyleBundles.Get(BlazorWebAssemblyStandardBundles.Styles.Global);
            globalStyles.AddContributors(typeof(WebAssemblyMudBlazorBasicThemeBundleStyleContributor));
        });
    }
}
