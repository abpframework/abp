using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

[HtmlTargetElement("abp-radio")]
public class AbpRadioInputTagHelper : AbpTagHelper<AbpRadioInputTagHelper, AbpRadioInputTagHelperService>
{
    public ModelExpression AspFor { get; set; } = default!;

    public string? Label { get; set; }

    public bool SuppressLabel { get; set; }

    [HtmlAttributeName("info")]
    public string? InfoText { get; set; }

    public bool? Inline { get; set; }

    public bool? Disabled { get; set; }

    public bool ButtonStyle { get; set; } = false;

    public bool UseButtonGroup { get; set; } = false;

    public AbpButtonType ButtonType { get; set; } = AbpButtonType.Outline_Primary;

    public AbpButtonSize ButtonSize { get; set; } = AbpButtonSize.Default;

    public IEnumerable<SelectListItem>? AspItems { get; set; }

    public AbpRadioInputTagHelper(AbpRadioInputTagHelperService tagHelperService)
        : base(tagHelperService)
    {

    }
}
