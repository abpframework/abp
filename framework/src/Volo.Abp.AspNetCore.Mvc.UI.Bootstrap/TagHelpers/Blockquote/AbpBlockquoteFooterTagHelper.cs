using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Blockquote
{
    [HtmlTargetElement("footer", ParentTag = "abp-blockquote")]
    public class AbpBlockquoteFooterTagHelper : AbpTagHelper<AbpBlockquoteFooterTagHelper, AbpBlockquoteFooterTagHelperService>
    {
        public AbpBlockquoteFooterTagHelper(AbpBlockquoteFooterTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
