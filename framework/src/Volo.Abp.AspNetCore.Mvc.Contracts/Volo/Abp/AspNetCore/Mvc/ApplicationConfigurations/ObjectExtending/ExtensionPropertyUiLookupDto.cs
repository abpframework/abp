using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;

[Serializable]
public class ExtensionPropertyUiLookupDto
{
    public string Url { get; set; } = default!;
    public string ResultListPropertyName { get; set; } = default!;
    public string DisplayPropertyName { get; set; } = default!;
    public string ValuePropertyName { get; set; } = default!;
    public string FilterParamName { get; set; } = default!;
}
