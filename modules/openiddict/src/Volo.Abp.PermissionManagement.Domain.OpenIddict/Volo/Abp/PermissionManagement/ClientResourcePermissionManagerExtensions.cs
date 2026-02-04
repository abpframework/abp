using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement;

public static class ClientResourcePermissionManagerExtensions
{
    public static Task<PermissionWithGrantedProviders> GetForClientAsync([NotNull] this IResourcePermissionManager resourcePermissionManager, string resourceName, string resourceKey, string clientId, string permissionName)
    {
        Check.NotNull(resourcePermissionManager, nameof(resourcePermissionManager));

        return resourcePermissionManager.GetAsync(permissionName, resourceName, resourceKey, ClientPermissionValueProvider.ProviderName, clientId);
    }

    public static Task<List<PermissionWithGrantedProviders>> GetAllForClientAsync([NotNull] this IResourcePermissionManager resourcePermissionManager, string resourceName, string resourceKey, string clientId)
    {
        Check.NotNull(resourcePermissionManager, nameof(resourcePermissionManager));

        return resourcePermissionManager.GetAllAsync(resourceName, resourceKey, ClientPermissionValueProvider.ProviderName, clientId);
    }

    public static Task SetForClientAsync([NotNull] this IResourcePermissionManager resourcePermissionManager, string resourceName, string resourceKey, string clientId, [NotNull] string permissionName, bool isGranted)
    {
        Check.NotNull(resourcePermissionManager, nameof(resourcePermissionManager));

        return resourcePermissionManager.SetAsync(permissionName, resourceName, resourceKey, ClientPermissionValueProvider.ProviderName, clientId, isGranted);
    }
}
