namespace Volo.Abp.ObjectExtending;

public class ExtensionPropertyPermissionPolicyConfiguration
{
    public string[] PermissionNames { get; set; } = [];

    public bool RequiresAll { get; set; } = default!;
}
