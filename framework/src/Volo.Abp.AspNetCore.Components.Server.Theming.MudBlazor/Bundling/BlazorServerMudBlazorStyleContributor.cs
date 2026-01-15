using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Components.Server.Theming.MudBlazor.Bundling;

public class BlazorServerMudBlazorStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        //TODO@MudBlazor: Do we need this public font ?
        context.Files.AddIfNotContains("https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&display=swap");
        context.Files.AddIfNotContains("/_content/MudBlazor/MudBlazor.min.css");
        context.Files.AddIfNotContains("/_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/css/abp.css");
        context.Files.AddIfNotContains("/_content/Volo.Abp.MudBlazorUI/volo.abp.mudblazorui.css");
    }
}
