using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.HighlightJs;

public class HighlightJsStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        //TODO: Make this configurable
        context.Files.AddIfNotContains("/libs/highlight.js/styles/github.min.css");
    }
}
