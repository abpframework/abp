using System;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;

[Serializable]
public class ExtensionPropertyPolicyDto
{
    [NotNull]
    public ExtensionPropertyGlobalFeaturePolicyDto GlobalFeatures { get; set; }

    [NotNull]
    public ExtensionPropertyFeaturePolicyDto Features { get; set; }

    [NotNull]
    public ExtensionPropertyPermissionPolicyDto Permissions { get; set; }

    public ExtensionPropertyPolicyDto()
    {
        GlobalFeatures = new ExtensionPropertyGlobalFeaturePolicyDto();
        Features = new ExtensionPropertyFeaturePolicyDto();
        Permissions = new ExtensionPropertyPermissionPolicyDto();
    }
}
