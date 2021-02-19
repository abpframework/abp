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
        [Route("url/{url}")]
        public virtual Task<PageDto> FindByUrlAsync(string url)
        {
            return PageAppService.FindByUrlAsync(url);
        }
    }
}