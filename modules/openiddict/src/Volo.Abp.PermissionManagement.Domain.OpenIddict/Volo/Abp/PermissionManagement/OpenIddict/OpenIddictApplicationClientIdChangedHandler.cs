using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.OpenIddict.Applications;

namespace Volo.Abp.PermissionManagement.OpenIddict;

public class OpenIddictApplicationClientIdChangedHandler :
    IDistributedEventHandler<OpenIddictApplicationClientIdChangedEto>,
    ITransientDependency
{
    protected IPermissionManager PermissionManager { get; }
    protected IPermissionGrantRepository PermissionGrantRepository { get; }
    protected IResourcePermissionManager ResourcePermissionManager { get; }
    protected IResourcePermissionGrantRepository ResourcePermissionGrantRepository { get; }

    public OpenIddictApplicationClientIdChangedHandler(
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

    public async Task HandleEventAsync(OpenIddictApplicationClientIdChangedEto eventData)
    {
        var permissionGrantsInRole = await PermissionGrantRepository.GetListAsync(ClientPermissionValueProvider.ProviderName, eventData.OldClientId);
        foreach (var permissionGrant in permissionGrantsInRole)
        {
            await PermissionManager.UpdateProviderKeyAsync(permissionGrant, eventData.ClientId);
        }

        var resourcePermissionGrantsInRole = await ResourcePermissionGrantRepository.GetListAsync(ClientResourcePermissionValueProvider.ProviderName, eventData.OldClientId);
        foreach (var resourcePermissionGrant in resourcePermissionGrantsInRole)
        {
            await ResourcePermissionManager.UpdateProviderKeyAsync(resourcePermissionGrant, eventData.ClientId);
        }
    }
}
