using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.CmsKit.Public.Blogs;
using Volo.CmsKit.Users;
using Xunit;

namespace Volo.CmsKit.Blogs;

public class BlogPostPublicAppService_Tests : CmsKitApplicationTestBase
{
    private readonly IBlogPostPublicAppService blogPostAppService;

    private readonly CmsKitTestData cmsKitTestData;

    private readonly ICmsUserRepository userRepository;
    
    private readonly IBlogRepository blogRepository;
    private readonly IBlogPostRepository blogPostRepository;
    
    private readonly BlogPostManager blogPostManager;
    public BlogPostPublicAppService_Tests()
    {
        blogPostAppService = GetRequiredService<IBlogPostPublicAppService>();
        cmsKitTestData = GetRequiredService<CmsKitTestData>();
        userRepository = GetRequiredService<ICmsUserRepository>();
        blogRepository = GetRequiredService<IBlogRepository>();
        blogPostManager = GetRequiredService<BlogPostManager>();
        blogPostRepository = GetRequiredService<IBlogPostRepository>();
    }

    [Fact]
    public async Task GetListAsync_ShouldWorkProperly_WithExistingBlog()
    {
        var blogPosts = await blogPostAppService.GetListAsync(cmsKitTestData.BlogSlug, new BlogPostGetListInput { MaxResultCount = 2 });

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

    [Fact]
    public async Task GetListAsync_ShouldFilterByUser()
    {
        var user2 = await userRepository.GetAsync(cmsKitTestData.User2Id);
        var blog = await blogRepository.GetAsync(cmsKitTestData.Blog_Id);
        
        var blogPost = await blogPostManager.CreateAsync(
            user2,
            blog,
            cmsKitTestData.BlogPost_1_Title + "by user2",
            cmsKitTestData.BlogPost_1_Slug + "by user2",
            BlogPostStatus.Published,
            "Short desc 1",
            "Blog Post 1 Content"
        );

        await blogPostRepository.InsertAsync(blogPost);

        //should get all not filtered by user
        var blogPosts = await blogPostAppService.GetListAsync(cmsKitTestData.BlogSlug,
            new BlogPostGetListInput {});

        blogPosts.ShouldNotBeNull();
        blogPosts.TotalCount.ShouldBe(3);
        blogPosts.Items.ShouldNotBeEmpty();
        blogPosts.Items.Count.ShouldBe(3);
        
        //should get only one filtered by user
        blogPosts = await blogPostAppService.GetListAsync(cmsKitTestData.BlogSlug,
            new BlogPostGetListInput {AuthorId = user2.Id});

        blogPosts.ShouldNotBeNull();
        blogPosts.TotalCount.ShouldBe(1);
        blogPosts.Items.ShouldNotBeEmpty();
        blogPosts.Items.Count.ShouldBe(1);
        
        //should get filtered by user1
        blogPosts = await blogPostAppService.GetListAsync(cmsKitTestData.BlogSlug,
            new BlogPostGetListInput {AuthorId = cmsKitTestData.User1Id});

        blogPosts.ShouldNotBeNull();
        blogPosts.TotalCount.ShouldBe(2);
        blogPosts.Items.ShouldNotBeEmpty();
        blogPosts.Items.Count.ShouldBe(2);
    }
    
    [Fact]
    public async Task GetListAsync_ShouldNotGet_UnPublishedItems()
    {
        var user2 = await userRepository.GetAsync(cmsKitTestData.User2Id);
        var blog = await blogRepository.GetAsync(cmsKitTestData.Blog_Id);
        
        var draftBlogPost1 = await blogPostManager.CreateAsync(
            user2,
            blog,
            cmsKitTestData.BlogPost_1_Title + "draft1",
            cmsKitTestData.BlogPost_1_Slug + "draft1",
            BlogPostStatus.Draft,
            "Short desc 1",
            "Blog Post 1 Content"
        );
        
        var draftBlogPost2 = await blogPostManager.CreateAsync(
            user2,
            blog,
            cmsKitTestData.BlogPost_1_Title + "draft2",
            cmsKitTestData.BlogPost_1_Slug + "draft2",
            BlogPostStatus.Draft,
            "Short desc 1",
            "Blog Post 1 Content"
        );
        
        var publishedBlogPost1 = await blogPostManager.CreateAsync(
            user2,
            blog,
            cmsKitTestData.BlogPost_1_Title + "published1",
            cmsKitTestData.BlogPost_1_Slug + "published1",
            BlogPostStatus.Published,
            "Short desc 1",
            "Blog Post 1 Content"
        );

        await blogPostRepository.InsertAsync(draftBlogPost1);
        await blogPostRepository.InsertAsync(draftBlogPost2);
        await blogPostRepository.InsertAsync(publishedBlogPost1);

        //should get all not filtered by user
        var blogPosts = await blogPostAppService.GetListAsync(cmsKitTestData.BlogSlug,
            new BlogPostGetListInput {});

        blogPosts.ShouldNotBeNull();
        blogPosts.TotalCount.ShouldBe(3);
        blogPosts.Items.ShouldNotBeEmpty();
        blogPosts.Items.Count.ShouldBe(3);
        blogPosts.Items.Any(x => x.Id == draftBlogPost1.Id).ShouldBeFalse();
        blogPosts.Items.Any(x => x.Id == draftBlogPost2.Id).ShouldBeFalse();
        blogPosts.Items.Any(x => x.Id == publishedBlogPost1.Id).ShouldBeTrue();

        var allItemsFromRepository = await blogPostRepository.GetListAsync();
        allItemsFromRepository.ShouldNotBeNull();
        allItemsFromRepository.Count.ShouldBe(5);//3 new added 2 already existing
        allItemsFromRepository.Any(x => x.Id == draftBlogPost1.Id).ShouldBeTrue();
        allItemsFromRepository.Any(x => x.Id == draftBlogPost2.Id).ShouldBeTrue();
        allItemsFromRepository.Any(x => x.Id == publishedBlogPost1.Id).ShouldBeTrue();
    }
}
