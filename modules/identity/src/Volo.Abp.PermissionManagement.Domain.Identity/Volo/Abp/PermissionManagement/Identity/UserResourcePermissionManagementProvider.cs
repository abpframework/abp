using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement.Identity;

public class UserResourcePermissionManagementProvider : ResourcePermissionManagementProvider
{
    public override string Name => UserResourcePermissionValueProvider.ProviderName;

    public UserResourcePermissionManagementProvider(
        IResourcePermissionGrantRepository resourcePermissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
        : base(
            resourcePermissionGrantRepository,
            guidGenerator,
            currentTenant)
    {

    }
}
