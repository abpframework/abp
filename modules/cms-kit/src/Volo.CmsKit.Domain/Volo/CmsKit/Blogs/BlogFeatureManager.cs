using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Blogs;

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

    public async Task SetDefaultsAsync(Guid blogId)
    {
        var defaultFeatures = await DefaultBlogFeatureProvider.GetDefaultFeaturesAsync(blogId);

        foreach (var feature in defaultFeatures)
        {
            await SetAsync(blogId, feature.FeatureName, isEnabled: true);
        }
    }
}
