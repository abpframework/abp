using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    [HtmlTargetElement("abp-bundle-contributor", TagStructure = TagStructure.NormalOrSelfClosing, ParentTag = "abp-style-bundle")]
    [HtmlTargetElement("abp-bundle-contributor", TagStructure = TagStructure.NormalOrSelfClosing, ParentTag = "abp-script-bundle")]
    public class AbpBundleContributorTagHelper : AbpTagHelper<AbpBundleContributorTagHelper, AbpBundleContributorTagHelperService>
    {
        public Type Type { get; set; }

        public AbpBundleContributorTagHelper(AbpBundleContributorTagHelperService service)
            : base(service)
        {

        }
    }
}