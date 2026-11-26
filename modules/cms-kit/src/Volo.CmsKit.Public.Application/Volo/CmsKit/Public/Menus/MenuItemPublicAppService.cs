using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Authorization.Permissions;
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
    protected IPermissionChecker PermissionChecker { get; }

    public MenuItemPublicAppService(
        IMenuItemRepository menuRepository,
        IDistributedCache<List<MenuItemDto>> distributedCache,
        IPermissionChecker permissionChecker)
    {
        MenuItemRepository = menuRepository;
        DistributedCache = distributedCache;
        PermissionChecker = permissionChecker;
    }

    public virtual async Task<List<MenuItemDto>> GetListAsync()
    {
        var cachedMenu = await DistributedCache.GetOrAddAsync(
            MenuApplicationConsts.MainMenuCacheKey,
            async () =>
            {
                var menuItems = await MenuItemRepository.GetOrderedListAsync();

                if (menuItems == null)
                {
                    return new();
                }

                return ObjectMapper.Map<List<MenuItem>, List<MenuItemDto>>(menuItems);
            });

        return await FilterVisibleMenuItemsAsync(cachedMenu);
    }

    protected virtual async Task<List<MenuItemDto>> FilterVisibleMenuItemsAsync(List<MenuItemDto> menuItems)
    {
        if (menuItems.IsNullOrEmpty())
        {
            return new();
        }

        var permissionNames = menuItems.Where(x => !x.RequiredPermissionName.IsNullOrWhiteSpace())
            .Select(x => x.RequiredPermissionName).Distinct().ToArray();
        var grants = permissionNames.Length > 0
            ? await PermissionChecker.IsGrantedAsync(permissionNames)
            : new MultiplePermissionGrantResult();
        var children = menuItems.ToLookup(x => x.ParentId);
        var visibleIds = new HashSet<Guid>();
        var parents = new Queue<Guid?>();
        parents.Enqueue(null);
        while (parents.Count > 0)
        {
            foreach (var item in children[parents.Dequeue()])
            {
                if (!item.IsActive || (!item.RequiredPermissionName.IsNullOrWhiteSpace() &&
                    (!grants.Result.TryGetValue(item.RequiredPermissionName, out var grant) || grant != PermissionGrantResult.Granted)))
                {
                    continue;
                }

                if (visibleIds.Add(item.Id))
                {
                    parents.Enqueue(item.Id);
                }
            }
        }

        return menuItems.Where(x => visibleIds.Contains(x.Id)).ToList();
    }
}
