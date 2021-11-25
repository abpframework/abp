using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Domain.Entities;
using Volo.CmsKit.Admin.Comments;
using Xunit;

namespace Volo.CmsKit.Comments;

public class CommentAdminAppService_Tests : CmsKitApplicationTestBase
{
    private readonly ICommentAdminAppService _commentAdminAppService;
    private readonly CmsKitTestData _cmsKitTestData;

    public CommentAdminAppService_Tests()
    {
        _commentAdminAppService = GetRequiredService<ICommentAdminAppService>();
        _cmsKitTestData = GetRequiredService<CmsKitTestData>();
    }

    [Fact]
    public async Task ShouldGet_PagedListAsync()
    {
        var comments = await _commentAdminAppService.GetListAsync(new CommentGetListInput
        {
            MaxResultCount = 3
        });

        comments.TotalCount.ShouldBe(6);
        comments.Items.Count.ShouldBe(3);
        comments.Items.Any(x => x.Author != null).ShouldBeTrue();
    }

    [Fact]
    public async Task ShouldGet_CommentWithAuthorAsync()
    {
        var comment = await _commentAdminAppService.GetAsync(_cmsKitTestData.CommentWithChildId);

        comment.ShouldNotBeNull();
        comment.Author.ShouldNotBeNull();
    }

    [Fact]
    public async Task ShouldDelete_WithRepliesAsync()
    {
        await _commentAdminAppService.DeleteAsync(_cmsKitTestData.CommentWithChildId);

        await Should.ThrowAsync<EntityNotFoundException>(async () => await _commentAdminAppService.GetAsync(_cmsKitTestData.CommentWithChildId));
    }
}
