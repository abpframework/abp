using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.StarRatingSvg;

public class StarRatingSvgStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/libs/star-rating-svg/css/star-rating-svg.css");
    }
}
