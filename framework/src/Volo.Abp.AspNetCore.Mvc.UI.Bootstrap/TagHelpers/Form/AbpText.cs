using System;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class AbpText : Attribute
{
    public ColumnSize? LabelWidth { get; set; }

    public bool SuppressLabel { get; set; } = false;

    public string? Format { get; set; }

    public AbpText()
    {
    }
}
