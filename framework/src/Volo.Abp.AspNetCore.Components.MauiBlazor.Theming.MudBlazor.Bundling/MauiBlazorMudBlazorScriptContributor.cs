using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.MudBlazor.Bundling;

public class MauiBlazorMudBlazorScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("_content/MudBlazor/MudBlazor.min.js");
        context.Files.AddIfNotContains("_content/Volo.Abp.MudBlazorUI/abp-mud-popover-patch.js");
        context.Files.AddIfNotContains("_content/Volo.Abp.MudBlazorUI/abp-mud-ripple-patch.js");
        context.Files.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/abp.js");
        context.Files.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/lang-utils.js");
    }
}
