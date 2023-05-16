using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.FlagIconCss;

public class FlagIconCssStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        if (context.FileProvider.GetFileInfo("/libs/flag-icons/css/flag-icons.min.css").Exists)
        {
            context.Files.AddIfNotContains("/libs/flag-icons/css/flag-icons.min.css");
        }
        else if (context.FileProvider.GetFileInfo("/libs/flag-icon-css/css/flag-icons.min.css").Exists)
        {
            context.Files.AddIfNotContains("/libs/flag-icon-css/css/flag-icons.min.css");
        }
    }
}
