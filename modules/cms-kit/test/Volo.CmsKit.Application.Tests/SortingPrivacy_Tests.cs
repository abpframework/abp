using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Validation;
using Volo.CmsKit.Admin.Comments;
using Volo.CmsKit.Public.Blogs;
using Xunit;

namespace Volo.CmsKit;

public class SortingPrivacy_Tests : CmsKitApplicationTestBase
{
    [Theory]
    [InlineData("email")]
    [InlineData("phoneNumber")]
    [InlineData("userName, PhoneNumber desc")]
    public async Task Author_Sorting_Should_Reject_Fields_Absent_From_The_Response(string sorting)
    {
        await Should.ThrowAsync<AbpValidationException>(() =>
            GetRequiredService<IBlogPostPublicAppService>().GetAuthorsHasBlogPostsAsync(
                new BlogPostFilteredPagedAndSortedResultRequestDto { Sorting = sorting }));
    }

    [Theory]
    [InlineData("author.email")]
    [InlineData("title, AUTHOR.PHONENUMBER desc")]
    public async Task Post_Sorting_Should_Reject_Hidden_Author_Fields(string sorting)
    {
        var data = GetRequiredService<CmsKitTestData>();
        await Should.ThrowAsync<AbpValidationException>(() =>
            GetRequiredService<IBlogPostPublicAppService>().GetListAsync(data.BlogSlug,
                new BlogPostGetListInput { Sorting = sorting }));
    }

    [Theory]
    [InlineData("author.email")]
    [InlineData("entityType, author.email")]
    [InlineData("creationTime desc, Author.PhoneNumber")]
    public async Task Admin_Comment_Sorting_Should_Reject_Author_Fields(string sorting)
    {
        await Should.ThrowAsync<AbpValidationException>(() =>
            GetRequiredService<ICommentAdminAppService>().GetListAsync(new CommentGetListInput { Sorting = sorting }));
    }

    [Theory]
    [InlineData("author.email")]
    [InlineData("title, Author.PhoneNumber desc")]
    public async Task Admin_Post_Sorting_Should_Reject_Author_Fields(string sorting)
    {
        await Should.ThrowAsync<AbpValidationException>(() =>
            GetRequiredService<Volo.CmsKit.Admin.Blogs.IBlogPostAdminAppService>().GetListAsync(new Volo.CmsKit.Admin.Blogs.BlogPostGetListInput { Sorting = sorting }));
    }

    [Fact]
    public async Task Admin_Table_Sorting_Should_Keep_Working()
    {
        (await GetRequiredService<ICommentAdminAppService>().GetListAsync(new CommentGetListInput { Sorting = "creationTime desc" }))
            .Items.ShouldNotBeEmpty();
        (await GetRequiredService<ICommentAdminAppService>().GetListAsync(new CommentGetListInput { Sorting = "url asc" }))
            .Items.ShouldNotBeEmpty();
        (await GetRequiredService<Volo.CmsKit.Admin.Blogs.IBlogPostAdminAppService>().GetListAsync(new Volo.CmsKit.Admin.Blogs.BlogPostGetListInput { Sorting = "title, slug desc, creationTime" }))
            .Items.ShouldNotBeEmpty();
    }

    [Theory]
    [InlineData("UserName DESC, Id asc", "Author.UserName, title desc")]
    [InlineData("username descending", "creationTime DeSc")]
    public async Task Public_Fields_Should_Keep_Working(string authorSorting, string postSorting)
    {
        var service = GetRequiredService<IBlogPostPublicAppService>();
        var data = GetRequiredService<CmsKitTestData>();

        (await service.GetAuthorsHasBlogPostsAsync(new BlogPostFilteredPagedAndSortedResultRequestDto { Sorting = authorSorting }))
            .Items.ShouldNotBeEmpty();
        (await service.GetListAsync(data.BlogSlug, new BlogPostGetListInput { Sorting = postSorting }))
            .Items.Count.ShouldBe(2);
    }
}
