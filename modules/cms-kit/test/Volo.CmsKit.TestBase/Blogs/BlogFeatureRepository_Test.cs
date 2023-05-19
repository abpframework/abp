using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.CmsKit.Blogs;

public abstract class BlogFeatureRepository_Test<TStartupModule> : CmsKitTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly CmsKitTestData testData;
    private readonly IBlogFeatureRepository blogFeatureRepository;
    public BlogFeatureRepository_Test()
    {
        testData = GetRequiredService<CmsKitTestData>();
        blogFeatureRepository = GetRequiredService<IBlogFeatureRepository>();
    }

    [Fact]
    public async Task GetListAsync_ShouldWorkProperly_WithBlogId()
    {
        var result = await blogFeatureRepository.GetListAsync(testData.Blog_Id);

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetListAsync_ShouldWorkProperly_WithBlogIdWithFeatureNames()
    {
        var result = await blogFeatureRepository.GetListAsync(testData.Blog_Id, new List<string> { testData.BlogFeature_1_FeatureName });

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public async Task FindAsync_ShouldWorkProperly_WithExistingFeatureName()
    {
        var result = await blogFeatureRepository.FindAsync(testData.Blog_Id, testData.BlogFeature_1_FeatureName);

        result.ShouldNotBeNull();
        result.FeatureName.ShouldBe(testData.BlogFeature_1_FeatureName);
        result.IsEnabled.ShouldBe(testData.BlogFeature_1_Enabled);
    }

    [Fact]
    public async Task FindAsync_ShouldReturnNull_WithNonExistingFeatureName()
    {
        var nonExistingFeatureName = "Some.Feature";
        var result = await blogFeatureRepository.FindAsync(testData.Blog_Id, nonExistingFeatureName);

        result.ShouldBeNull();
    }

    [Fact]
    public async Task FindAsync_ShouldReturnNull_WithNonExistingBlogId()
    {
        var nonExistingBlogId = Guid.NewGuid();
        var result = await blogFeatureRepository.FindAsync(nonExistingBlogId, testData.BlogFeature_1_FeatureName);

        result.ShouldBeNull();
    }
}
