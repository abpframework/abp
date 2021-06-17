using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;

namespace Volo.CmsKit.Menus
{
    public class MenuUpdatedHandler :  ILocalEventHandler<EntityUpdatedEventData<Menu>>, ITransientDependency
    {
        protected IDistributedCache<MenuWithDetailsDto> DistributedCache { get; }
        
        public MenuUpdatedHandler(IDistributedCache<MenuWithDetailsDto> distributedCache)
        {
            DistributedCache = distributedCache;
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<Menu> eventData)
        {
            await DistributedCache.RemoveAsync(MenuApplicationConsts.MainMenuCacheKey);
        }
    }
}