using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor.Bundling;

public class BlazorServerMudBlazorStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/_content/Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor/libs/fontawesome/css/all.css");
        context.Files.AddIfNotContains("/_content/MudBlazor/MudBlazor.min.css");
        context.Files.AddIfNotContains("/_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/css/abp.css");
        context.Files.AddIfNotContains("/_content/Volo.Abp.MudBlazorUI/volo.abp.mudblazorui.css");
        context.Files.AddIfNotContains("/_content/Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor/libs/flag-icon/css/flag-icon.css");
    }
}
