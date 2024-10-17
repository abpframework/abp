using System;
using System.Collections.Generic;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;

[Serializable]
public class ExtensionPropertyDto
{
    public string Type { get; set; } = default!;

    public string TypeSimple { get; set; } = default!;

    public LocalizableStringDto? DisplayName { get; set; }

    public ExtensionPropertyApiDto Api { get; set; }

    public ExtensionPropertyUiDto Ui { get; set; }

    public ExtensionPropertyPolicyDto Policy { get; set; }

    public List<ExtensionPropertyAttributeDto> Attributes { get; set; }

    public Dictionary<string, object> Configuration { get; set; }

    public object? DefaultValue { get; set; }

    public ExtensionPropertyDto()
    {
        Api = new ExtensionPropertyApiDto();
        Ui = new ExtensionPropertyUiDto();
        Policy = new ExtensionPropertyPolicyDto();
        Attributes = new List<ExtensionPropertyAttributeDto>();
        Configuration = new Dictionary<string, object>();
    }
}
