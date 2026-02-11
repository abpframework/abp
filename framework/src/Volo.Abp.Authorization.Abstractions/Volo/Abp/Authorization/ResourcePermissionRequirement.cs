using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization;

public class ResourcePermissionRequirement : IAuthorizationRequirement
{
    public string PermissionName { get; }

    public ResourcePermissionRequirement([NotNull] string permissionName)
    {
        Check.NotNull(permissionName, nameof(permissionName));

        PermissionName = permissionName;
    }

    public override string ToString()
    {
        return $"ResourcePermissionRequirement: {PermissionName}";
    }
}
