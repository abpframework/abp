using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Controllers.Tags
{
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit/tags")]
    public class TagController : CmsKitControllerBase, ITagAppService
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
