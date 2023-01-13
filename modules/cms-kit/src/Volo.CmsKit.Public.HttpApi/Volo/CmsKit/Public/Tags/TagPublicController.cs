using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Public.Tags;

[RequiresFeature(CmsKitFeatures.TagEnable)]
[RequiresGlobalFeature(typeof(TagsFeature))]
[RemoteService(Name = CmsKitPublicRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitPublicRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit-public/tags")]
public class TagPublicController : CmsKitPublicControllerBase, ITagAppService
{
    protected ITagAppService TagAppService { get; }

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
