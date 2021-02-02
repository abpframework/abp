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
    public abstract class BlogPostRepository_Test<TStartupModule> : CmsKitTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly CmsKitTestData testData;
        private readonly IBlogPostRepository blogPostRepository;

        public BlogPostRepository_Test()
        {
            testData = GetRequiredService<CmsKitTestData>();
            blogPostRepository = GetRequiredService<IBlogPostRepository>();
        }

        [Fact]
        public async Task SlugExistsAsync_ShouldReturnTrue_WithExistingUrlSlug()
        {
            var result = await blogPostRepository.SlugExistsAsync(testData.Blog_Id, testData.BlogPost_1_UrlSlug);

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task SlugExistsAsync_ShouldReturnFalse_WithNonExistingUrlSlug()
        {
            var nonExistingSlug = "any-other-url-slug";

            var result = await blogPostRepository.SlugExistsAsync(testData.Blog_Id, nonExistingSlug);

            result.ShouldBeFalse();
        }

        [Fact]
        public async Task SlugExistsAsync_ShouldReturnFalse_WithNonExistingBlogId()
        {
            var nonExistingBlogId = Guid.NewGuid();

            var result = await blogPostRepository.SlugExistsAsync(nonExistingBlogId, testData.BlogPost_1_UrlSlug);

            result.ShouldBeFalse();
        }

        [Fact]
        public async Task GetByUrlSlugAsync_ShouldWorkProperly_WithCorrectParameters()
        {
            var blogPost = await blogPostRepository.GetByUrlSlugAsync(testData.Blog_Id, testData.BlogPost_1_UrlSlug);

            blogPost.ShouldNotBeNull();
            blogPost.Id.ShouldBe(testData.BlogPost_1_Id);
            blogPost.UrlSlug.ShouldBe(testData.BlogPost_1_UrlSlug);
        }

        [Fact]
        public async Task GetByUrlSlugAsync_ShouldThrowException_WithNonExistingBlogPostUrlSlug()
        {
            var nonExistingSlugUrl = "absolutely-non-existing-url";
            var exception = await Should.ThrowAsync<EntityNotFoundException>(
                async () => await blogPostRepository.GetByUrlSlugAsync(testData.Blog_Id, nonExistingSlugUrl));

            exception.ShouldNotBeNull();
            exception.EntityType.ShouldBe(typeof(BlogPost));
        }

        [Fact]
        public async Task GetByUrlSlugAsync_ShouldThrowException_WithNonExistingBlogId()
        {
            var nonExistingBlogId = Guid.NewGuid();
            var exception = await Should.ThrowAsync<EntityNotFoundException>(
                async () => await blogPostRepository.GetByUrlSlugAsync(nonExistingBlogId, testData.BlogPost_1_UrlSlug));

            exception.ShouldNotBeNull();
            exception.EntityType.ShouldBe(typeof(BlogPost));
        }

        [Fact]
        public async Task GetPagedListAsync_ShouldWorkProperly_WithBlogId_WhileGetting10_WithoutSorting()
        {
            var result = await blogPostRepository.GetPagedListAsync(testData.Blog_Id, 0, 10, default);

            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetPagedListAsync_ShouldWorkProperly_WithBlogId_WhileGetting1_WithoutSorting()
        {
            var result = await blogPostRepository.GetPagedListAsync(testData.Blog_Id, default, 1, default);

            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetPagedListAsync_ShouldWorkProperly_WithBlogId_WhileGetting1InPage2_WithoutSorting()
        {
            var result = await blogPostRepository.GetPagedListAsync(testData.Blog_Id, 1, 1, default);

            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GetPagedListAsync_ShouldWorkProperly_WithBlogId_WhileGetting10_WithSortingByTitle()
        {
            var result = await blogPostRepository.GetPagedListAsync(testData.Blog_Id, default, 10, nameof(BlogPost.Title));

            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(2);
        }
    }
}
