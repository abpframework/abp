using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Menus;

namespace Volo.CmsKit.Public.Menus
{
    [RequiresGlobalFeature(typeof(BlogsFeature))]
    [RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-public/menus/items")]
    public class MenuItemPublicController : CmsKitPublicControllerBase, IMenuItemPublicAppService
    {
        protected  IMenuItemPublicAppService MenuPublicAppService { get; }

        public MenuItemPublicController(IMenuItemPublicAppService menuPublicAppService)
        {
            MenuPublicAppService = menuPublicAppService;
        }

        [HttpGet]
        public Task<List<MenuItemDto>> GetMenuItemsAsync()
        {
            return MenuPublicAppService.GetMenuItemsAsync();
        }
    }
}