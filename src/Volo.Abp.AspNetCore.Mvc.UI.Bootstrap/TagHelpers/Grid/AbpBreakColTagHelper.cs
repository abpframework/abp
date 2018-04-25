using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid
{
    public class AbpBreakColTagHelper : AbpTagHelper<AbpBreakColTagHelper, AbpBreakColTagHelperService>
    {
        public AbpBreakColTagHelper(AbpBreakColTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
