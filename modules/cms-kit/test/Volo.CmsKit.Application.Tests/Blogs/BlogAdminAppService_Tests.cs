using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.CmsKit.Admin.Blogs;
using Xunit;

namespace Volo.CmsKit.Blogs;

public class BlogAdminAppService_Tests : CmsKitApplicationTestBase
{
    protected IBlogAdminAppService BlogAdminAppService { get; }
    protected CmsKitTestData CmsKitTestData { get; }
    protected IBlogRepository BlogRepository { get; }

    public BlogAdminAppService_Tests()
    {
        BlogAdminAppService = GetRequiredService<IBlogAdminAppService>();
        CmsKitTestData = GetRequiredService<CmsKitTestData>();
        BlogRepository = GetRequiredService<IBlogRepository>();
    }

    [Fact]
    public async Task GetAsync()
    {
        var blog = await BlogAdminAppService.GetAsync(CmsKitTestData.Blog_Id);

        blog.Slug.ShouldBe(CmsKitTestData.BlogSlug);
    }

    [Fact]
    public async Task GetListAsync()
    {
        var blogs = await BlogAdminAppService.GetListAsync(new BlogGetListInput());

        blogs.TotalCount.ShouldBeGreaterThan(0);
        blogs.Items.Any(x => x.Slug == CmsKitTestData.BlogSlug).ShouldBeTrue();
    }

    [Fact]
    public async Task CreateAsync_ShouldWork()
    {
        var blog = await BlogAdminAppService.CreateAsync(new CreateBlogDto
        {
            Name = "News",
            Slug = "latest-news"
        });

        blog.ShouldNotBeNull();
        blog.Name.ShouldBe("News");
        blog.Slug.ShouldBe("latest-news");
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WithExistSlug()
    {
        await Should.ThrowAsync<BlogSlugAlreadyExistException>(
            async () =>
        await BlogAdminAppService.CreateAsync(new CreateBlogDto
        {
            Name = "News",
            Slug = CmsKitTestData.BlogSlug
        }));
    }

    [Fact]
    public async Task UpdateAsync_ShouldWork()
    {
        var blog = await BlogAdminAppService.UpdateAsync(CmsKitTestData.Blog_Id, new UpdateBlogDto
        {
            Name = "New Name",
            Slug = "new-slug"
        });

        var updatedBlog = await BlogAdminAppService.GetAsync(CmsKitTestData.Blog_Id);

        updatedBlog.Name.ShouldBe("New Name");
        updatedBlog.Slug.ShouldBe("new-slug");
    }

    [Fact]
    public async Task DeleteAsync_ShouldWork()
    {
        await BlogAdminAppService.DeleteAsync(CmsKitTestData.Blog_Id);

        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await BlogAdminAppService.GetAsync(CmsKitTestData.Blog_Id)
        );
    }
}
