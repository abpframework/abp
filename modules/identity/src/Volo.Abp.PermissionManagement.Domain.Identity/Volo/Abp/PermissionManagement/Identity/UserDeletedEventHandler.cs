using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.PermissionManagement.Identity;

public class UserDeletedEventHandler :
    IDistributedEventHandler<EntityDeletedEto<UserEto>>,
    ITransientDependency
{
    protected IPermissionManager PermissionManager { get; }

    public UserDeletedEventHandler(IPermissionManager permissionManager)
    {
        PermissionManager = permissionManager;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEto<UserEto> eventData)
    {
        await PermissionManager.DeleteAsync(UserPermissionValueProvider.ProviderName, eventData.Entity.Id.ToString());
    }
}
