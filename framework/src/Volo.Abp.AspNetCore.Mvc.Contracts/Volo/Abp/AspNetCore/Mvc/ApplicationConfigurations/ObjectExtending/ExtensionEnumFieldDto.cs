using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;

[Serializable]
public class ExtensionEnumFieldDto
{
    public string Name { get; set; }

    public object Value { get; set; }
}
