using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Menus
{
    public class PageChangedHandler: ILocalEventHandler<EntityUpdatedEventData<Page>>,
        ITransientDependency
    {
        protected IMenuRepository MenuRepository { get; }
        protected MenuManager MenuManager { get; }

        public PageChangedHandler(
            IMenuRepository menuRepository, 
            MenuManager menuManager)
        {
            MenuRepository = menuRepository;
            MenuManager = menuManager;
        }
        
        public async Task HandleEventAsync(EntityUpdatedEventData<Page> eventData)
        {
            // TODO: Write a repository query.
            var allMenus = await MenuRepository.GetListAsync(includeDetails: true);

            var affectedMenus = allMenus
                .Where(menu => menu.Items.Any(x => x.PageId == eventData.Entity.Id))
                .ToArray();

            var affectedMenuItems =
                affectedMenus.SelectMany(sm => sm.Items).Where(mItem => mItem.PageId == eventData.Entity.Id);

            foreach (var menuItem in affectedMenuItems)
            {
                MenuManager.SetPageUrl(menuItem, eventData.Entity);
            }

            await MenuRepository.UpdateManyAsync(affectedMenus);
        }
    }
}