using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Blockquote
{
    public class AbpBlockquoteTagHelper : AbpTagHelper<AbpBlockquoteTagHelper, AbpBlockquoteTagHelperService>
    {
        public AbpBlockquoteTagHelper(AbpBlockquoteTagHelperService tagHelperService)
            : base(tagHelperService)
        {
            
        }
    }
}
