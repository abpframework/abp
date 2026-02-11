using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;

[Serializable]
public class ExtensionPropertyFeaturePolicyDto
{
    public string[] Features { get; set; } = default!;

    public bool RequiresAll { get; set; }
}
