using System;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Grid;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

[AttributeUsage(AttributeTargets.Property)]
public class AbpCheckbox : Attribute
{
    public ColumnSize ColumnSize { get; set; } = ColumnSize.Auto;
    public bool SuppressLabel { get; set; }
}