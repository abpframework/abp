using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Admin.Blogs;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Blogs
{
    public class BlogFeatureAdminAppService : CmsKitAdminAppServiceBase, IBlogFeatureAdminAppService
    {
        protected IBlogFeatureRepository BlogFeatureRepository { get; }
        protected IBlogFeatureManager BlogFeatureManager { get; }

        public BlogFeatureAdminAppService(
            IBlogFeatureRepository blogFeatureRepository,
            IBlogFeatureManager blogFeatureManager)
        {
            BlogFeatureRepository = blogFeatureRepository;
            BlogFeatureManager = blogFeatureManager;
        }

        [Authorize(CmsKitAdminPermissions.Blogs.Features)]
        public async Task<List<BlogFeatureDto>> GetListAsync(Guid blogId)
        {
            var blogFeatures = await BlogFeatureManager.GetListAsync(blogId);

            return ObjectMapper.Map<List<BlogFeature>, List<BlogFeatureDto>>(blogFeatures);
        }

        [Authorize(CmsKitAdminPermissions.Blogs.Features)]
        public async Task SetAsync(Guid blogId, BlogFeatureDto dto)
        {
            var blogFeature = await BlogFeatureRepository.FindAsync(blogId, dto.FeatureName);
            if (blogFeature == null)
            {
                var blogFeature = new BlogFeature(blogId, dto.FeatureName, dto.Enabled);
                await BlogFeatureRepository.InsertAsync(blogFeature);
            }
            else
            {
                blogFeature.Enabled = dto.Enabled;
                await BlogFeatureRepository.UpdateAsync(blogFeature);
            }
        }
    }
}
