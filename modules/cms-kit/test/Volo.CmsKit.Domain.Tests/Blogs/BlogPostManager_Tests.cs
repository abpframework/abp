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
            var newUrlSlug = "yet-another-post";

            var blogPost = await blogPostRepository.GetAsync(cmsKitTestData.BlogPost_1_Id);

            blogPost.SetTitle(newTitle);
            blogPost.SetUrlSlug(newUrlSlug);

            await blogPostManager.UpdateAsync(blogPost);

            var updated = await blogPostRepository.GetAsync(cmsKitTestData.BlogPost_1_Id);
            updated.Title.ShouldBe(newTitle);
            updated.UrlSlug.ShouldBe(newUrlSlug);
        }
    }
}
