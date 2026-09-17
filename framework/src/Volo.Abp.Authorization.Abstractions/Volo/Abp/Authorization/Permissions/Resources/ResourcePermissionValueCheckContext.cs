using System.Security.Claims;

namespace Volo.Abp.Authorization.Permissions.Resources;

public class ResourcePermissionValueCheckContext : PermissionValueCheckContext
{
    public string ResourceName { get; }

    public string ResourceKey { get; }

    public ResourcePermissionValueCheckContext(PermissionDefinition permission, string resourceName, string resourceKey)
        : this(permission, null, resourceName, resourceKey)
    {
    }

    public ResourcePermissionValueCheckContext(PermissionDefinition permission, ClaimsPrincipal? principal, string resourceName, string resourceKey)
        : base(permission, principal)
    {
        ResourceName = resourceName;
        ResourceKey = resourceKey;
    }
}
