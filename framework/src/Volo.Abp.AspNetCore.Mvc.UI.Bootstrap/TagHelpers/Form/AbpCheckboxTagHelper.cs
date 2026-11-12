using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

[HtmlTargetElement("abp-checkbox", TagStructure = TagStructure.NormalOrSelfClosing)]
public class AbpCheckboxTagHelper : AbpTagHelper<AbpCheckboxTagHelper, AbpCheckboxTagHelperService>
{
    public bool SuppressLabel { get; set; }

    [HtmlAttributeName("label")]
    public string? Label { get; set; }

    [HtmlAttributeName("asp-for")]
    public ModelExpression AspFor { get; set; } = default!;

    [HtmlAttributeName("asp-items")]
    public IEnumerable<SelectListItem>? AspItems { get; set; }

    [HtmlAttributeName("disabled")]
    public bool Disabled { get; set; }

    [HtmlAttributeName("readonly")]
    public bool ReadOnly { get; set; }

    [HtmlAttributeName("column-size")]
    public ColumnSize ColumnSize { get; set; } = ColumnSize.Auto;

    [HtmlAttributeName("class")]
    public string? CssClass { get; set; }

    [HtmlAttributeName("label-class")]
    public string? LabelClass { get; set; }

    [HtmlAttributeName("info")]
    public string? InfoText { get; set; }

    public AbpCheckboxTagHelper(AbpCheckboxTagHelperService service)
        : base(service)
    {
    }
}