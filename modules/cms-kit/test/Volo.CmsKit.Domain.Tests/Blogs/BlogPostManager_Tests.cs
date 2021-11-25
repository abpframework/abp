using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.Users;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;
using Xunit;

namespace Volo.CmsKit.Blogs;

public class BlogPostManager_Tests : CmsKitDomainTestBase
{
    private readonly BlogPostManager blogPostManager;
    private readonly IGuidGenerator guidGenerator;
    private readonly IBlogPostRepository blogPostRepository;
    private readonly IBlogRepository blogRepository;
    private readonly ICmsUserRepository userRepository;
    private readonly CmsKitTestData cmsKitTestData;

    public BlogPostManager_Tests()
    {
        blogPostManager = GetRequiredService<BlogPostManager>();
        guidGenerator = GetRequiredService<IGuidGenerator>();
        blogPostRepository = GetRequiredService<IBlogPostRepository>();
        blogRepository = GetRequiredService<IBlogRepository>();
        cmsKitTestData = GetRequiredService<CmsKitTestData>();
        userRepository = GetRequiredService<ICmsUserRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldWorkProperly_WithExistingUserAndBlog()
    {
        var title = "New blog post";
        var slug = "new-blog-post";

        var author = await userRepository.GetAsync(cmsKitTestData.User1Id);

        var blog = await blogRepository.GetAsync(cmsKitTestData.Blog_Id);

        var blogPost = await blogPostManager.CreateAsync(author, blog, title, slug);

        blogPost.Id.ShouldNotBe(Guid.Empty);
        blogPost.Title.ShouldBe(title);
        blogPost.Slug.ShouldBe(slug);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenSlugAlreadyExists()
    {

        var author = await userRepository.GetAsync(cmsKitTestData.User1Id);

        var blog = await blogRepository.GetAsync(cmsKitTestData.Blog_Id);

        await Should.ThrowAsync<BlogPostSlugAlreadyExistException>(async () =>
            await blogPostManager.CreateAsync(author, blog, "Any New Title", cmsKitTestData.BlogPost_1_Slug));
    }

    [Fact]
    public async Task SetSlugAsync_ShouldWorkProperly_WithNonExistingSlug()
    {
        var newSlug = "yet-another-post";

        var blogPost = await blogPostRepository.GetAsync(cmsKitTestData.BlogPost_1_Id);

        await blogPostManager.SetSlugUrlAsync(blogPost, newSlug);

        blogPost.Slug.ShouldBe(newSlug);
    }

    [Fact]
    public async Task SetSlugAsync_ShouldThrowException_WithExistingSlug()
    {
        var blogPost = await blogPostRepository.GetAsync(cmsKitTestData.BlogPost_1_Id);

        var exception = await Should.ThrowAsync<BlogPostSlugAlreadyExistException>(async () =>
            await blogPostManager.SetSlugUrlAsync(blogPost, cmsKitTestData.BlogPost_2_Slug));

        exception.BlogId.ShouldBe(blogPost.BlogId);
        exception.Slug.ShouldBe(cmsKitTestData.BlogPost_2_Slug);
    }
}
