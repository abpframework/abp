using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Bundling
{
    [HtmlTargetElement("abp-script-bundle", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AbpScriptBundleTagHelper : AbpTagHelper<AbpScriptBundleTagHelper, AbpScriptBundleTagHelperService>
    {
        public string Name { get; set; }

        public AbpScriptBundleTagHelper(AbpScriptBundleTagHelperService service)
            : base(service)
        {
        }
    }
}