using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.FontAwesome;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Components.Server.Theming.Bundling
{
    [DependsOn(
        typeof(BootstrapStyleContributor),
        typeof(FontAwesomeStyleContributor)
    )]
    public class BlazorGlobalStyleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/_content/Blazorise/blazorise.css");
            context.Files.AddIfNotContains("/_content/Blazorise.Bootstrap5/blazorise.bootstrap5.css");
            context.Files.AddIfNotContains("/_content/Blazorise.Snackbar/blazorise.snackbar.css");
        }
    }
}