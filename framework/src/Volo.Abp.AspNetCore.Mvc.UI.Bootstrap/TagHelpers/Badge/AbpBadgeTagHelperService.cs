using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Badge
{
    public class AbpBadgeTagHelperService : AbpTagHelperService<AbpBadgeTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetBadgeClass(context, output);
            SetBadgeStyle(context, output);
        }

        protected virtual void SetBadgeStyle(TagHelperContext context, TagHelperOutput output)
        {
            var badgeType = GetBadgeType(context, output);

            if (badgeType != AbpBadgeType.Default && badgeType != AbpBadgeType._)
            {
                output.Attributes.AddClass("badge-" + badgeType.ToString().ToLowerInvariant());
            }
        }

        protected virtual void SetBadgeClass(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.AddClass("badge");

            if (TagHelper.BadgePillType != AbpBadgeType._)
            {
                output.Attributes.AddClass("rounded-pill");
            }
        }

        protected virtual AbpBadgeType GetBadgeType(TagHelperContext context, TagHelperOutput output)
        {
            return TagHelper.BadgeType != AbpBadgeType._ ? TagHelper.BadgeType : TagHelper.BadgePillType;
        }
    }
}
