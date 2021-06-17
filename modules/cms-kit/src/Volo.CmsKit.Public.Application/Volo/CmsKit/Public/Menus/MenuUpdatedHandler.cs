using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.Public.Menus
{
    public class MenuUpdatedHandler : IDistributedEventHandler<MenuUpdatedEto>, ITransientDependency
    {
        protected IDistributedCache<MenuWithDetailsDto, MainMenuCacheKey> DistributedCache { get; }
        
        public MenuUpdatedHandler(IDistributedCache<MenuWithDetailsDto, MainMenuCacheKey> distributedCache)
        {
            DistributedCache = distributedCache;
        }

        public async Task HandleEventAsync(MenuUpdatedEto eventData)
        {
            await DistributedCache.RemoveAsync(new MainMenuCacheKey());
        }
    }
}