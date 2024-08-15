namespace Volo.Abp.ObjectExtending;

public class ExtensionPropertyPolicyConfiguration
{
    public ExtensionPropertyGlobalFeaturePolicyConfiguration GlobalFeatures { get; set; }

    public ExtensionPropertyFeaturePolicyConfiguration Features { get; set; }

    public ExtensionPropertyPermissionPolicyConfiguration Permissions { get; set; }

    public ExtensionPropertyPolicyConfiguration()
    {
        GlobalFeatures = new ExtensionPropertyGlobalFeaturePolicyConfiguration();
        Features = new ExtensionPropertyFeaturePolicyConfiguration();
        Permissions = new ExtensionPropertyPermissionPolicyConfiguration();
    }
}
