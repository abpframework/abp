using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Menus
{
    public class PageChangedHandler: ILocalEventHandler<EntityCreatedEventData<Page>>,
        ITransientDependency
    {
        protected IMenuRepository MenuRepository { get; }
        
        public PageChangedHandler(IMenuRepository menuRepository)
        {
            MenuRepository = menuRepository;
        }
        
        public Task HandleEventAsync(EntityCreatedEventData<Page> eventData)
        {
            // TODO: Find a way to get affected MenuItems.
            throw new NotImplementedException();
        }
    }
}