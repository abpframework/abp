namespace Volo.Abp.ObjectExtending;

public class ExtensionPropertyGlobalFeaturePolicyConfiguration
{
    public string[] Features { get; set; } = [];

    public bool RequiresAll { get; set; } = default!;
}
