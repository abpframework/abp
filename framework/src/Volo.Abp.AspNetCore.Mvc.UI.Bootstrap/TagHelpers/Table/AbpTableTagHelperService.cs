using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Table
{
    public class AbpTableTagHelperService : AbpTagHelperService<AbpTableTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "table";
            output.Attributes.AddClass("table");
            output.TagMode = TagMode.StartTagAndEndTag;

            SetResponsiveness(context, output);
            SetTheme(context, output);
            SetHoverableRows(context, output);
            SetBorderStyle(context, output);
            SetSmall(context, output);
            SetStripedRows(context, output);
        }

        protected virtual void SetResponsiveness(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Responsive ?? false)
            {
                output.PreElement.SetHtmlContent("<div class=\"table-responsive\">");
            }
            else if (TagHelper.ResponsiveSm ?? false)
            {
                output.PreElement.SetHtmlContent("<div class=\"table-responsive-sm\">");
            }
            else if (TagHelper.ResponsiveMd ?? false)
            {
                output.PreElement.SetHtmlContent("<div class=\"table-responsive-md\">");
            }
            else if (TagHelper.ResponsiveLg ?? false)
            {
                output.PreElement.SetHtmlContent("<div class=\"table-responsive-lg\">");
            }
            else if (TagHelper.ResponsiveXl ?? false)
            {
                output.PreElement.SetHtmlContent("<div class=\"table-responsive-xl\">");
            }
            else
            {
                return;
            }

            output.PostElement.SetHtmlContent("</div>");
        }

        protected virtual void SetTheme(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.DarkTheme ?? false)
            {
                output.Attributes.AddClass("table-dark");
            }
        }

        protected virtual void SetStripedRows(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.StripedRows ?? false)
            {
                output.Attributes.AddClass("table-striped");
            }
        }

        protected virtual void SetHoverableRows(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.HoverableRows ?? false)
            {
                output.Attributes.AddClass("table-hover");
            }
        }

        protected virtual void SetSmall(TagHelperContext context, TagHelperOutput output)
        {
            if (TagHelper.Small ?? false)
            {
                output.Attributes.AddClass("table-sm");
            }
        }

        protected virtual void SetBorderStyle(TagHelperContext context, TagHelperOutput output)
        {
            switch (TagHelper.BorderStyle)
            {
                case AbpTableBorderStyle.Default:
                    return;
                case AbpTableBorderStyle.Bordered:
                    output.Attributes.AddClass("table-bordered");
                    return;
                case AbpTableBorderStyle.Borderless:
                    output.Attributes.AddClass("table-borderless");
                    return;
            }
        }
    }
}
