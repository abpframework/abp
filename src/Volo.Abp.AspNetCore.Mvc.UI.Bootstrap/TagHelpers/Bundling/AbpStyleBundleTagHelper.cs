using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Bundling
{
    //TODO: Move to Volo.Abp.AspNetCore.Mvc.UI package.

    [HtmlTargetElement("abp-style-bundle", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AbpStyleBundleTagHelper : AbpTagHelper<AbpStyleBundleTagHelper, AbpStyleBundleTagHelperService>
    {
        public string Name { get; set; }

        public AbpStyleBundleTagHelper(AbpStyleBundleTagHelperService service)
            : base(service)
        {
        }
    }
}
