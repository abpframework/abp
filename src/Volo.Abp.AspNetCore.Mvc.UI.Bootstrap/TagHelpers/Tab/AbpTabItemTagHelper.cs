using System.Dynamic;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab
{
    [HtmlTargetElement("abp-tab-item")]
    public class AbpTabItemTagHelper : AbpTagHelper<AbpTabItemTagHelper, AbpTabItemTagHelperService>
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public bool? Active { get; set; }

        public AbpTabItemTagHelper(AbpTabItemTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
