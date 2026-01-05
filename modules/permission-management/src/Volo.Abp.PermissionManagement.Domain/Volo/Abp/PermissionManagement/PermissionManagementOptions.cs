using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Volo.Abp.PermissionManagement;

public class PermissionManagementOptions
{
    public ITypeList<IPermissionManagementProvider> ManagementProviders { get; }

    public Dictionary<string, string> ProviderPolicies { get; }

    public ITypeList<IResourcePermissionManagementProvider> ResourceManagementProviders { get; }

    public ITypeList<IResourcePermissionProviderKeyLookupService> ResourcePermissionProviderKeyLookupServices { get; }

    /// <summary>
    /// Default: true.
    /// </summary>
    public bool SaveStaticPermissionsToDatabase { get; set; } = true;

    /// <summary>
    /// Default: false.
    /// </summary>
    public bool IsDynamicPermissionStoreEnabled { get; set; }

    public PermissionManagementOptions()
    {
        ManagementProviders = new TypeList<IPermissionManagementProvider>();
        ProviderPolicies = new Dictionary<string, string>();

        ResourceManagementProviders = new TypeList<IResourcePermissionManagementProvider>();
        ResourcePermissionProviderKeyLookupServices = new TypeList<IResourcePermissionProviderKeyLookupService>();
    }
}
