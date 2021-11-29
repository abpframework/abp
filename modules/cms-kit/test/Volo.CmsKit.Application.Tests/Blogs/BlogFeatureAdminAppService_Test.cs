using Shouldly;
using System.Threading.Tasks;
using Volo.CmsKit.Admin.Blogs;
using Xunit;

namespace Volo.CmsKit.Blogs
{
    public class BlogFeatureAdminAppService_Test : CmsKitApplicationTestBase
    {
        private readonly CmsKitTestData testData;
        private readonly IBlogFeatureAdminAppService blogFeatureAdminAppService;
        private readonly IBlogFeatureRepository blogFeatureRepository;

        public BlogFeatureAdminAppService_Test()
        {
            testData = GetRequiredService<CmsKitTestData>();
            blogFeatureAdminAppService = GetRequiredService<IBlogFeatureAdminAppService>();
            blogFeatureRepository = GetRequiredService<IBlogFeatureRepository>();
        }

        [Fact]
        public async Task SetAsync_ShouldWorkProperly_WithNonExistingFeature()
        {
            var dto = new BlogFeatureInputDto
            {
                FeatureName = "My.Awesome.Feature",
                IsEnabled = true
            };

            await blogFeatureAdminAppService.SetAsync(testData.Blog_Id, dto);

            var feature = await blogFeatureRepository.FindAsync(testData.Blog_Id, dto.FeatureName);

            feature.ShouldNotBeNull();
            feature.BlogId.ShouldBe(testData.Blog_Id);
            feature.IsEnabled.ShouldBe(dto.IsEnabled);
        }

        [Fact]
        public async Task SetAsync_ShouldWorkProperly_WithExistingFeature()
        {
            var dto = new BlogFeatureInputDto
            {
                FeatureName = testData.BlogFeature_2_FeatureName,
                IsEnabled = !testData.BlogFeature_2_Enabled
            };

            await blogFeatureAdminAppService.SetAsync(testData.Blog_Id, dto);

            var feature = await blogFeatureRepository.FindAsync(testData.Blog_Id, dto.FeatureName);

            feature.ShouldNotBeNull();
            feature.BlogId.ShouldBe(testData.Blog_Id);
            feature.IsEnabled.ShouldBe(dto.IsEnabled);
        }

        [Fact]
        public async Task GetListAsync_ShouldWorkProperly_WithBlogId()
        {
            var result = await blogFeatureAdminAppService.GetListAsync(testData.Blog_Id);

            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBeGreaterThanOrEqualTo(2); // 2 are seeded and there are built-in Features.
        }
    }
}
