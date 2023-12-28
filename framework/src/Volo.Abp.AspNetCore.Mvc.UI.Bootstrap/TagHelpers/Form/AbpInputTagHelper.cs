using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

public class AbpInputTagHelper : AbpTagHelper<AbpInputTagHelper, AbpInputTagHelperService>
{
    public ModelExpression AspFor { get; set; } = default!;

    public string? Label { get; set; }

    public string? LabelTooltip { get; set; }

    public string LabelTooltipIcon { get; set; } = "bi-info-circle";

    public string LabelTooltipPlacement  { get; set; } = "right";

    public bool LabelTooltipHtml  { get; set; } = false;

    [HtmlAttributeName("info")]
    public string? InfoText { get; set; }

    [HtmlAttributeName("disabled")]
    public bool IsDisabled { get; set; } = false;

    [HtmlAttributeName("readonly")]
    public bool? IsReadonly { get; set; } = false;

    public bool AutoFocus { get; set; }

    [HtmlAttributeName("type")]
    public string? InputTypeName { get; set; }

    public AbpFormControlSize Size { get; set; } = AbpFormControlSize.Default;

    [HtmlAttributeName("required-symbol")]
    public bool DisplayRequiredSymbol { get; set; } = true;

    [HtmlAttributeName("asp-format")]
    public string? Format { get; set; }

    public string? Name { get; set; }

    public string? Value { get; set; }

    public bool SuppressLabel { get; set; }

    [HtmlAttributeName("floating-label")]

    public bool FloatingLabel { get; set; }

    public CheckBoxHiddenInputRenderMode? CheckBoxHiddenInputRenderMode { get; set; }

    public bool AddMarginBottomClass  { get; set; } = true;

    public AbpInputTagHelper(AbpInputTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
