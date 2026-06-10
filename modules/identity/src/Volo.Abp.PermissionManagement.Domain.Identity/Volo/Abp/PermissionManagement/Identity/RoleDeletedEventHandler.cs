using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace Volo.Abp.PermissionManagement.Identity;

public class RoleDeletedEventHandler :
    IDistributedEventHandler<EntityDeletedEto<IdentityRoleEto>>,
    ITransientDependency
{
    protected IPermissionManager PermissionManager { get; }
    protected IResourcePermissionManager ResourcePermissionManager { get; }

    public RoleDeletedEventHandler(IPermissionManager permissionManager, IResourcePermissionManager resourcePermissionManager)
    {
        PermissionManager = permissionManager;
        ResourcePermissionManager = resourcePermissionManager;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEto<IdentityRoleEto> eventData)
    {
        await PermissionManager.DeleteAsync(RolePermissionValueProvider.ProviderName, eventData.Entity.Name);
        await ResourcePermissionManager.DeleteAsync(RoleResourcePermissionValueProvider.ProviderName, eventData.Entity.Name);
    }
}
