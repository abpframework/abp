using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Menus
{
    [RequiresGlobalFeature(typeof(MenuFeature))]
    [Authorize(CmsKitAdminPermissions.Menus.Default)]
    public class MenuItemAdminAppService : CmsKitAdminAppServiceBase, IMenuItemAdminAppService
    {
        protected MenuItemManager MenuManager { get; }
        protected IMenuItemRepository MenuItemRepository { get; }
        protected IPageRepository PageRepository { get; }

        public MenuItemAdminAppService(
            MenuItemManager menuManager,
            IMenuItemRepository menuRepository,
            IPageRepository pageRepository)
        {
            MenuManager = menuManager;
            MenuItemRepository = menuRepository;
            PageRepository = pageRepository;
        }

        public virtual async Task<ListResultDto<MenuItemDto>> GetListAsync()
        {
            var menuItems = await MenuItemRepository.GetListAsync();

            return new ListResultDto<MenuItemDto>(
                    ObjectMapper.Map<List<MenuItem>, List<MenuItemDto>>(menuItems)
            );
        }

        public virtual async Task<MenuItemDto> GetAsync(Guid id)
        {
            var menu = await MenuItemRepository.GetAsync(id);
            return ObjectMapper.Map<MenuItem, MenuItemDto>(menu);
        }

        [Authorize(CmsKitAdminPermissions.Menus.Create)]
        public virtual async Task<MenuItemDto> CreateAsync(MenuItemCreateInput input)
        {
            var menuItem = new MenuItem(
                    GuidGenerator.Create(),
                    input.DisplayName,
                    input.Url.IsNullOrEmpty() ? "#" : input.Url,
                    input.IsActive,
                    input.ParentId,
                    input.Icon,
                    input.Order,
                    input.Target,
                    input.ElementId,
                    input.CssClass
                );

            await MenuItemRepository.InsertAsync(menuItem);

            return ObjectMapper.Map<MenuItem, MenuItemDto>(menuItem);
        }

        [Authorize(CmsKitAdminPermissions.Menus.Update)]
        public virtual async Task<MenuItemDto> UpdateAsync(Guid id, MenuItemUpdateInput input)
        {
            var menuItem = await MenuItemRepository.GetAsync(id);

            if (input.PageId.HasValue)
            {
                MenuManager.SetPageUrl(menuItem, await PageRepository.GetAsync(input.PageId.Value));
            }
            else
            {
                menuItem.SetUrl(input.Url);
            }

            menuItem.SetDisplayName(input.DisplayName);
            menuItem.IsActive = input.IsActive;
            menuItem.Icon = input.Icon;
            menuItem.Target = input.Target;
            menuItem.ElementId = input.ElementId;
            menuItem.CssClass = input.CssClass;
            menuItem.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

            await MenuItemRepository.UpdateAsync(menuItem);

            return ObjectMapper.Map<MenuItem, MenuItemDto>(menuItem);
        }

        [Authorize(CmsKitAdminPermissions.Menus.Delete)]
        public virtual Task DeleteAsync(Guid id)
        {
            return MenuItemRepository.DeleteAsync(id);
        }

        [Authorize(CmsKitAdminPermissions.Menus.Update)]
        public virtual Task MoveMenuItemAsync(Guid id, MenuItemMoveInput input)
        {
            return MenuManager.MoveAsync(id, input.NewParentId, input.Position);
        }

        public virtual async Task<PagedResultDto<PageLookupDto>> GetPageLookupAsync(PageLookupInputDto input)
        {
            var count = await PageRepository.GetCountAsync(input.Filter);

            var pages = await PageRepository.GetListAsync(
                input.Filter,
                input.MaxResultCount,
                input.SkipCount,
                input.Sorting
            );

            return new PagedResultDto<PageLookupDto>(
                count,
                ObjectMapper.Map<List<Page>, List<PageLookupDto>>(pages)
            );
        }
    }
}