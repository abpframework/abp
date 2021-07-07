using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Menus
{
    public class PageChangedHandler : 
        ILocalEventHandler<EntityUpdatedEventData<Page>>,
        ITransientDependency
    {
        protected IMenuItemRepository MenuRepository { get; }
        protected MenuItemManager MenuManager { get; }

        public PageChangedHandler(
            IMenuItemRepository menuRepository,
            MenuItemManager menuManager)
        {
            MenuRepository = menuRepository;
            MenuManager = menuManager;
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<Page> eventData)
        {
            // TODO: Write a repository query.
            var allMenuItems = await MenuRepository.GetListAsync();

            var affectedMenuItems = allMenuItems
                                    .Where(x => x.PageId == eventData.Entity.Id)
                                    .ToArray();

            foreach (var menuItem in affectedMenuItems)
            {
                MenuManager.SetPageUrl(menuItem, eventData.Entity);
            }

            await MenuRepository.UpdateManyAsync(affectedMenuItems);
        }
    }
}