using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Menus
{
    [RequiresGlobalFeature(typeof(MenuFeature))]
    [RemoteService(Name = CmsKitAdminRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Authorize(CmsKitAdminPermissions.Menus.Default)]
    [Route("api/cms-kit-admin/menu-items")]
    public class MenuItemAdminController : CmsKitAdminController, IMenuItemAdminAppService
    {
        protected IMenuItemAdminAppService MenuItemAdminAppService { get; }

        public MenuItemAdminController(IMenuItemAdminAppService menuAdminAppService)
        {
            MenuItemAdminAppService = menuAdminAppService;
        }

        [HttpGet]
        public virtual Task<ListResultDto<MenuItemDto>> GetListAsync()
        {
            return MenuItemAdminAppService.GetListAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<MenuItemDto> GetAsync(Guid id)
        {
            return MenuItemAdminAppService.GetAsync(id);
        }

        [HttpPost]
        [Authorize(CmsKitAdminPermissions.Menus.Create)]
        public virtual Task<MenuItemDto> CreateAsync(MenuItemCreateInput input)
        {
            return MenuItemAdminAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(CmsKitAdminPermissions.Menus.Update)]
        public virtual Task<MenuItemDto> UpdateAsync(Guid id, MenuItemUpdateInput input)
        {
            return MenuItemAdminAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(CmsKitAdminPermissions.Menus.Delete)]
        public virtual Task DeleteAsync(Guid id)
        {
            return MenuItemAdminAppService.DeleteAsync(id);
        }

        [HttpPut]
        [Route("{id}/move")]
        [Authorize(CmsKitAdminPermissions.Menus.Update)]
        public virtual Task MoveMenuItemAsync(Guid id, MenuItemMoveInput input)
        {
            return MenuItemAdminAppService.MoveMenuItemAsync(id, input);
        }

        [HttpGet]
        [Route("lookup/pages")]
        public virtual Task<PagedResultDto<PageLookupDto>> GetPageLookupAsync(PageLookupInputDto input)
        {
            return MenuItemAdminAppService.GetPageLookupAsync(input);
        }
    }
}
