using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Contents;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Public.Contents
{
    [RequiresGlobalFeature(typeof(ContentsFeature))]
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-public/contents")]
    public class ContentController : CmsKitControllerBase, IContentPublicAppService
    {
        protected IContentPublicAppService ContentAppService { get; }

        public ContentController(IContentPublicAppService contentAppService)
        {
            ContentAppService = contentAppService;
        }

        [HttpGet]
        public virtual Task<ContentDto> GetAsync(GetContentInput input)
        {
            return ContentAppService.GetAsync(input);
        }
    }
}
