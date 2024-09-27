namespace Volo.Abp.ObjectExtending;

public class ExtensionPropertyFeaturePolicyConfiguration
{
    public string[] Features { get; set; } = [];

    public bool RequiresAll { get; set; } = default!;
}
