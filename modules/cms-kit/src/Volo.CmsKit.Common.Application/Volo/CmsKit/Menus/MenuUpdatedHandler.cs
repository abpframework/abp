using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Volo.CmsKit.Public.Menus;

namespace Volo.CmsKit.Menus
{
    public class MenuUpdatedHandler :  ILocalEventHandler<EntityUpdatedEventData<Menu>>, ITransientDependency
    {
        protected IDistributedCache<MenuWithDetailsDto, MainMenuCacheKey> DistributedCache { get; }
        
        public MenuUpdatedHandler(IDistributedCache<MenuWithDetailsDto, MainMenuCacheKey> distributedCache)
        {
            DistributedCache = distributedCache;
        }

        public async Task HandleEventAsync(EntityUpdatedEventData<Menu> eventData)
        {
            await DistributedCache.RemoveAsync(new MainMenuCacheKey());
        }
    }
}