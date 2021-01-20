using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace Volo.CmsKit.Public.Pages
{
    [RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-public/comments")]
    public class PagesPublicController : IPageAppService
    {
        protected readonly IPageAppService PageAppService;

        public PagesPublicController(IPageAppService pageAppService)
        {
            PageAppService = pageAppService;
        }

        [HttpGet]
        [Route("url/{url}")]
        public Task<PageDto> FindByUrlAsync(string url)
        {
            return PageAppService.FindByUrlAsync(url);
        }
    }
}