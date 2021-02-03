using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.CmsKit.Blogs
{
    public abstract class BlogRepository_Test<TStartupModule> : CmsKitTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly CmsKitTestData testData;
        private readonly IBlogRepository blogRepository;

        public BlogRepository_Test()
        {
            this.testData = GetRequiredService<CmsKitTestData>();
            this.blogRepository = GetRequiredService<IBlogRepository>();
        }

        [Fact]
        public async Task GetByUrlSlugAsync_ShouldWorkProperly_WithExistingSlug()
        {
            var blog = await blogRepository.GetByUrlSlugAsync(testData.BlogUrlSlug);

            blog.ShouldNotBeNull();
            blog.UrlSlug.ShouldBe(testData.BlogUrlSlug);
            blog.Id.ShouldBe(testData.Blog_Id);
        }

        [Fact]
        public async Task GetByUrlSlugAsync_ShouldThrowException_WithNonExistingSlug()
        {
            var nonExistingSlug = "some-blog-slug-that-doesnt-exist";

            var exception = await Should.ThrowAsync<EntityNotFoundException>(
                    async () => await blogRepository.GetByUrlSlugAsync(nonExistingSlug));

            exception.ShouldNotBeNull();
            exception.EntityType.ShouldBe(typeof(Blog));
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnTrue_WithExistingId()
        {
            var result = await blogRepository.ExistsAsync(testData.Blog_Id);

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task ExistsAsync_ShouldReturnFalse_WithExistingId()
        {
            var nonExistingId = Guid.NewGuid();

            var result = await blogRepository.ExistsAsync(nonExistingId);

            result.ShouldBeFalse();
        }
    }
}
