using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Collapse
{
    public class AbpCollapseButtonTagHelper : AbpTagHelper<AbpCollapseButtonTagHelper, AbpCollapseButtonTagHelperService>
    {
        public AbpButtonType ButonType { get; set; } = AbpButtonType.Default;

        public string BodyId { get; set; }

        public AbpCollapseButtonTagHelper(AbpCollapseButtonTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
