using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Uow;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeatureManager : DomainService
    {
        protected IBlogFeatureRepository BlogFeatureRepository { get; }

        protected IDefaultBlogFeatureProvider DefaultBlogFeatureProvider { get; }

        public BlogFeatureManager(
            IBlogFeatureRepository blogFeatureRepository,
            IDefaultBlogFeatureProvider defaultBlogFeatureProvider)
        {
            BlogFeatureRepository = blogFeatureRepository;
            DefaultBlogFeatureProvider = defaultBlogFeatureProvider;
        }

        public async Task<List<BlogFeature>> GetListAsync(Guid blogId)
        {
            var blogFeatures = await BlogFeatureRepository.GetListAsync(blogId);

            var defaultFeatures = await DefaultBlogFeatureProvider.GetDefaultFeaturesAsync(blogId);

            defaultFeatures.ForEach(x => blogFeatures.AddIfNotContains(x));

            return blogFeatures;
        }

        public async Task SetAsync(Guid blogId, string featureName, bool isEnabled)
        {
            var blogFeature = await BlogFeatureRepository.FindAsync(blogId, featureName);
            if (blogFeature == null)
            {
                var newBlogFeature = new BlogFeature(blogId, featureName, isEnabled);
                await BlogFeatureRepository.InsertAsync(newBlogFeature);
            }
            else
            {
                blogFeature.IsEnabled = isEnabled;
                await BlogFeatureRepository.UpdateAsync(blogFeature);
            }
        }
    }
}
