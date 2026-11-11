using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Localization;

namespace Volo.Abp.SettingManagement;

public class DefaultLanguageSettingChangedHandler :
    ILocalEventHandler<EntityChangedEventData<Setting>>,
    ITransientDependency
{
    protected IDistributedEventBus DistributedEventBus { get; }

    public DefaultLanguageSettingChangedHandler(IDistributedEventBus distributedEventBus)
    {
        DistributedEventBus = distributedEventBus;
    }

    public virtual async Task HandleEventAsync(EntityChangedEventData<Setting> eventData)
    {
        if (eventData.Entity.Name != LocalizationSettingNames.DefaultLanguage)
        {
            return;
        }

        await DistributedEventBus.PublishAsync(new LanguageChangedEto());
    }
}
