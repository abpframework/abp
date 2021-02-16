using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Public.Blogs;
using Xunit;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeaturePublicAppService_Test : CmsKitApplicationTestBase
    {
        private readonly CmsKitTestData testData;
        private readonly IBlogFeaturePublicAppService blogFeaturePublicAppService;

        public BlogFeaturePublicAppService_Test()
        {
            testData = GetRequiredService<CmsKitTestData>();
            blogFeaturePublicAppService = GetRequiredService<IBlogFeaturePublicAppService>();
        }

        [Fact]
        public async Task GetAsync_ShouldWorkProperly_WithExistingFeatureName()
        {
            var result = await blogFeaturePublicAppService.GetAsync(testData.Blog_Id, testData.BlogFeature_1_FeatureName);

            result.ShouldNotBeNull();
            result.FeatureName.ShouldBe(testData.BlogFeature_1_FeatureName);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnDefault_WithNonExistingFeatureName()
        {
            var nonExistingFeatureName = "AnyOtherFeature";
            var result = await blogFeaturePublicAppService.GetAsync(testData.Blog_Id, nonExistingFeatureName);

            var defaultFeature = new BlogFeature(Guid.Empty, nonExistingFeatureName);
            result.ShouldNotBeNull();
            result.Enabled.ShouldBe(defaultFeature.Enabled);
        }
    }
}
