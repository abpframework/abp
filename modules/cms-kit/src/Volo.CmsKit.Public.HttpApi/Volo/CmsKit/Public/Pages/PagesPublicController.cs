using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace Volo.CmsKit.Public.Pages
{
    [RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-public/comments")]
    public class PagesPublicController
    {
        protected readonly IPageAppService PageAppService;

        public PagesPublicController(IPageAppService pageAppService)
        {
            PageAppService = pageAppService;
        }

        [HttpGet]
        [Route("url/{url}")]
        public Task<PageDto> GetByUrlAsync(string url)
        {
            return PageAppService.GetByUrlAsync(url);
        }
    }
}