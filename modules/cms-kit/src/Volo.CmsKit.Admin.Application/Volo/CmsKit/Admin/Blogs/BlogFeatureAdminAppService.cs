using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs
{
    [RequiresGlobalFeature(typeof(BlogsFeature))]
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

        [Authorize(CmsKitAdminPermissions.Blogs.Features)]
        public virtual async Task<List<BlogFeatureDto>> GetListAsync(Guid blogId)
        {
            var blogFeatures = await BlogFeatureManager.GetListAsync(blogId);

            return ObjectMapper.Map<List<BlogFeature>, List<BlogFeatureDto>>(blogFeatures);
        }

        [Authorize(CmsKitAdminPermissions.Blogs.Features)]
        public virtual Task SetAsync(Guid blogId, BlogFeatureInputDto dto)
        {
            return BlogFeatureManager.SetAsync(blogId, dto.FeatureName, dto.IsEnabled);
        }
    }
}
