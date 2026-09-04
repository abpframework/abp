using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

[HtmlTargetElement("abp-text", TagStructure = TagStructure.NormalOrSelfClosing)]
public class AbpTextTagHelper : AbpTagHelper<AbpTextTagHelper, AbpTextTagHelperService>
{
    [HtmlAttributeName("asp-for")]
    public ModelExpression AspFor { get; set; } = default!;

    [HtmlAttributeName("label")]
    public string? Label { get; set; }

    [HtmlAttributeName("label-width")]
    public ColumnSize? LabelWidth { get; set; }

    [HtmlAttributeName("suppress-label")]
    public bool SuppressLabel { get; set; } = false;

    [HtmlAttributeName("format")]
    public string? Format { get; set; }

    public AbpTextTagHelper(AbpTextTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}