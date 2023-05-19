using System;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Modal;

[Flags]
public enum AbpModalButtons
{
    None = 0,
    Save = 1,
    Cancel = 2,
    Close = 4
}
