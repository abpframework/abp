using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Volo.Abp.Authorization.Permissions;

public class AbpPermissionOptions
{
    public ITypeList<IPermissionDefinitionProvider> DefinitionProviders { get; }

    public ITypeList<IPermissionValueProvider> ValueProviders { get; }

    public HashSet<string> DeletedPermissions { get; }

    public HashSet<string> DeletedPermissionGroups { get; }

    public AbpPermissionOptions()
    {
        DefinitionProviders = new TypeList<IPermissionDefinitionProvider>();
        ValueProviders = new TypeList<IPermissionValueProvider>();

        DeletedPermissions = new HashSet<string>();
        DeletedPermissionGroups = new HashSet<string>();
    }
}
