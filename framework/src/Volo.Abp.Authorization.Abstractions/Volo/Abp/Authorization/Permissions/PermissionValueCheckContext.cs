using System.Security.Claims;
using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Permissions;

public class PermissionValueCheckContext
{
    [NotNull]
    public PermissionDefinition Permission { get; }

    public ClaimsPrincipal? Principal { get; }

    public PermissionValueCheckContext(
        [NotNull] PermissionDefinition permission,
        ClaimsPrincipal? principal)
    {
        Check.NotNull(permission, nameof(permission));

        Permission = permission;
        Principal = principal;
    }
}
