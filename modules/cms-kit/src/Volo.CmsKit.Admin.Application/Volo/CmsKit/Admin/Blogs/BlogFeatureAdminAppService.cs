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

        protected IBlogFeatureManager BlogFeatureManager { get; }

        protected IDistributedEventBus EventBus { get; }

        public BlogFeatureAdminAppService(
            IBlogFeatureRepository blogFeatureRepository,
            IBlogFeatureManager blogFeatureManager,
            IDistributedEventBus eventBus)
        {
            BlogFeatureRepository = blogFeatureRepository;
            BlogFeatureManager = blogFeatureManager;
            EventBus = eventBus;
        }

        [Authorize(CmsKitAdminPermissions.Blogs.Features)]
        public async Task<List<BlogFeatureDto>> GetListAsync(Guid blogId)
        {
            var blogFeatures = await BlogFeatureManager.GetListAsync(blogId);

            return ObjectMapper.Map<List<BlogFeature>, List<BlogFeatureDto>>(blogFeatures);
        }

        [Authorize(CmsKitAdminPermissions.Blogs.Features)]
        public async Task SetAsync(Guid blogId, BlogFeatureInputDto dto)
        {
            var blogFeature = await BlogFeatureRepository.FindAsync(blogId, dto.FeatureName);
            if (blogFeature == null)
            {
                var newBlogFeature = new BlogFeature(blogId, dto.FeatureName, dto.IsEnabled);
                await BlogFeatureRepository.InsertAsync(newBlogFeature);
            }
            else
            {
                blogFeature.IsEnabled = dto.IsEnabled;
                await BlogFeatureRepository.UpdateAsync(blogFeature);
            }

            await EventBus.PublishAsync(new BlogFeatureChangedEto
            {
                BlogId = blogId,
                FeatureName = dto.FeatureName,
                IsEnabled = dto.IsEnabled
            });
        }
    }
}
