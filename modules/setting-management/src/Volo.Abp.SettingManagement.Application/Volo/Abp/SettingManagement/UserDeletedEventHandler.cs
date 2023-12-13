using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Settings;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.SettingManagement;

public class UserDeletedEventHandler :
    IDistributedEventHandler<EntityDeletedEto<UserEto>>,
    ITransientDependency
{
    protected ISettingManager SettingManager { get; }

    public UserDeletedEventHandler(ISettingManager settingManager)
    {
        SettingManager = settingManager;
    }

    [UnitOfWork]
    public virtual async Task HandleEventAsync(EntityDeletedEto<UserEto> eventData)
    {
        await SettingManager.DeleteAsync(UserSettingValueProvider.ProviderName, eventData.Entity.Id.ToString());
    }
}
