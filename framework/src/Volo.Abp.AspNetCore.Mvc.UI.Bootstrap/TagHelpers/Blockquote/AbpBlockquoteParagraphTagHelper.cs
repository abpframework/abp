using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Blockquote;

[HtmlTargetElement("p", ParentTag = "blockquote")]
public class AbpBlockquoteParagraphTagHelper : AbpTagHelper<AbpBlockquoteParagraphTagHelper, AbpBlockquoteParagraphTagHelperService>
{
    public AbpBlockquoteParagraphTagHelper(AbpBlockquoteParagraphTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
