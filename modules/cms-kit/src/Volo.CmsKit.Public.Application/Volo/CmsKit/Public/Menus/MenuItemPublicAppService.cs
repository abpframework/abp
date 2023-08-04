using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.Public.Menus;

[RequiresFeature(CmsKitFeatures.MenuEnable)]
[RequiresGlobalFeature(typeof(MenuFeature))]
public class MenuItemPublicAppService : CmsKitPublicAppServiceBase, IMenuItemPublicAppService
{
    protected IMenuItemRepository MenuItemRepository { get; }
    protected IDistributedCache<List<MenuItemDto>> DistributedCache { get; }

    public MenuItemPublicAppService(
        IMenuItemRepository menuRepository,
        IDistributedCache<List<MenuItemDto>> distributedCache)
    {
        MenuItemRepository = menuRepository;
        DistributedCache = distributedCache;
    }

    public virtual async Task<List<MenuItemDto>> GetListAsync()
    {
        var cachedMenu = await DistributedCache.GetOrAddAsync(
            MenuApplicationConsts.MainMenuCacheKey,
            async () =>
            {
                var menuItems = await MenuItemRepository.GetListAsync();

                if (menuItems == null)
                {
                    return new();
                }

                return ObjectMapper.Map<List<MenuItem>, List<MenuItemDto>>(menuItems);
            });

        return cachedMenu;
    }
}
