using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.CmsKit.Tags;
using Xunit;

namespace Volo.CmsKit.Blogs
{
    public class BlogPostManager_Tests : CmsKitDomainTestBase
    {
        private readonly IBlogPostManager blogPostManager;
        private readonly IGuidGenerator guidGenerator;
        private readonly IBlogPostRepository blogPostRepository;
        private readonly CmsKitTestData cmsKitTestData;

        public BlogPostManager_Tests()
        {
            this.blogPostManager = GetRequiredService<IBlogPostManager>();
            this.guidGenerator = GetRequiredService<IGuidGenerator>();
            this.blogPostRepository = GetRequiredService<IBlogPostRepository>();
            this.cmsKitTestData = GetRequiredService<CmsKitTestData>();
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkProperly_WithCorrectData()
        {
            var title = "New blog post";
            var urlSlug = "new-blog-post";

            var created = await blogPostManager.CreateAsync(
                new BlogPost(guidGenerator.Create(), cmsKitTestData.Blog_Id, title, urlSlug));

            created.Id.ShouldNotBe(Guid.Empty);

            var blogPost = await blogPostRepository.GetAsync(created.Id);
            blogPost.Title.ShouldBe(title);
            blogPost.UrlSlug.ShouldBe(urlSlug);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenUrlSlugAlreadyExists()
        {
            var blogPost = new BlogPost(guidGenerator.Create(), cmsKitTestData.Blog_Id, "Any New Title", cmsKitTestData.BlogPost_1_UrlSlug);

            await Should.ThrowAsync<BlogPostUrlSlugAlreadyExistException>(async () =>
                await blogPostManager.CreateAsync(blogPost));
        }

        [Fact]
        public async Task UpdateAsync_ShoudlWorkProperly_WithCorrectData()
        {
            var newTitle = "Yet Another Post";

            var blogPost = await blogPostRepository.GetAsync(cmsKitTestData.BlogPost_1_Id);

            blogPost.SetTitle(newTitle);

            await blogPostManager.UpdateAsync(blogPost);

            var updated = await blogPostRepository.GetAsync(cmsKitTestData.BlogPost_1_Id);
            updated.Title.ShouldBe(newTitle);
        }

        [Fact]
        public async Task SetUrlSlugAsync_ShouldWorkProperly_WithNonExistingSlug()
        {
            var newUrlSlug = "yet-another-post";

            var blogPost = await blogPostRepository.GetAsync(cmsKitTestData.BlogPost_1_Id);

            await blogPostManager.SetSlugUrlAsync(blogPost, newUrlSlug);

            blogPost.UrlSlug.ShouldBe(newUrlSlug);
        }

        [Fact]
        public async Task SetUrlSlugAsync_ShouldThrowException_WithExistingSlug()
        {
            var blogPost = await blogPostRepository.GetAsync(cmsKitTestData.BlogPost_1_Id);

            var exception = await Should.ThrowAsync<BlogPostUrlSlugAlreadyExistException>(async () =>
                await blogPostManager.SetSlugUrlAsync(blogPost, cmsKitTestData.BlogPost_2_UrlSlug));

            exception.BlogId.ShouldBe(blogPost.BlogId);
            exception.UrlSlug.ShouldBe(cmsKitTestData.BlogPost_2_UrlSlug);
        }
    }
}
