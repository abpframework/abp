using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Uow;

namespace Volo.Abp.PermissionManagement.IdentityServer;

public class ClientDeletedEventHandler :
    IDistributedEventHandler<EntityDeletedEto<ClientEto>>,
    ITransientDependency
{
    protected IPermissionManager PermissionManager { get; }
    protected IResourcePermissionManager ResourcePermissionManager { get; }

    public ClientDeletedEventHandler(IPermissionManager permissionManager, IResourcePermissionManager resourcePermissionManager)
    {
        PermissionManager = permissionManager;
        ResourcePermissionManager = resourcePermissionManager;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEto<ClientEto> eventData)
    {
        await PermissionManager.DeleteAsync(ClientPermissionValueProvider.ProviderName, eventData.Entity.ClientId);
        await ResourcePermissionManager.DeleteAsync(ClientResourcePermissionValueProvider.ProviderName, eventData.Entity.ClientId);
    }
}
