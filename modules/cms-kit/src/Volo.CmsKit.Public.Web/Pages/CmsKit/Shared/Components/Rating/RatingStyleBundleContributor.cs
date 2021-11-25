using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.StarRatingSvg;
using Volo.Abp.Modularity;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Rating;

[DependsOn(typeof(StarRatingSvgStyleContributor))]
public class RatingStyleBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/Pages/CmsKit/Shared/Components/Rating/default.css");
    }
}
