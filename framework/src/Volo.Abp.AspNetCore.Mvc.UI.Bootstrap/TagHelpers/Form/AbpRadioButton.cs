using System;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Button;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class AbpRadioButton : Attribute
{
    public bool Inline { get; set; } = false;

    public bool Disabled { get; set; } = false;

    public bool SuppressLabel { get; set; }

    public bool ButtonStyle { get; set; } = false;

    public bool UseButtonGroup { get; set; } = false;

    public AbpButtonType ButtonType { get; set; } = AbpButtonType.Default;

    public AbpButtonSize ButtonSize { get; set; } = AbpButtonSize.Default;
}
