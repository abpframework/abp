using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Menus
{
    [RequiresGlobalFeature(typeof(MenuFeature))]
    [Authorize(CmsKitAdminPermissions.Menus.Default)]
    public class MenuAdminAppService : CmsKitAdminAppServiceBase, IMenuAdminAppService
    {
        protected MenuManager MenuManager { get; }
        protected IMenuRepository MenuRepository { get; }
        protected IPageRepository PageRepository { get; }

        public MenuAdminAppService(
            MenuManager menuManager,
            IMenuRepository menuRepository,
            IPageRepository pageRepository)
        {
            MenuManager = menuManager;
            MenuRepository = menuRepository;
            PageRepository = pageRepository;
        }

        public async Task<PagedResultDto<MenuDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var menus = await MenuRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);

            return new PagedResultDto<MenuDto>(
                await MenuRepository.GetCountAsync(),
                ObjectMapper.Map<List<Menu>, List<MenuDto>>(menus)
            );
        }

        public async Task<MenuWithDetailsDto> GetAsync(Guid id)
        {
            var menu = await MenuRepository.GetAsync(id, includeDetails: true);
            return ObjectMapper.Map<Menu, MenuWithDetailsDto>(menu);
        }

        public async Task<MenuDto> GetSimpleAsync(Guid id)
        {
            var menu = await MenuRepository.GetAsync(id, includeDetails: false);
            return ObjectMapper.Map<Menu, MenuDto>(menu);
        }

        [Authorize(CmsKitAdminPermissions.Menus.Create)]
        public async Task<MenuDto> CreateAsync(MenuCreateInput input)
        {
            var menu = new Menu(GuidGenerator.Create(), CurrentTenant.Id, input.Name);

            await MenuRepository.InsertAsync(menu);

            return ObjectMapper.Map<Menu, MenuDto>(menu);
        }

        [Authorize(CmsKitAdminPermissions.Menus.Update)]
        public async Task<MenuDto> UpdateAsync(Guid menuId, MenuUpdateInput input)
        {
            var menu = await MenuRepository.GetAsync(menuId);

            menu.SetName(input.Name);

            await MenuRepository.UpdateAsync(menu);

            return ObjectMapper.Map<Menu, MenuDto>(menu);
        }

        [Authorize(CmsKitAdminPermissions.Menus.Delete)]
        public virtual Task DeleteAsync(Guid menuId)
        {
            return MenuRepository.DeleteAsync(menuId);
        }

        public async Task<MenuItemDto> GetMenuItemAsync(Guid menuId, Guid menuItemId)
        {
            var menu = await MenuRepository.GetAsync(menuId);

            var menuItem = menu.Items.FirstOrDefault(x => x.Id == menuItemId)
                           ?? throw new EntityNotFoundException(typeof(MenuItem), menuItemId);

            return ObjectMapper.Map<MenuItem, MenuItemDto>(menuItem);
        }

        [Authorize(CmsKitAdminPermissions.Menus.MenuItems.Create)]
        public virtual async Task<MenuItemDto> CreateMenuItemAsync(Guid menuId, MenuItemCreateInput input)
        {
            var menu = await MenuRepository.GetAsync(menuId, includeDetails: true);

            var menuItem =
                new MenuItem(
                    GuidGenerator.Create(),
                    menuId,
                    input.DisplayName,
                    input.Url.IsNullOrEmpty() ? "#" : input.Url,
                    input.IsActive,
                    input.ParentId,
                    input.Icon,
                    input.Order,
                    input.Target,
                    input.ElementId,
                    input.CssClass,
                    input.RequiredPermissionName);

            if (input.PageId.HasValue)
            {
                var page = await PageRepository.GetAsync(input.PageId.Value);
                MenuManager.SetPageUrl(menuItem, page);
            }

            menu.Items.Add(menuItem);

            MenuManager.OrganizeTreeOrderForMenuItem(menu, menuItem);

            await MenuRepository.UpdateAsync(menu);

            return ObjectMapper.Map<MenuItem, MenuItemDto>(menuItem);
        }

        [Authorize(CmsKitAdminPermissions.Menus.MenuItems.Update)]
        public virtual async Task<MenuItemDto> UpdateMenuItemAsync(Guid menuId, Guid menuItemId,
            MenuItemUpdateInput input)
        {
            var menu = await MenuRepository.GetAsync(menuId, includeDetails: true);

            var menuItem = menu.Items.FirstOrDefault(x => x.Id == menuItemId)
                           ?? throw new EntityNotFoundException(typeof(MenuItem), menuItemId);

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
            menuItem.RequiredPermissionName = input.RequiredPermissionName;

            await MenuRepository.UpdateAsync(menu);

            return ObjectMapper.Map<MenuItem, MenuItemDto>(menuItem);
        }

        [Authorize(CmsKitAdminPermissions.Menus.MenuItems.Delete)]
        public virtual async Task DeleteMenuItemAsync(Guid menuId, Guid menuItemId)
        {
            var menu = await MenuRepository.GetAsync(menuId, includeDetails: true);

            var menuItem = menu.Items.FirstOrDefault(x => x.Id == menuItemId)
                           ?? throw new EntityNotFoundException(typeof(MenuItem), menuItemId);

            menu.Items.Remove(menuItem);

            await MenuRepository.UpdateAsync(menu);
        }

        [Authorize(CmsKitAdminPermissions.Menus.MenuItems.Update)]
        public virtual Task MoveMenuItemAsync(Guid menuId, Guid menuItemId, MenuItemMoveInput input)
        {
            return MenuManager.MoveAsync(menuId, menuItemId, input.NewParentId, input.Position);
        }

        public virtual async Task UpdateMainMenuAsync(Guid menuId, UpdateMainMenuInput input)
        {
            if (input.IsMainMenu)
            {
                await MenuManager.SetMainMenuAsync(menuId);
            }
            else
            {
                await MenuManager.UnSetMainMenuAsync(menuId);
            }
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