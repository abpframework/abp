using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;

namespace Volo.Abp.PermissionManagement.Identity
{
    // public class RoleDeletedEventHandler :
    //     ILocalEventHandler<EntityDeletedEventData<IdentityRole>>,
    //     ITransientDependency
    // {
    //     protected IPermissionManager PermissionManager { get; }
    //
    //     public RoleDeletedEventHandler(IPermissionManager permissionManager)
    //     {
    //         PermissionManager = permissionManager;
    //     }
    //
    //     public virtual async Task HandleEventAsync(EntityDeletedEventData<IdentityRole> eventData)
    //     {
    //         await PermissionManager.DeleteAsync(RolePermissionValueProvider.ProviderName, eventData.Entity.Name);
    //     }
    // }
}
