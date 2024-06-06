using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Theming.Bundling;

public class MauiStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.MauiBlazor.Theming/libs/bootstrap/css/bootstrap.min.css");
        context.Files.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.MauiBlazor.Theming/libs/fontawesome/css/all.css");
        context.Files.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/css/abp.css");
        context.Files.AddIfNotContains("_content/Volo.Abp.AspNetCore.Components.MauiBlazor.Theming/libs/flag-icon/css/flag-icon.css");
        context.Files.AddIfNotContains("_content/Blazorise/blazorise.css");
        context.Files.AddIfNotContains("_content/Blazorise.Bootstrap5/blazorise.bootstrap5.css");
        context.Files.AddIfNotContains("_content/Blazorise.Snackbar/blazorise.snackbar.css");
    }
}
