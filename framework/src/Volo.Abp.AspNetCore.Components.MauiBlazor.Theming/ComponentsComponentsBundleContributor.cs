using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.MauiBlazor.Theming;

public class ComponentsComponentsBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {
        context.Add("_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/abp.js");
        context.Add("_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/lang-utils.js");
    }

    public void AddStyles(BundleContext context)
    {
        context.BundleDefinitions.Insert(0, new BundleDefinition
        {
            Source = "_content/Volo.Abp.AspNetCore.Components.MauiBlazor.Theming/libs/bootstrap/css/bootstrap.min.css"
        });
        context.BundleDefinitions.Insert(1, new BundleDefinition
        {
            Source = "_content/Volo.Abp.AspNetCore.Components.MauiBlazor.Theming/libs/fontawesome/css/all.css"
        });

        context.Add("_content/Volo.Abp.AspNetCore.Components.MauiBlazor.Theming/libs/flag-icon/css/flag-icon.css");
        context.Add("_content/Blazorise/blazorise.css");
        context.Add("_content/Blazorise.Bootstrap5/blazorise.bootstrap5.css");
        context.Add("_content/Blazorise.Snackbar/blazorise.snackbar.css");
    }
}
