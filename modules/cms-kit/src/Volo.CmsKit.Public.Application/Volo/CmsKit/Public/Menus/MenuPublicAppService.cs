using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.Public.Menus
{
    [RequiresGlobalFeature(typeof(MenuFeature))]
    public class MenuPublicAppService : CmsKitPublicAppServiceBase, IMenuPublicAppService
    {
        protected IMenuRepository MenuRepository { get; }

        protected IDistributedCache<MenuWithDetailsDto, MainMenuCacheKey> DistributedCache { get; }

        public MenuPublicAppService(
            IMenuRepository menuRepository,
            IDistributedCache<MenuWithDetailsDto, MainMenuCacheKey> distributedCache)
        {
            MenuRepository = menuRepository;
            DistributedCache = distributedCache;
        }

        public async Task<MenuWithDetailsDto> GetMainMenuAsync()
        {
            var cachedMenu = await DistributedCache.GetOrAddAsync(
                new MainMenuCacheKey(),
                async () =>
                {
                    var menu = await MenuRepository.FindMainMenuAsync(includeDetails: true);
                    
                    if (menu == null)
                    {
                        return new MenuWithDetailsDto(); 
                    }

                    return ObjectMapper.Map<Menu, MenuWithDetailsDto>(menu);
                });

            return cachedMenu;
        }
    }
}