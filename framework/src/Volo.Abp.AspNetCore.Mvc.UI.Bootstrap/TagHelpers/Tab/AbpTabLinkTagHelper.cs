using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab
{
    [HtmlTargetElement("abp-tab-link", TagStructure = TagStructure.WithoutEndTag)]
    public class AbpTabLinkTagHelper : AbpTagHelper<AbpTabLinkTagHelper, AbpTabLinkTagHelperService>
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string ParentDropdownName { get; set; }

        public string Href { get; set; }

        public AbpTabLinkTagHelper(AbpTabLinkTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
