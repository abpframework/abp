using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Authorization.Permissions.Resources;

namespace Volo.Abp.PermissionManagement;

public static class UserResourcePermissionManagerExtensions
{
    public static Task<List<PermissionWithGrantedProviders>> GetAllForUserAsync([NotNull] this IResourcePermissionManager resourcePermissionManager, Guid userId, [NotNull] string resourceName,  [NotNull] string resourceKey)
    {
        Check.NotNull(resourcePermissionManager, nameof(resourcePermissionManager));

        return resourcePermissionManager.GetAllAsync(resourceName, resourceKey, UserResourcePermissionValueProvider.ProviderName, userId.ToString());
    }

    public static Task SetForUserAsync([NotNull] this IResourcePermissionManager resourcePermissionManager, Guid userId, [NotNull] string name, [NotNull] string resourceName,  [NotNull] string resourceKey, bool isGranted)
    {
        Check.NotNull(resourcePermissionManager, nameof(resourcePermissionManager));

        return resourcePermissionManager.SetAsync(name, resourceName, resourceKey, UserResourcePermissionValueProvider.ProviderName, userId.ToString(), isGranted);
    }
}
