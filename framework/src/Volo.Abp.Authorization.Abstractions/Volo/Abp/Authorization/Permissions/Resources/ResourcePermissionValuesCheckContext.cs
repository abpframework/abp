using System.Collections.Generic;
using System.Security.Claims;

namespace Volo.Abp.Authorization.Permissions.Resources;

public class ResourcePermissionValuesCheckContext : PermissionValuesCheckContext
{
    public string ResourceName { get; }

    public string ResourceKey { get; }

    public ResourcePermissionValuesCheckContext(PermissionDefinition permission,string resourceName, string resourceKey)
        : this([permission], null, resourceName, resourceKey)
    {

    }


    public ResourcePermissionValuesCheckContext(PermissionDefinition permission, ClaimsPrincipal? principal, string resourceName, string resourceKey)
        : this([permission], principal, resourceName, resourceKey)
    {

    }

    public ResourcePermissionValuesCheckContext(List<PermissionDefinition> permissions, string resourceName, string resourceKey)
        : this(permissions, null, resourceName, resourceKey)
    {
        ResourceName = resourceName;
        ResourceKey = resourceKey;
    }

    public ResourcePermissionValuesCheckContext(List<PermissionDefinition> permissions, ClaimsPrincipal? principal, string resourceName, string resourceKey)
        : base(permissions, principal)
    {
        ResourceName = resourceName;
        ResourceKey = resourceKey;
    }
}
