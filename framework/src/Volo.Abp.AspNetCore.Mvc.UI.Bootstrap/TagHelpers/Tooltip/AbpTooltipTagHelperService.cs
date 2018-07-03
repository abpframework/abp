using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Tooltip
{
    public class AbpTooltipTagHelperService : AbpTagHelperService<AbpTooltipTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            SetDataToggle(context, output);
            SetDataPlacement(context, output);
            SetTooltipTitle(context, output);
        }

        protected virtual void SetDataToggle(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("data-toggle","tooltip");
        }

        protected virtual void SetDataPlacement(TagHelperContext context, TagHelperOutput output)
        {
            var directory = GetDirectory() != TooltipDirectory.Default ? GetDirectory() : TooltipDirectory.Bottom;
            output.Attributes.Add("data-placement", directory.ToString().ToLowerInvariant());
        }

        protected virtual void SetTooltipTitle(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("title", GetTitle());
        }

        protected virtual string GetTitle()
        {
            switch (GetDirectory())
            {
                case TooltipDirectory.Top:
                    return TagHelper.AbpTooltipTop;
                case TooltipDirectory.Right:
                    return TagHelper.AbpTooltipRight;
                case TooltipDirectory.Bottom:
                    return TagHelper.AbpTooltipBottom;
                case TooltipDirectory.Left:
                    return TagHelper.AbpTooltipLeft;
                default:
                    return TagHelper.AbpTooltip;
            }
        }

        protected virtual TooltipDirectory GetDirectory()
        {
            if (!string.IsNullOrWhiteSpace(TagHelper.AbpTooltipTop))
            {
                return TooltipDirectory.Top;
            }
            if (!string.IsNullOrWhiteSpace(TagHelper.AbpTooltipBottom))
            {
                return TooltipDirectory.Bottom;
            }
            if (!string.IsNullOrWhiteSpace(TagHelper.AbpTooltipRight))
            {
                return TooltipDirectory.Right;
            }
            if (!string.IsNullOrWhiteSpace(TagHelper.AbpTooltipLeft))
            {
                return TooltipDirectory.Left;
            }

            return TooltipDirectory.Default;
        }
    }
}