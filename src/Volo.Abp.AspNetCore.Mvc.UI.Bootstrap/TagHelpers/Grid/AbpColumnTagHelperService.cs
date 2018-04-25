using System;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid
{
    public class AbpColumnTagHelperService : AbpTagHelperService<AbpColumnTagHelper>
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.AddClass("col");

            ProcessSizeClass(output, TagHelper.Size, "");
            ProcessSizeClass(output, TagHelper.SizeSm, "-sm");
            ProcessSizeClass(output, TagHelper.SizeMd, "-md");
            ProcessSizeClass(output, TagHelper.SizeLg, "-lg");
            ProcessSizeClass(output, TagHelper.SizeXl, "-xl");

            ProcessOffsetClass(output, TagHelper.Offset, "");
            ProcessOffsetClass(output, TagHelper.OffsetSm, "-sm");
            ProcessOffsetClass(output, TagHelper.OffsetMd, "-md");
            ProcessOffsetClass(output, TagHelper.OffsetLg, "-lg");
            ProcessOffsetClass(output, TagHelper.OffsetXl, "-xl");

            ProcessColumnOrder(output);
            ProcessVerticalAlign(output);
        }

        protected virtual void ProcessSizeClass(TagHelperOutput output, ColumnSize size, string breakpoint)
        {
            if (size == ColumnSize.Empty)
            {
                return;
            }

            var classString = "col" + breakpoint;

            if (size != ColumnSize.C)
            {
                classString += "-" + size.ToString("D");
            }

            output.Attributes.AddClass(classString);
        }

        protected virtual void ProcessOffsetClass(TagHelperOutput output, ColumnSize size, string breakpoint)
        {
            if (size == ColumnSize.Empty)
            {
                return;
            }

            var classString = "offset" + breakpoint;

            if (size == ColumnSize.C)
            {
                classString += "-0";
            }
            else
            {
                classString += "-" + size.ToString("D");
            }

            output.Attributes.AddClass(classString);
        }

        protected virtual void ProcessVerticalAlign(TagHelperOutput output)
        {
            if (TagHelper.VAlign == VerticalAlign.Default)
            {
                return;
            }

            output.Attributes.AddClass("align-self-" + TagHelper.VAlign.ToString().ToLowerInvariant());
        }

        protected virtual void ProcessColumnOrder(TagHelperOutput output)
        {
            if (TagHelper.ColumnOrder == ColumnOrder.Empty)
            {
                return;
            }

            var classString = "order-";

            if (TagHelper.ColumnOrder == ColumnOrder.First)
            {
                classString += "first";
            }
            else if (TagHelper.ColumnOrder == ColumnOrder.Last)
            {
                classString += "last";
            }
            else 
            {
                classString += TagHelper.ColumnOrder.ToString("D");
            }

            output.Attributes.AddClass(classString);
        }
    }
}