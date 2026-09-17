using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.Authorization.Permissions.Resources;

public static class KeyedObjectResourcePermissionStoreExtensions
{
    /// <summary>
    /// Retrieves an array of granted permissions for a specific entity.
    /// </summary>
    /// <typeparam name="TResource">The type of the resource.</typeparam>
    /// <param name="resourcePermissionStore">The resource permission store instance.</param>
    /// <param name="resource">The resource for which the permissions are being checked.</param>
    /// <returns>An array of granted permission names as strings.</returns>
    public static async Task<string[]> GetGrantedPermissionsAsync<TResource>(
        this IResourcePermissionStore resourcePermissionStore,
        TResource resource
    )
        where TResource : class, IKeyedObject
    {
        Check.NotNull(resourcePermissionStore, nameof(resourcePermissionStore));
        Check.NotNull(resource, nameof(resource));

        return (await GetPermissionsAsync(resourcePermissionStore, resource)).Where(x => x.Value).Select(x => x.Key).ToArray();
    }

    /// <summary>
    /// Retrieves a dictionary of permissions and their granted status for the specified entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="resourcePermissionStore">The resource permission store instance.</param>
    /// <param name="entity">The entity for which the permissions are being retrieved.</param>
    /// <returns>A dictionary where the keys are permission names and the values are booleans indicating whether the permission is granted.</returns>
    public static async Task<IDictionary<string, bool>> GetPermissionsAsync<TEntity>(
        this IResourcePermissionStore resourcePermissionStore,
        TEntity entity
    )
        where TEntity : class, IKeyedObject
    {
        Check.NotNull(resourcePermissionStore, nameof(resourcePermissionStore));
        Check.NotNull(entity, nameof(entity));

        return await resourcePermissionStore.GetPermissionsAsync(
            entity,
            entity.GetObjectKey() ?? throw new AbpException("The entity doesn't have a key.")
        );
    }
}
