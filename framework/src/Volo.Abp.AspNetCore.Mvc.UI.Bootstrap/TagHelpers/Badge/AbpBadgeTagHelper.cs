using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Badge
{

    [HtmlTargetElement("a", Attributes = "abp-badge")]
    [HtmlTargetElement("span", Attributes = "abp-badge")]
    [HtmlTargetElement("a", Attributes = "abp-badge-pill")]
    [HtmlTargetElement("span", Attributes = "abp-badge-pill")]
    public class AbpBadgeTagHelper : AbpTagHelper<AbpBadgeTagHelper, AbpBadgeTagHelperService>
    {
        [HtmlAttributeName("abp-badge")]
        public AbpBadgeType BadgeType { get; set; } = AbpBadgeType._;

        [HtmlAttributeName("abp-badge-pill")]
        public AbpBadgeType BadgePillType { get; set; } = AbpBadgeType._;

        public AbpBadgeTagHelper(AbpBadgeTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
