using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Public.Blogs;
using Xunit;

namespace Volo.CmsKit.Blogs;

public class BlogFeaturePublicAppService_Test : CmsKitApplicationTestBase
{
    private readonly CmsKitTestData testData;
    private readonly IBlogFeatureAppService blogFeatureAppService;

    public BlogFeaturePublicAppService_Test()
    {
        testData = GetRequiredService<CmsKitTestData>();
        blogFeatureAppService = GetRequiredService<IBlogFeatureAppService>();
    }

    [Fact]
    public async Task GetAsync_ShouldWorkProperly_WithExistingFeatureName()
    {
        var result = await blogFeatureAppService.GetOrDefaultAsync(testData.Blog_Id, testData.BlogFeature_1_FeatureName);

        result.ShouldNotBeNull();
        result.FeatureName.ShouldBe(testData.BlogFeature_1_FeatureName);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnDefault_WithNonExistingFeatureName()
    {
        var nonExistingFeatureName = "AnyOtherFeature";
        var result = await blogFeatureAppService.GetOrDefaultAsync(testData.Blog_Id, nonExistingFeatureName);

        var defaultFeature = new BlogFeature(Guid.Empty, nonExistingFeatureName);
        result.ShouldNotBeNull();
        result.IsEnabled.ShouldBe(defaultFeature.IsEnabled);
    }
}
