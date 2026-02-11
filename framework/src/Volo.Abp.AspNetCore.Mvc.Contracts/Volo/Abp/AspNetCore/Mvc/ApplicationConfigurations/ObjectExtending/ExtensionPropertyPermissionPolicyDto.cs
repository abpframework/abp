using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;

[Serializable]
public class ExtensionPropertyPermissionPolicyDto
{
    public string[] PermissionNames { get; set; } = default!;

    public bool RequiresAll { get; set; }
}
