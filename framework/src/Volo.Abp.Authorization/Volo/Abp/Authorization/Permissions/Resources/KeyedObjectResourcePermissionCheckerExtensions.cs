using System.Threading.Tasks;

namespace Volo.Abp.Authorization.Permissions.Resources;

public static class KeyedObjectResourcePermissionCheckerExtensions
{
    /// <summary>
    /// Checks if the specified permission is granted for the given resource.
    /// </summary>
    /// <typeparam name="TResource">The type of the object.</typeparam>
    /// <param name="resourcePermissionChecker">The resource permission checker instance.</param>
    /// <param name="permissionName">The name of the permission to check.</param>
    /// <param name="resource">The resource for which the permission is being checked.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the permission is granted.</returns>
    public static Task<bool> IsGrantedAsync<TResource>(this IResourcePermissionChecker resourcePermissionChecker, string permissionName, TResource resource)
        where TResource : class, IKeyedObject
    {
        Check.NotNull(resourcePermissionChecker, nameof(resourcePermissionChecker));
        Check.NotNullOrWhiteSpace(permissionName, nameof(permissionName));
        Check.NotNull(resource, nameof(resource));

        return resourcePermissionChecker.IsGrantedAsync(
            permissionName,
            resource,
            resource.GetObjectKey() ?? throw new AbpException("The resource doesn't have a key.")
        );
    }
}
