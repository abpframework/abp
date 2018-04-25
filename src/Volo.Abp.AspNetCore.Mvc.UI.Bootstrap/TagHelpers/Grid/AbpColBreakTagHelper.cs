using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid
{
    public class AbpColBreakTagHelper : AbpTagHelper<AbpColBreakTagHelper, AbpColBreakTagHelperService>
    {
        public AbpColBreakTagHelper(AbpColBreakTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
