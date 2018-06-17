using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bundling.TagHelpers
{
    [HtmlTargetElement("abp-script-bundle", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class AbpScriptBundleTagHelper : AbpTagHelper<AbpScriptBundleTagHelper, AbpScriptBundleTagHelperService>, IBundleTagHelper
    {
        public string Name { get; set; }

        public AbpScriptBundleTagHelper(AbpScriptBundleTagHelperService service)
            : base(service)
        {
        }

        public string GetNameOrNull()
        {
            return Name;
        }
    }
}