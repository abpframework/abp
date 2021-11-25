using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs;

[RequiresGlobalFeature(typeof(BlogsFeature))]
[Authorize(CmsKitAdminPermissions.Blogs.Features)]
public class BlogFeatureAdminAppService : CmsKitAdminAppServiceBase, IBlogFeatureAdminAppService
{
    protected IBlogFeatureRepository BlogFeatureRepository { get; }

    protected BlogFeatureManager BlogFeatureManager { get; }

    protected IDistributedEventBus EventBus { get; }

    public BlogFeatureAdminAppService(
        IBlogFeatureRepository blogFeatureRepository,
        BlogFeatureManager blogFeatureManager,
        IDistributedEventBus eventBus)
    {
        BlogFeatureRepository = blogFeatureRepository;
        BlogFeatureManager = blogFeatureManager;
        EventBus = eventBus;
    }

    public virtual async Task<List<BlogFeatureDto>> GetListAsync(Guid blogId)
    {
        var blogFeatures = await BlogFeatureRepository.GetListAsync(blogId);

        return ObjectMapper.Map<List<BlogFeature>, List<BlogFeatureDto>>(blogFeatures);
    }

    public virtual Task SetAsync(Guid blogId, BlogFeatureInputDto dto)
    {
        return BlogFeatureManager.SetAsync(blogId, dto.FeatureName, dto.IsEnabled);
    }
}
