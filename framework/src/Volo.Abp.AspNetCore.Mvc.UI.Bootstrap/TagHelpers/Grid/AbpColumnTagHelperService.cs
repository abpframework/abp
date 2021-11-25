using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;

public class AbpColumnTagHelperService : AbpTagHelperService<AbpColumnTagHelper>
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";

        output.Attributes.AddClass("col");

        ProcessSizeClasses(context, output);
        ProcessOffsetClasses(context, output);
        ProcessColumnOrder(context, output);
        ProcessVerticalAlign(context, output);
    }

    protected virtual void ProcessSizeClasses(TagHelperContext context, TagHelperOutput output)
    {
        ProcessSizeClass(context, output, TagHelper.Size, "");
        ProcessSizeClass(context, output, TagHelper.SizeSm, "-sm");
        ProcessSizeClass(context, output, TagHelper.SizeMd, "-md");
        ProcessSizeClass(context, output, TagHelper.SizeLg, "-lg");
        ProcessSizeClass(context, output, TagHelper.SizeXl, "-xl");
    }

    protected virtual void ProcessOffsetClasses(TagHelperContext context, TagHelperOutput output)
    {
        ProcessOffsetClass(context, output, TagHelper.Offset, "");
        ProcessOffsetClass(context, output, TagHelper.OffsetSm, "-sm");
        ProcessOffsetClass(context, output, TagHelper.OffsetMd, "-md");
        ProcessOffsetClass(context, output, TagHelper.OffsetLg, "-lg");
        ProcessOffsetClass(context, output, TagHelper.OffsetXl, "-xl");
    }

    protected virtual void ProcessSizeClass(TagHelperContext context, TagHelperOutput output, ColumnSize size, string breakpoint)
    {
        if (size == ColumnSize.Undefined)
        {
            return;
        }

        var classString = "col" + breakpoint;

        if (size == ColumnSize.Auto)
        {
            classString += "-auto";
        }
        else if (size != ColumnSize._)
        {
            classString += "-" + size.ToString("D");
        }

        output.Attributes.RemoveClass("col");
        output.Attributes.AddClass(classString);
    }

    protected virtual void ProcessOffsetClass(TagHelperContext context, TagHelperOutput output, ColumnSize size, string breakpoint)
    {
        if (size == ColumnSize.Undefined)
        {
            return;
        }

        var classString = "offset" + breakpoint;

        if (size == ColumnSize._)
        {
            classString += "-0";
        }
        else
        {
            classString += "-" + size.ToString("D");
        }

        output.Attributes.AddClass(classString);
    }

    protected virtual void ProcessVerticalAlign(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.VAlign == VerticalAlign.Default)
        {
            return;
        }

        output.Attributes.AddClass("align-self-" + TagHelper.VAlign.ToString().ToLowerInvariant());
    }

    protected virtual void ProcessColumnOrder(TagHelperContext context, TagHelperOutput output)
    {
        if (TagHelper.ColumnOrder == ColumnOrder.Undefined)
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
