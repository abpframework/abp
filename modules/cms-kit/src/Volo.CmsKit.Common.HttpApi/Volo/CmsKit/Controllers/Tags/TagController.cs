using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.CmsKit.Controllers;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Common.HttpApi.Volo.CmsKit.Controllers.Tags
{
    public class TagController : CmsKitControllerBase, ITagAppService
    {
        protected readonly ITagAppService TagAppService;

        public TagController(ITagAppService tagAppService)
        {
            TagAppService = tagAppService;
        }

        public Task<List<TagDto>> GetAllRelatedTagsAsync(GetRelatedTagsInput input)
        {
            return TagAppService.GetAllRelatedTagsAsync(input);
        }
    }
}
