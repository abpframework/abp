using System.Threading.Tasks;

namespace Volo.Abp.Authorization.Permissions.Resources;

public interface IResourcePermissionStore
{
    /// <summary>
    /// Checks if the given permission is granted for the given resource.
    /// </summary>
    /// <param name="name">The name of the permission.</param>
    /// <param name="resourceName">The name of the resource.</param>
    /// <param name="resourceKey">Resource key</param>
    /// <param name="providerName">The name of the provider.</param>
    /// <param name="providerKey">The key of the provider.</param>
    /// <returns>
    /// True if the permission is granted.
    /// </returns>
    Task<bool> IsGrantedAsync(
        string name,
        string resourceName,
        string resourceKey,
        string providerName,
        string providerKey
    );

    /// <summary>
    /// Checks if the given permissions are granted for the given resource.
    /// </summary>
    /// <param name="names">The name of the permissions.</param>
    /// <param name="resourceName">The name of the resource.</param>
    /// <param name="resourceKey">Resource key</param>
    /// <param name="providerName">The name of the provider.</param>
    /// <param name="providerKey">The key of the provider.</param>
    /// <returns>
    /// A <see cref="MultiplePermissionGrantResult"/> object containing the grant results for each permission.
    /// </returns>
    Task<MultiplePermissionGrantResult> IsGrantedAsync(
        string[] names,
        string resourceName,
        string resourceKey,
        string providerName,
        string providerKey
    );

    /// <summary>
    /// Gets all permissions for the given resource.
    /// </summary>
    /// <param name="resourceName">Resource name</param>
    /// <param name="resourceKey">Resource key</param>
    /// <returns>
    /// A <see cref="MultiplePermissionGrantResult"/> object containing the grant results for each permission.
    /// </returns>
    Task<MultiplePermissionGrantResult> GetPermissionsAsync(
        string resourceName,
        string resourceKey
    );

    /// <summary>
    /// Gets all granted permissions for the given resource.
    /// </summary>
    /// <param name="resourceName">Resource name</param>
    /// <param name="resourceKey">Resource key</param>
    /// <returns>
    /// An array of granted permission names.
    /// </returns>
    Task<string[]> GetGrantedPermissionsAsync(
        string resourceName,
        string resourceKey
    );

    /// <summary>
    /// Retrieves the keys of resources for which the specified permission is granted.
    /// </summary>
    /// <param name="resourceName">The name of the resource.</param>
    /// <param name="name">The name of the permission.</param>
    /// <returns>
    /// An array of resource keys where the specified permission is granted.
    /// </returns>
    Task<string[]> GetGrantedResourceKeysAsync(
        string resourceName,
        string name
    );
}
