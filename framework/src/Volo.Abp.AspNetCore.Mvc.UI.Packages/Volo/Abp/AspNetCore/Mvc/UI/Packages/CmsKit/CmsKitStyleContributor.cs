using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.StarRatingSvg;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Mvc.UI.Packages.CmsKit
{
    [DependsOn(typeof(StarRatingSvgStyleContributor))]
    public class CmsKitStyleContributor : BundleContributor
    {
    }
}