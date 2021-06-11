using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.CmsKit.Public.Blogs;
using Xunit;

namespace Volo.CmsKit.Blogs
{
    public class BlogPostPublicAppService_Tests : CmsKitApplicationTestBase
    {
        private readonly IBlogPostPublicAppService blogPostAppService;

        private readonly CmsKitTestData cmsKitTestData;

        public BlogPostPublicAppService_Tests()
        {
            blogPostAppService = GetRequiredService<IBlogPostPublicAppService>();
            cmsKitTestData = GetRequiredService<CmsKitTestData>();
        }

        [Fact]
        public async Task GetListAsync_ShouldWorkProperly_WithExistingBlog()
        {
            var blogPosts = await blogPostAppService.GetListAsync(cmsKitTestData.BlogSlug, new PagedAndSortedResultRequestDto { MaxResultCount = 2 });

            blogPosts.ShouldNotBeNull();
            blogPosts.TotalCount.ShouldBe(2);
            blogPosts.Items.ShouldNotBeEmpty();
            blogPosts.Items.Count.ShouldBe(2);
        }

        [Fact]
        public async Task GetAsync_ShouldWorkProperly_WithExistingSlug()
        {
            var blogPost = await blogPostAppService.GetAsync(cmsKitTestData.BlogSlug, cmsKitTestData.BlogPost_1_Slug);

            blogPost.Id.ShouldBe(cmsKitTestData.BlogPost_1_Id);
            blogPost.Title.ShouldBe(cmsKitTestData.BlogPost_1_Title);
        }

        [Fact]
        public async Task GetAsync_ShouldThrowException_WithNonExistingBlogPostSlug()
        {
            var nonExistingSlug = "any-other-url";
            var exception = await Should.ThrowAsync<EntityNotFoundException>(async () =>
                                await blogPostAppService.GetAsync(cmsKitTestData.BlogSlug, nonExistingSlug));

            exception.EntityType.ShouldBe(typeof(BlogPost));
        }

        [Fact]
        public async Task GetAsync_ShouldThrowException_WithNonExistingBlogSlug()
        {
            var nonExistingSlug = "any-other-url";
            var exception = await Should.ThrowAsync<EntityNotFoundException>(async () =>
                                await blogPostAppService.GetAsync(nonExistingSlug, cmsKitTestData.Page_1_Slug));

            exception.EntityType.ShouldBe(typeof(Blog));
        }
    }
}
