using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement;

public interface IResourcePermissionManager
{
    Task<List<IResourcePermissionProviderKeyLookupService>> GetProviderKeyLookupServicesAsync();

    Task<IResourcePermissionProviderKeyLookupService> GetProviderKeyLookupServiceAsync(string providerName);

    Task<List<PermissionDefinition>> GetAvailablePermissionsAsync(string resourceName);

    Task<PermissionWithGrantedProviders> GetAsync(
        string permissionName,
        string resourceName,
        string resourceKey,
        string providerName,
        string providerKey
    );

    Task<MultiplePermissionWithGrantedProviders> GetAsync(
        string[] permissionNames,
        string resourceName,
        string resourceKey,
        string providerName,
        string providerKey
    );

    Task<List<PermissionWithGrantedProviders>> GetAllAsync(
        string resourceName,
        string resourceKey
    );

    Task<List<PermissionWithGrantedProviders>> GetAllAsync(
        string resourceName,
        string resourceKey,
        string providerName,
        string providerKey
    );

    Task<List<PermissionProviderWithPermissions>> GetAllGroupAsync(
        string resourceName,
        string resourceKey
    );

    Task SetAsync(
        string permissionName,
        string resourceName,
        string resourceKey,
        string providerName,
        string providerKey,
        bool isGranted
    );

    Task<ResourcePermissionGrant> UpdateProviderKeyAsync(
        ResourcePermissionGrant resourcePermissionGrant,
        string providerKey
    );

    Task DeleteAsync(
        string resourceName,
        string resourceKey,
        string providerName,
        string providerKey
    );

    Task DeleteAsync(
        string name,
        string resourceName,
        string resourceKey,
        string providerName,
        string providerKey
    );

    Task DeleteAsync(
        string providerName,
        string providerKey
    );
}
