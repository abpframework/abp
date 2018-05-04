using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Badge
{
    public class AbpBadgeTagHelperService : AbpTagHelperService<AbpBadgeTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var badgeType = TagHelper.BadgeType != AbpBadgeType._? TagHelper.BadgeType : TagHelper.BadgePillType;

            output.Attributes.AddClass("badge");

            if (TagHelper.BadgePillType != AbpBadgeType._)
            {
                output.Attributes.AddClass("badge-pill");
            }

            if (badgeType != AbpBadgeType.Default && badgeType != AbpBadgeType._)
            {
                output.Attributes.AddClass("badge-" + badgeType.ToString().ToLowerInvariant());
            }

        }
        
    }
}