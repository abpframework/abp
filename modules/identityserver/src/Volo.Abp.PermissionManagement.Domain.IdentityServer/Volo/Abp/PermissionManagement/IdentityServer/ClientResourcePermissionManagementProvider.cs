using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement.IdentityServer;

public class ClientResourcePermissionManagementProvider : ResourcePermissionManagementProvider
{
    public override string Name => ClientResourcePermissionValueProvider.ProviderName;

    public ClientResourcePermissionManagementProvider(
        IResourcePermissionGrantRepository permissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
        : base(
            permissionGrantRepository,
            guidGenerator,
            currentTenant)
    {
    }

    public override Task<ResourcePermissionValueProviderGrantInfo> CheckAsync(string name, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        using (CurrentTenant.Change(null))
        {
            return base.CheckAsync(name, resourceName, resourceKey, providerName, providerKey);
        }
    }

    public override Task<MultipleResourcePermissionValueProviderGrantInfo> CheckAsync(string[] names, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        using (CurrentTenant.Change(null))
        {
            return base.CheckAsync(names, resourceName, resourceKey, providerName, providerKey);
        }
    }

    public override Task SetAsync(string name,  string resourceName, string resourceKey, string providerKey, bool isGranted)
    {
        using (CurrentTenant.Change(null))
        {
            return base.SetAsync(name, resourceName, resourceKey, providerKey, isGranted);
        }
    }

    protected override async Task GrantAsync(string name, string resourceName, string resourceKey, string providerKey)
    {
        using (CurrentTenant.Change(null))
        {
            await base.GrantAsync(name, resourceName, resourceKey, providerKey);
        }
    }

    protected override Task RevokeAsync(string name, string resourceName, string resourceKey, string providerKey)
    {
        using (CurrentTenant.Change(null))
        {
            return base.RevokeAsync(name, resourceName, resourceKey, providerKey);
        }
    }
}
