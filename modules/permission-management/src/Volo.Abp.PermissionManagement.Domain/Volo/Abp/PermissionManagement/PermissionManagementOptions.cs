using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Volo.Abp.PermissionManagement;

public class PermissionManagementOptions
{
    public ITypeList<IPermissionManagementProvider> ManagementProviders { get; }

    public Dictionary<string, string> ProviderPolicies { get; }
    
    public HashSet<string> DeletedPermissions { get; }
    
    public HashSet<string> DeletedPermissionGroups { get; }
    
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

        DeletedPermissions = new HashSet<string>();
        DeletedPermissionGroups = new HashSet<string>();
    }
}
