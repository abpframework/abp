using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Blogs;

[RequiresGlobalFeature(typeof(BlogsFeature))]
[RemoteService(Name = CmsKitCommonRemoteServiceConsts.RemoteServiceName)]
[Area(CmsKitCommonRemoteServiceConsts.ModuleName)]
[Route("api/cms-kit/blogs/{blogId}/features")]
public class BlogFeatureController : CmsKitControllerBase, IBlogFeatureAppService
{
    protected IBlogFeatureAppService BlogFeatureAppService { get; }

    public BlogFeatureController(IBlogFeatureAppService blogFeatureAppService)
    {
        BlogFeatureAppService = blogFeatureAppService;
    }

    [HttpGet]
    [Route("{featureName}")]
    public Task<BlogFeatureDto> GetOrDefaultAsync(Guid blogId, string featureName)
    {
        return BlogFeatureAppService.GetOrDefaultAsync(blogId, featureName);
    }
}
