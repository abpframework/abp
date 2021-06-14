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
using Volo.Abp.UI.Navigation;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Menus;
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
        public Task<MenuWithDetailsDto> CreateAsync(MenuCreateInput input)
        {
            return MenuAdminAppService.CreateAsync(input);
        }

        [Route("{menuId}/menu-items/{menuItemId}")]        
        [Authorize(CmsKitAdminPermissions.Menus.Update)]
        [HttpGet]
        public Task<MenuItemDto> GetMenuItemAsync(Guid menuId, Guid menuItemId)
        {
            return MenuAdminAppService.GetMenuItemAsync(menuId, menuItemId);
        }

        [Route("{menuId}/menu-items")]
        [HttpPost]
        [Authorize(CmsKitAdminPermissions.Menus.Update)]
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
        [Authorize(CmsKitAdminPermissions.Menus.Update)]
        public Task DeleteMenuItemAsync(Guid menuId, Guid menuItemId)
        {
            return MenuAdminAppService.DeleteMenuItemAsync(menuId, menuItemId);
        }

        [Route("{menuId}")]
        [HttpGet]
        public Task<MenuWithDetailsDto> GetAsync(Guid menuId)
        {
            return MenuAdminAppService.GetAsync(menuId);
        }

        [HttpGet]
        public Task<PagedResultDto<MenuWithDetailsDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return MenuAdminAppService.GetListAsync(input);
        }

        [Route("{menuId}/menu-items/{menuItemId}/move")]
        [HttpPut]
        [Authorize(CmsKitAdminPermissions.Menus.Update)]
        public Task MoveMenuItemAsync(Guid menuId, Guid menuItemId, MenuItemMoveInput input)
        {
            return MenuAdminAppService.MoveMenuItemAsync(menuId, menuItemId, input);
        }
        
        [Route("{menuId}/main-menu")]
        [HttpPut]        
        [Authorize(CmsKitAdminPermissions.Menus.Update)]
        public Task UpdateMainMenuAsync(Guid menuId, UpdateMainMenuInput input)
        {
            return MenuAdminAppService.UpdateMainMenuAsync(menuId, input);
        }

        [Route("{menuId}")]
        [HttpPut]
        [Authorize(CmsKitAdminPermissions.Menus.Update)]
        public Task<MenuWithDetailsDto> UpdateAsync(Guid menuId, MenuUpdateInput input)
        {
            return MenuAdminAppService.UpdateAsync(menuId, input);
        }

        [Route("{menuId}/menu-items/{menuItemId}")]
        [HttpPut]
        [Authorize(CmsKitAdminPermissions.Menus.Update)]
        public Task<MenuItemDto> UpdateMenuItemAsync(Guid menuId, Guid menuItemId, MenuItemUpdateInput input)
        {
            return MenuAdminAppService.UpdateMenuItemAsync(menuId, menuItemId, input);
        }

        [HttpGet]
        [Route("lookup/pages")]
        public virtual Task<PagedResultDto<PageLookupDto>> GetPageLookupAsync([FromQuery]PageLookupInputDto input)
        {
            return MenuAdminAppService.GetPageLookupAsync(input);
        }
    }
}
