using System.Security.Claims;
using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Permissions;

public class PermissionValueCheckContext
{
    [NotNull]
    public PermissionDefinition Permission { get; }

    [CanBeNull]
    public ClaimsPrincipal Principal { get; }

    public PermissionValueCheckContext(
        [NotNull] PermissionDefinition permission,
        [CanBeNull] ClaimsPrincipal principal)
    {
        Check.NotNull(permission, nameof(permission));

        Permission = permission;
        Principal = principal;
    }
}
