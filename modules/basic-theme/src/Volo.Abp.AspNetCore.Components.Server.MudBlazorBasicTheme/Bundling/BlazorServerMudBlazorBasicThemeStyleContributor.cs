using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Components.Server.MudBlazorBasicTheme.Bundling;

public class BlazorServerMudBlazorBasicThemeStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/Volo.Abp.AspNetCore.Components.Web.MudBlazorBasicTheme/themes/basic/main.css");
    }
}
