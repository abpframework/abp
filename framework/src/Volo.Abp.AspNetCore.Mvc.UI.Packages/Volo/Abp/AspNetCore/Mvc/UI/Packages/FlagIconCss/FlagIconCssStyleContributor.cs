using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.FlagIconCss;

public class FlagIconCssStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/libs/flag-icon-css/css/flag-icon.min.css");
    }
}
