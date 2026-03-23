using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.Uow;

namespace Volo.Abp.PermissionManagement.OpenIddict;

public class OpenIddictApplicationDeletedEventHandler :
    IDistributedEventHandler<EntityDeletedEto<OpenIddictApplicationEto>>,
    ITransientDependency
{
    protected IPermissionManager PermissionManager { get; }
    protected IResourcePermissionManager ResourcePermissionManager { get; }

    public OpenIddictApplicationDeletedEventHandler(IPermissionManager permissionManager, IResourcePermissionManager resourcePermissionManager)
    {
        PermissionManager = permissionManager;
        ResourcePermissionManager = resourcePermissionManager;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEto<OpenIddictApplicationEto> eventData)
    {
        await PermissionManager.DeleteAsync(ClientPermissionValueProvider.ProviderName, eventData.Entity.ClientId);
        await ResourcePermissionManager.DeleteAsync(ClientResourcePermissionValueProvider.ProviderName, eventData.Entity.ClientId);
    }
}
