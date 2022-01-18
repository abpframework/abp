using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.CmsKit.Users;
using Xunit;

namespace Volo.CmsKit.Comments;

public class CommentManager_Test : CmsKitDomainTestBase
{
    private readonly CommentManager commentManager;
    private readonly CmsKitTestData testData;
    private readonly ICmsUserRepository userRepository;

    public CommentManager_Test()
    {
        commentManager = GetRequiredService<CommentManager>();
        testData = GetRequiredService<CmsKitTestData>();
        userRepository = GetRequiredService<ICmsUserRepository>();
    }

    [Fact]
    public async Task CreateAsync_ShouldWorkProperly_WithCorrectData()
    {
        var creator = await userRepository.GetAsync(testData.User1Id);

        var text = "Thank you for the article. It's awesome";

        var comment = await commentManager.CreateAsync(creator, testData.EntityType1, testData.EntityId1, text);

        comment.Id.ShouldNotBe(Guid.Empty);
        comment.CreatorId.ShouldBe(creator.Id);
        comment.EntityType.ShouldBe(testData.EntityType1);
        comment.EntityId.ShouldBe(testData.EntityId1);
        comment.Text.ShouldBe(text);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WithNotConfiguredEntityType()
    {
        var creator = await userRepository.GetAsync(testData.User1Id);
        var notConfiguredEntityType = "Some.New.Entity";
        var text = "Thank you for the article. It's awesome";

        var exception = await Should.ThrowAsync<EntityNotCommentableException>(async () =>
                            await commentManager.CreateAsync(
                                creator,
                                notConfiguredEntityType,
                                testData.EntityId1,
                                text));

        exception.ShouldNotBeNull();
        exception.EntityType.ShouldBe(notConfiguredEntityType);
    }
}
