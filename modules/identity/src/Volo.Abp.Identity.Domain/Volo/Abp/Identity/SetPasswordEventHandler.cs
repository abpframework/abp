using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Users;

namespace Volo.Abp.Identity;
public class SetPasswordEventHandler :
    IDistributedEventHandler<UserPasswordChangeRequestedEto>,
    ITransientDependency
{
    protected IdentityUserManager UserManager { get; }

    public SetPasswordEventHandler(
        IdentityUserManager permissionManager)
    {
        UserManager = permissionManager;
    }

    public async Task HandleEventAsync(UserPasswordChangeRequestedEto eventData)
    {
        await UserManager.SetPasswordAsync(
            eventData.TenantId,
            eventData.UserName,
            eventData.Password);
    }
}