using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.CmsKit.Blogs;

namespace Volo.CmsKit.Public.Blogs
{
    public class BlogFeaturePublicAppService : CmsKitPublicAppServiceBase, IBlogFeaturePublicAppService
    {
        protected IBlogFeatureRepository BlogFeatureRepository { get; }
        protected IDistributedCache<BlogFeatureDto> Cache { get; }

        public BlogFeaturePublicAppService(
            IBlogFeatureRepository blogFeatureRepository,
            IDistributedCache<BlogFeatureDto> cache)
        {
            BlogFeatureRepository = blogFeatureRepository;
            Cache = cache;
        }

        public async Task<BlogFeatureDto> GetAsync(Guid blogId, string featureName)
        {
            return await Cache.GetOrAddAsync(
                $"{blogId}_{featureName}",
                () => GetFromDatabaseAsync(blogId, featureName),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
                });
        }

        private async Task<BlogFeatureDto> GetFromDatabaseAsync(Guid blogId, string featureName)
        {
            var feature = await BlogFeatureRepository.FindAsync(blogId, featureName);
            if (feature == null)
            {
                feature = new BlogFeature(blogId, featureName);
            }

            return ObjectMapper.Map<BlogFeature, BlogFeatureDto>(feature);
        }

        public async Task<List<BlogFeatureDto>> GetManyAsync(Guid blogId, GetBlogFeatureInput input)
        {
            var features = await BlogFeatureRepository.GetListAsync(blogId);

            var nonExistingFeatures = input.FeatureNames.Where(x => !features.Any(a => a.FeatureName == x));

            foreach (var featureName in nonExistingFeatures)
            {
                features.Add(new BlogFeature(blogId, featureName));
            }

            return ObjectMapper.Map<List<BlogFeature>, List<BlogFeatureDto>>(features);
        }
    }
}
