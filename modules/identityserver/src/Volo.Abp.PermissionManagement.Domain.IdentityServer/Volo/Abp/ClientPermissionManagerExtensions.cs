using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement;

public static class ClientPermissionManagerExtensions
{
    public static Task<PermissionWithGrantedProviders> GetForClientAsync([NotNull] this IPermissionManager permissionManager, string clientId, string permissionName)
    {
        Check.NotNull(permissionManager, nameof(permissionManager));

        return permissionManager.GetAsync(permissionName, ClientPermissionValueProvider.ProviderName, clientId);
    }

    public static Task<List<PermissionWithGrantedProviders>> GetAllForClientAsync([NotNull] this IPermissionManager permissionManager, string clientId)
    {
        Check.NotNull(permissionManager, nameof(permissionManager));

        return permissionManager.GetAllAsync(ClientPermissionValueProvider.ProviderName, clientId);
    }

    public static Task SetForClientAsync([NotNull] this IPermissionManager permissionManager, string clientId, [NotNull] string permissionName, bool isGranted)
    {
        Check.NotNull(permissionManager, nameof(permissionManager));

        return permissionManager.SetAsync(permissionName, ClientPermissionValueProvider.ProviderName, clientId, isGranted);
    }
}
