using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;

namespace Volo.CmsKit.Menus
{
    public class MenuUpdatedHandler : ILocalEventHandler<EntityUpdatedEventData<Menu>>, ITransientDependency
    {
        public IDistributedEventBus EventBus { get; }

        public MenuUpdatedHandler(IDistributedEventBus eventBus)
        {
            EventBus = eventBus;
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<Menu> eventData)
        {
            await EventBus.PublishAsync(new MenuUpdatedEto
            {
                MenuId = eventData.Entity.Id
            });
        }
    }
}