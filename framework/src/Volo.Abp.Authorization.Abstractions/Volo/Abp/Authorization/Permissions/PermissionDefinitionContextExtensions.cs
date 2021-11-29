using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Permissions;

public static class PermissionDefinitionContextExtensions
{
    /// <summary>
    /// Finds and disables a permission with the given <paramref name="name"/>.
    /// Returns false if given permission was not found.
    /// </summary>
    /// <param name="context">Permission definition context</param>
    /// <param name="name">Name of the permission</param>
    /// <returns>
    /// Returns true if given permission was found.
    /// Returns false if given permission was not found.
    /// </returns>
    public static bool TryDisablePermission(
        [NotNull] this IPermissionDefinitionContext context,
        [NotNull] string name)
    {
        Check.NotNull(context, nameof(context));
        Check.NotNull(name, nameof(name));

        var permission = context.GetPermissionOrNull(name);
        if (permission == null)
        {
            return false;
        }

        permission.IsEnabled = false;
        return true;
    }
}
