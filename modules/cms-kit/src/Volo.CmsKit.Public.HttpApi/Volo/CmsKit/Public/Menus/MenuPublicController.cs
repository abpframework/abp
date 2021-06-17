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
    [Route("api/cms-kit-public/menus")]
    public class MenuPublicController : CmsKitPublicControllerBase, IMenuPublicAppService
    {
        protected  IMenuPublicAppService MenuPublicAppService { get; }

        public MenuPublicController(IMenuPublicAppService menuPublicAppService)
        {
            MenuPublicAppService = menuPublicAppService;
        }

        [Route("main-menu")]
        [HttpGet]
        public Task<MenuWithDetailsDto> GetMainMenuAsync()
        {
            return MenuPublicAppService.GetMainMenuAsync();
        }
    }
}