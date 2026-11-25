using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Uow;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Users;
using Xunit;

namespace Volo.CmsKit.PublicWebTests;

public class BlogPrivacy_Http_Tests : CmsKitPublicWebTestBase
{
    [Theory]
    [InlineData("authors", "email")]
    [InlineData("posts", "author.email")]
    public async Task Anonymous_Sorting_Should_Reject_Hidden_Author_Fields(string entry, string sorting)
    {
        var slug = entry == "authors" ? "authors" : GetRequiredService<CmsKitTestData>().BlogSlug;
        using var response = await Client.GetAsync("/api/cms-kit-public/blog-posts/" + slug +
            "?sorting=" + Uri.EscapeDataString(sorting));
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(BlogPostStatus.Draft, HttpStatusCode.NotFound)]
    [InlineData(BlogPostStatus.Published, HttpStatusCode.OK)]
    public async Task Anonymous_Detail_Should_Only_Return_Published_Posts(BlogPostStatus status, HttpStatusCode expected)
    {
        var data = GetRequiredService<CmsKitTestData>();
        BlogPost post;
        using (var uow = GetRequiredService<IUnitOfWorkManager>().Begin(requiresNew: true, isTransactional: false))
        {
            post = await GetRequiredService<BlogPostManager>().CreateAsync(
                await GetRequiredService<ICmsUserRepository>().GetAsync(data.User1Id),
                await GetRequiredService<IBlogRepository>().GetAsync(data.Blog_Id),
                "Visibility boundary", "visibility-boundary", status, content: "Private post content");
            await GetRequiredService<IBlogPostRepository>().InsertAsync(post);
            await uow.CompleteAsync();
        }

        using var response = await Client.GetAsync("/api/cms-kit-public/blog-posts/" + data.BlogSlug + "/" + post.Slug);
        response.StatusCode.ShouldBe(expected);
        if (status == BlogPostStatus.Published)
        {
            (await response.Content.ReadFromJsonAsync<BlogPostCommonDto>()).Content.ShouldBe("Private post content");
        }
        else
        {
            (await response.Content.ReadAsStringAsync()).ShouldNotContain("Private post content");
        }
    }
}
