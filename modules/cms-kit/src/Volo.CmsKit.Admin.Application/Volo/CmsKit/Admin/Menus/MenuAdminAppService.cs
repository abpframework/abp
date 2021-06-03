using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
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


        [Authorize(CmsKitAdminPermissions.Menus.Create)]
        public async Task<MenuDto> CreateAsync(MenuCreateInput input)
        {
            var menu = ObjectMapper.Map<MenuCreateInput, Menu>(input);

            foreach (var item in input.Items)
            {
                menu.Items.Add(await MapToMenuItemAsync(item));
            }

            await MenuRepository.InsertAsync(menu);

            return ObjectMapper.Map<Menu, MenuDto>(menu);
        }

        private async Task<MenuItem> MapToMenuItemAsync(MenuItemCreateInput dto)
        {
            var menuItem = new MenuItem(
                GuidGenerator.Create(),
                dto.MenuId,
                dto.DisplayName,
                dto.Url,
                dto.IsActive,
                dto.ParentMenuId)

            menuItem.SetDisplayName(dto.DisplayName);
            menuItem.SetUrl(dto.Url);

            if (dto.PageId.HasValue)
            {
                var page = await PageRepository.GetAsync(dto.PageId.Value, includeDetails: false);

                MenuManager.SetPageUrl(menuItem, page);
            }

            foreach (var item in dto.MenuItems)
            {
                menuItem.MenuItems.Add(await MapToMenuItemAsync(item));
            }

            return menuItem;
        }

        [Authorize(CmsKitAdminPermissions.Menus.Delete)]
        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<MenuDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<MenuDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            throw new NotImplementedException();
        }

        [Authorize(CmsKitAdminPermissions.Menus.Update)]
        public Task<MenuDto> UpdateAsync(Guid id, MenuCreateInput input)
        {
            throw new NotImplementedException();
        }
    }
}
