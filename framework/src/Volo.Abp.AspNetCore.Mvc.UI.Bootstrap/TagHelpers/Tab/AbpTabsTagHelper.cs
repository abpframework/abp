
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tab
{
    public class AbpTabsTagHelper : AbpTagHelper<AbpTabsTagHelper, AbpTabsTagHelperService>
    {
        public string Name { get; set; }

        public TabStyle TabStyle { get; set; } = TabStyle.Tab;

        public ColumnSize VerticalHeaderSize { get; set; } = ColumnSize._3;

        public AbpTabsTagHelper(AbpTabsTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
