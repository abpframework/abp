using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Public.Tags
{
    [RequiresGlobalFeature(typeof(TagsFeature))]
    [RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
    [Area("cms-kit")]
    [Route("api/cms-kit-public/tags")]
    public class TagPublicController : CmsKitPublicControllerBase, ITagAppService
    {
        protected readonly ITagAppService TagAppService;

        public TagPublicController(ITagAppService tagAppService)
        {
            TagAppService = tagAppService;
        }

        [HttpGet]
        [Route("{entityType}/{entityId}")]
        public Task<List<TagDto>> GetAllRelatedTagsAsync(string entityType, string entityId)
        {
            return TagAppService.GetAllRelatedTagsAsync(entityType, entityId);
        }
    }
}