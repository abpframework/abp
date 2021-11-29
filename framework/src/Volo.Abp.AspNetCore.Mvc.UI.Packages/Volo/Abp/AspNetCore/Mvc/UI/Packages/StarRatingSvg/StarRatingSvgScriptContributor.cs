using System.Collections.Generic;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.StarRatingSvg
{
    [DependsOn(typeof(JQueryScriptContributor))]
    public class StarRatingSvgScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.AddIfNotContains("/libs/star-rating-svg/js/jquery.star-rating-svg.min.js");
        }
    }
}
