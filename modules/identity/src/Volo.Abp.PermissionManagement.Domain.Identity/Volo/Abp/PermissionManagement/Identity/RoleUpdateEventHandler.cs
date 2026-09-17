using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;

namespace Volo.Abp.PermissionManagement.Identity;

public class RoleUpdateEventHandler :
    IDistributedEventHandler<IdentityRoleNameChangedEto>,
    ITransientDependency
{
    protected IPermissionManager PermissionManager { get; }
    protected IPermissionGrantRepository PermissionGrantRepository { get; }
    protected IResourcePermissionManager ResourcePermissionManager { get; }
    protected IResourcePermissionGrantRepository ResourcePermissionGrantRepository { get; }

    public RoleUpdateEventHandler(
        IPermissionManager permissionManager,
        IPermissionGrantRepository permissionGrantRepository,
        IResourcePermissionManager resourcePermissionManager,
        IResourcePermissionGrantRepository resourcePermissionGrantRepository)
    {
        PermissionManager = permissionManager;
        PermissionGrantRepository = permissionGrantRepository;
        ResourcePermissionManager = resourcePermissionManager;
        ResourcePermissionGrantRepository = resourcePermissionGrantRepository;
    }

    public async Task HandleEventAsync(IdentityRoleNameChangedEto eventData)
    {
        var permissionGrantsInRole = await PermissionGrantRepository.GetListAsync(RolePermissionValueProvider.ProviderName, eventData.OldName);
        foreach (var permissionGrant in permissionGrantsInRole)
        {
            await PermissionManager.UpdateProviderKeyAsync(permissionGrant, eventData.Name);
        }

        var resourcePermissionGrantsInRole = await ResourcePermissionGrantRepository.GetListAsync(RoleResourcePermissionValueProvider.ProviderName, eventData.OldName);
        foreach (var resourcePermissionGrant in resourcePermissionGrantsInRole)
        {
            await ResourcePermissionManager.UpdateProviderKeyAsync(resourcePermissionGrant, eventData.Name);
        }
    }
}
