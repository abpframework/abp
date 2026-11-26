using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Blogs;

[RequiresGlobalFeature(typeof(BlogsFeature))]
public class BlogFeatureAppService : CmsKitAppServiceBase, IBlogFeatureAppService
{
    protected virtual IBlogFeatureRepository BlogFeatureRepository { get; }

    protected virtual IDistributedCache<BlogFeatureCacheItem, BlogFeatureCacheKey> Cache { get; }

    public BlogFeatureAppService(
        IBlogFeatureRepository blogFeatureRepository,
        IDistributedCache<BlogFeatureCacheItem, BlogFeatureCacheKey> blogFeatureCacheManager)
    {
        BlogFeatureRepository = blogFeatureRepository;
        Cache = blogFeatureCacheManager;
    }

    public virtual async Task<BlogFeatureDto> GetOrDefaultAsync(Guid blogId, string featureName)
    {
        var key = new BlogFeatureCacheKey(blogId, featureName);
        var cacheItem = await Cache.GetAsync(key);
        if (cacheItem == null)
        {
            cacheItem = await GetOrDefaultFromRepositoryAsync(blogId, featureName);
            if (cacheItem.Id != Guid.Empty)
            {
                await Cache.SetAsync(key, cacheItem);
            }
        }

        return ObjectMapper.Map<BlogFeatureCacheItem, BlogFeatureDto>(cacheItem);
    }

    protected virtual async Task<BlogFeatureCacheItem> GetOrDefaultFromRepositoryAsync(Guid blogId, string featureName)
    {
        var feature = await BlogFeatureRepository.FindAsync(blogId, featureName);
        var blogFeature = feature ?? new BlogFeature(blogId, featureName);

        return ObjectMapper.Map<BlogFeature, BlogFeatureCacheItem>(blogFeature);
    }
}
