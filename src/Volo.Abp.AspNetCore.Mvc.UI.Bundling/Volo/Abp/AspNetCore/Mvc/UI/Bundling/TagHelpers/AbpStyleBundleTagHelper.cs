using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    //TODO: Move to Volo.Abp.AspNetCore.Mvc.UI package.

    [HtmlTargetElement("abp-style-bundle", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AbpStyleBundleTagHelper : AbpTagHelper<AbpStyleBundleTagHelper, AbpStyleBundleTagHelperService>, IBundleTagHelper
    {
        public string Name { get; set; }

        public AbpStyleBundleTagHelper(AbpStyleBundleTagHelperService service)
            : base(service)
        {
        }
    }
}
