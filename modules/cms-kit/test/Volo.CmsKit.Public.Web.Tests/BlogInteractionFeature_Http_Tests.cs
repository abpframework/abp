using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Uow;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.GlobalFeatures;
using Xunit;

namespace Volo.CmsKit.PublicWebTests;

public class BlogInteractionFeature_Http_Tests : CmsKitPublicWebTestBase
{
    [Fact]
    public async Task Comment_Widget_Should_Respect_The_Blog_Comments_Feature()
    {
        await SetCommentsFeatureAsync(false);
        using (var response = await ValidateCommentAsync())
        {
            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        await SetCommentsFeatureAsync(true);
        using (var response = await ValidateCommentAsync())
        {
            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }
    }

    private async Task SetCommentsFeatureAsync(bool enabled)
    {
        using var uow = GetRequiredService<IUnitOfWorkManager>().Begin(requiresNew: true, isTransactional: false);
        await GetRequiredService<BlogFeatureManager>().SetAsync(GetRequiredService<CmsKitTestData>().Blog_Id, CommentsFeature.Name, enabled);
        await uow.CompleteAsync();
    }

    private async Task<HttpResponseMessage> ValidateCommentAsync()
    {
        var data = GetRequiredService<CmsKitTestData>();
        using var request = new HttpRequestMessage(HttpMethod.Post, "/CmsKitPublicComments/Validate");
        request.Headers.Add("X-Test-User", data.User1Id.ToString());
        request.Content = JsonContent.Create(new
        {
            EntityType = BlogPostConsts.EntityType,
            EntityId = data.BlogPost_1_Id.ToString(),
            Text = "Feature boundary",
            IdempotencyToken = Guid.NewGuid().ToString("N")
        });

        return await Client.SendAsync(request);
    }
}
