using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.Authorization.Permissions.Resources;

public static class ResourcePermissionStoreExtensions
{
    /// <summary>
    /// Retrieves the list of granted permissions for a specific resource with a given key.
    /// </summary>
    /// <typeparam name="TResource">The type of the resource.</typeparam>
    /// <param name="resourcePermissionStore">The resource permission store instance.</param>
    /// <param name="resource">The resource instance to retrieve permissions for.</param>
    /// <param name="resourceKey">The unique key identifying the resource instance.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of strings representing the granted permissions.</returns>
    public static async Task<string[]> GetGrantedPermissionsAsync<TResource>(
        this IResourcePermissionStore resourcePermissionStore,
        TResource resource,
        object resourceKey
    )
    {
        return (await GetPermissionsAsync(resourcePermissionStore, resource, resourceKey)).Where(x => x.Value).Select(x => x.Key).ToArray();
    }

    /// <summary>
    /// Retrieves a dictionary of permissions and their granted status for the specified entity.
    /// </summary>
    /// <typeparam name="TResource">The type of the resource.</typeparam>
    /// <param name="resourcePermissionStore">The resource permission store instance.</param>
    /// <param name="resource">The resource for which the permissions are being retrieved.</param>
    /// <param name="resourceKey">The unique key identifying the resource instance.</param>
    /// <returns>A dictionary where the keys are permission names and the values are booleans indicating whether the permission is granted.</returns>
    public static async Task<IDictionary<string, bool>> GetPermissionsAsync<TResource>(
        this IResourcePermissionStore resourcePermissionStore,
        TResource resource,
        object resourceKey
    )
    {
        Check.NotNull(resourcePermissionStore, nameof(resourcePermissionStore));
        Check.NotNull(resource, nameof(resource));
        Check.NotNull(resourceKey, nameof(resourceKey));

        var result = await resourcePermissionStore.GetPermissionsAsync(
            typeof(TResource).FullName!,
            resourceKey.ToString()!
        );

        return result.Result.ToDictionary(x => x.Key, x => x.Value == PermissionGrantResult.Granted);
    }

    /// <summary>
    /// Retrieves the keys of the resources granted a specific permission.
    /// </summary>
    /// <typeparam name="TResource">The type of the resource.</typeparam>
    /// <param name="resourcePermissionStore">The resource permission store instance.</param>
    /// <param name="resource">The resource instance to check granted permissions for.</param>
    /// <param name="permissionName">The name of the permission to check.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of strings representing the granted resource keys.</returns>
    public static Task<string[]> GetGrantedResourceKeysAsync<TResource>(
        this IResourcePermissionStore resourcePermissionStore,
        TResource resource,
        string permissionName
    )
    {
        Check.NotNull(resourcePermissionStore, nameof(resourcePermissionStore));
        Check.NotNull(resource, nameof(resource));
        Check.NotNullOrWhiteSpace(permissionName, nameof(permissionName));

        return resourcePermissionStore.GetGrantedResourceKeysAsync(
            typeof(TResource).FullName!,
            permissionName
        );
    }
}
