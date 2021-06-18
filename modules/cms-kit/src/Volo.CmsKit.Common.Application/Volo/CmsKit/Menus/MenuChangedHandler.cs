using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Volo.CmsKit.Menus
{
    public class MenuChangedHandler :
      ILocalEventHandler<EntityUpdatedEventData<Menu>>,
      ILocalEventHandler<EntityDeletedEventData<Menu>>,
      ITransientDependency
    {
        protected IDistributedCache<MenuWithDetailsDto> DistributedCache { get; }
        
        public MenuChangedHandler(IDistributedCache<MenuWithDetailsDto> distributedCache)
        {
            DistributedCache = distributedCache;
        }

        public Task HandleEventAsync(EntityUpdatedEventData<Menu> eventData)
        {
            return DistributedCache.RemoveAsync(MenuApplicationConsts.MainMenuCacheKey);
        }

        public Task HandleEventAsync(EntityDeletedEventData<Menu> eventData)
        {
            return DistributedCache.RemoveAsync(MenuApplicationConsts.MainMenuCacheKey);
        }
    }
}