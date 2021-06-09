using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Menus
{
    [RequiresGlobalFeature(typeof(MenuFeature))]
    [RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Authorize(CmsKitAdminPermissions.Menus.Default)]
    [Route("api/cms-kit-admin/menus")]
    public class MenuAdminController : CmsKitAdminController, IMenuAdminAppService
    {
        protected IMenuAdminAppService MenuAdminAppService { get; }

        public MenuAdminController(IMenuAdminAppService menuAdminAppService)
        {
            MenuAdminAppService = menuAdminAppService;
        }

        [HttpPost]
        [Authorize(CmsKitAdminPermissions.Menus.Create)]
        public Task<MenuDto> CreateAsync(MenuCreateInput input)
        {
            return MenuAdminAppService.CreateAsync(input);
        }

        [Route("{menuId}/menu-items")]
        [HttpPost]
        [Authorize(CmsKitAdminPermissions.Menus.MenuItems.Create)]
        public Task<MenuItemDto> CreateMenuItemAsync(Guid menuId, MenuItemCreateInput input)
        {
            return MenuAdminAppService.CreateMenuItemAsync(menuId, input);
        }

        [Route("{menuId}")]
        [HttpDelete]
        [Authorize(CmsKitAdminPermissions.Menus.Delete)]
        public Task DeleteAsync(Guid menuId)
        {
            return MenuAdminAppService.DeleteAsync(menuId);
        }

        [Route("{menuId}/menu-items/{menuItemId}")]
        [HttpDelete]
        [Authorize(CmsKitAdminPermissions.Menus.MenuItems.Delete)]
        public Task DeleteMenuItemAsync(Guid menuId, Guid menuItemId)
        {
            return MenuAdminAppService.DeleteMenuItemAsync(menuId, menuItemId);
        }

        [Route("{menuId}/details")]
        [HttpGet]
        public Task<MenuWithDetailsDto> GetAsync(Guid menuId)
        {
            return MenuAdminAppService.GetAsync(menuId);
        }

        [Route("{menuId}")]
        [HttpGet]
        public Task<MenuDto> GetSimpleAsync(Guid menuId)
        {
            return MenuAdminAppService.GetSimpleAsync(menuId);
        }

        [HttpGet]
        public Task<PagedResultDto<MenuDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return MenuAdminAppService.GetListAsync(input);
        }

        [Route("{menuId}/menu-items/{menuItemId}/move")]
        [HttpPut]
        [Authorize(CmsKitAdminPermissions.Menus.MenuItems.Update)]
        public Task MoveMenuItemAsync(Guid menuId, Guid menuItemId, MenuItemMoveInput input)
        {
            return MenuAdminAppService.MoveMenuItemAsync(menuId, menuItemId, input);
        }

        [Route("{menuId}")]
        [HttpPut]
        [Authorize(CmsKitAdminPermissions.Menus.Update)]
        public Task<MenuDto> UpdateAsync(Guid menuId, MenuUpdateInput input)
        {
            return MenuAdminAppService.UpdateAsync(menuId, input);
        }

        [Route("{menuId}/menu-items/{menuItemId}")]
        [HttpPut]
        [Authorize(CmsKitAdminPermissions.Menus.MenuItems.Update)]
        public Task<MenuItemDto> UpdateMenuItemAsync(Guid menuId, Guid menuItemId, MenuItemUpdateInput input)
        {
            return MenuAdminAppService.UpdateMenuItemAsync(menuId, menuItemId, input);
        }
    }
}
