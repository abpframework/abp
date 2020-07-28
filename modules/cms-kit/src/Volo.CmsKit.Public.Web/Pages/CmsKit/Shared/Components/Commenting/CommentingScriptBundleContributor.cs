using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Bootstrap;
using Volo.Abp.Modularity;

namespace Volo.CmsKit.Web.Pages.CmsKit.Shared.Components.Commenting
{
    public class CommentingScriptBundleContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/Pages/CmsKit/Shared/Components/Commenting/default.js");
        }
    }
}
