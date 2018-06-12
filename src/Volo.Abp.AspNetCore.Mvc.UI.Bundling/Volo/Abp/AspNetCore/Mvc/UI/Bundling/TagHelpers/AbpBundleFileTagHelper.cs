using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Bundling
{
    [HtmlTargetElement("abp-bundle-file", TagStructure = TagStructure.NormalOrSelfClosing, ParentTag = "abp-style-bundle")]
    [HtmlTargetElement("abp-bundle-file", TagStructure = TagStructure.NormalOrSelfClosing, ParentTag = "abp-script-bundle")]
    public class AbpBundleFileTagHelper : AbpTagHelper<AbpBundleFileTagHelper, AbpBundleFileTagHelperService>
    {
        public string Src { get; set; }

        public AbpBundleFileTagHelper(AbpBundleFileTagHelperService service) 
            : base(service)
        {

        }
    }
}