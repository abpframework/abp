using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.CmsKit.Comments;

public abstract class CommentRepository_Tests<TStartupModule> : CmsKitTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly CmsKitTestData _cmsKitTestData;
    private readonly ICommentRepository _commentRepository;

    public CommentRepository_Tests()
    {
        _cmsKitTestData = GetRequiredService<CmsKitTestData>();
        _commentRepository = GetRequiredService<ICommentRepository>();
    }

    [Fact]
    public async Task GetList_ShouldWork()
    {
        var comments = await _commentRepository.GetListAsync();

        comments.ShouldNotBeNull();
        comments.Count.ShouldBe(6);

        var headCommentId = comments.First(x => x.Comment.RepliedCommentId != null).Comment.RepliedCommentId;

        var replies = await _commentRepository.GetListAsync(repliedCommentId: headCommentId);

        replies.ShouldNotBeNull();
        replies.Count.ShouldBeLessThan(6);
        replies.Count.ShouldBeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task GetCount_ShouldWork()
    {
        var commentsCount = await _commentRepository.GetCountAsync();

        commentsCount.ShouldBe(6);
    }

    [Fact]
    public async Task GetListWithAuthorsAsync()
    {
        var list = await _commentRepository.GetListWithAuthorsAsync(_cmsKitTestData.EntityType1,
            _cmsKitTestData.EntityId1);

        list.Count.ShouldBe(4);
        list.Any(x => x.Comment == null).ShouldBeFalse();
        list.Any(x => x.Author == null).ShouldBeFalse();
    }

    [Fact]
    public async Task DeleteWithRepliesAsync()
    {
        var comment = await _commentRepository.GetAsync(_cmsKitTestData.CommentWithChildId);
        await _commentRepository.DeleteWithRepliesAsync(comment);

        var list = await _commentRepository.GetListAsync();

        list.Any(x =>
                x.Comment.Id == _cmsKitTestData.CommentWithChildId || x.Comment.RepliedCommentId == _cmsKitTestData.CommentWithChildId)
            .ShouldBeFalse();
    }
}
