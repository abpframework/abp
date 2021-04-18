using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.Pages
{
    [RequiresGlobalFeature(typeof(PagesFeature))]
    [RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-public/pages")]
    public class PagesPublicController : IPagePublicAppService
    {
        protected IPagePublicAppService PageAppService { get; }

        public PagesPublicController(IPagePublicAppService pageAppService)
        {
            PageAppService = pageAppService;
        }

        [HttpGet]
        [Route("{slug}")]
        public virtual Task<PageDto> FindBySlugAsync(string slug)
        {
            return PageAppService.FindBySlugAsync(slug);
        }
    }
}