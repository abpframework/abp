using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.PermissionManagement;

public abstract class ResourcePermissionManagementProvider : IResourcePermissionManagementProvider
{
    public abstract string Name { get; }

    protected IResourcePermissionGrantRepository ResourcePermissionGrantRepository { get; }

    protected IGuidGenerator GuidGenerator { get; }

    protected ICurrentTenant CurrentTenant { get; }

    protected ResourcePermissionManagementProvider(
        IResourcePermissionGrantRepository resourcePermissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
    {
        ResourcePermissionGrantRepository = resourcePermissionGrantRepository;
        GuidGenerator = guidGenerator;
        CurrentTenant = currentTenant;
    }

    public virtual Task<bool> IsAvailableAsync()
    {
        return Task.FromResult(true);
    }

    public virtual async Task<ResourcePermissionValueProviderGrantInfo> CheckAsync(string name, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        var multiplePermissionValueProviderGrantInfo = await CheckAsync(new[] { name }, resourceName, resourceKey, providerName, providerKey);

        return multiplePermissionValueProviderGrantInfo.Result.First().Value;
    }

    public virtual async Task<MultipleResourcePermissionValueProviderGrantInfo> CheckAsync(string[] names, string resourceName, string resourceKey, string providerName, string providerKey)
    {
        using (ResourcePermissionGrantRepository.DisableTracking())
        {
            var multiplePermissionValueProviderGrantInfo = new MultipleResourcePermissionValueProviderGrantInfo(names);
            if (providerName != Name)
            {
                return multiplePermissionValueProviderGrantInfo;
            }

            var resourcePermissionGrants = await ResourcePermissionGrantRepository.GetListAsync(names, resourceName, resourceKey, providerName, providerKey);

            foreach (var permissionName in names)
            {
                var isGrant = resourcePermissionGrants.Any(x => x.Name == permissionName);
                multiplePermissionValueProviderGrantInfo.Result[permissionName] = new ResourcePermissionValueProviderGrantInfo(isGrant, providerKey);
            }

            return multiplePermissionValueProviderGrantInfo;
        }
    }

    public virtual Task SetAsync(string name,  string resourceName, string resourceKey, string providerKey, bool isGranted)
    {
        return isGranted
            ? GrantAsync(name, resourceName, resourceKey, providerKey)
            : RevokeAsync(name, resourceName, resourceKey, providerKey);
    }

    protected virtual async Task GrantAsync(string name, string resourceName, string resourceKey, string providerKey)
    {
        var resourcePermissionGrants = await ResourcePermissionGrantRepository.FindAsync(name, resourceName, resourceKey, Name, providerKey);
        if (resourcePermissionGrants != null)
        {
            return;
        }

        resourcePermissionGrants = new ResourcePermissionGrant(GuidGenerator.Create(), name, resourceName, resourceKey, Name, providerKey, CurrentTenant.Id);
        await ResourcePermissionGrantRepository.InsertAsync(resourcePermissionGrants, true);
    }

    protected virtual async Task RevokeAsync(string name, string resourceName,string resourceKey, string providerKey)
    {
        var resourcePermissionGrants = await ResourcePermissionGrantRepository.FindAsync(name, resourceName, resourceKey, Name, providerKey);
        if (resourcePermissionGrants == null)
        {
            return;
        }

        await ResourcePermissionGrantRepository.DeleteAsync(resourcePermissionGrants, true);
    }
}
