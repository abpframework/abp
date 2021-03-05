using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

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

            /* TODO: Creating transient entities in DefaultBlogFeatureProvider.GetDefaultFeaturesAsync
                     is not a good idea. Returned list will contain mixed (some in db some in-memory).
                     For example, if I delete/update one of the BlogFeature comes from in-memory,
                     I will have an strange behaviour. You should find another way.
             */
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
