using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Blockquote
{
    public class AbpBlockquoteParagraphTagHelperService : AbpTagHelperService<AbpBlockquoteParagraphTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("mb-0");
        }
        
    }
}