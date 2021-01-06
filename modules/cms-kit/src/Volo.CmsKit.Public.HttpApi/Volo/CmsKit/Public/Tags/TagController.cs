using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Public.Tags
{
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit/tags")]
    public class TagController : CmsKitPublicControllerBase, ITagAppService
    {
        protected readonly ITagAppService TagAppService;

        public TagController(ITagAppService tagAppService)
        {
            TagAppService = tagAppService;
        }

        [HttpGet]
        public Task<List<TagDto>> GetAllRelatedTagsAsync(GetRelatedTagsInput input)
        {
            return TagAppService.GetAllRelatedTagsAsync(input);
        }
    }
}
