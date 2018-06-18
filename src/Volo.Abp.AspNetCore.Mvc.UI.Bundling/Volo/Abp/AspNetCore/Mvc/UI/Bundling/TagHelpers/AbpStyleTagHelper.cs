using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    [HtmlTargetElement("abp-style", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AbpStyleTagHelper : AbpBundleItemTagHelper<AbpStyleTagHelper, AbpStyleTagHelperService>, IBundleItemTagHelper
    {
        public AbpStyleTagHelper(AbpStyleTagHelperService service)
            : base(service)
        {

        }

        protected override string GetFileExtension()
        {
            return "css";
        }
    }
}