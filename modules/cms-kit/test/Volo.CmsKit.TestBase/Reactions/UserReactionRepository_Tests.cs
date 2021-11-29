using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.CmsKit.Comments;
using Xunit;

namespace Volo.CmsKit.Reactions;

public abstract class UserReactionRepository_Tests<TStartupModule> : CmsKitTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly CmsKitTestData _cmsKitTestData;
    private readonly IUserReactionRepository __userReactionRepository;

    public UserReactionRepository_Tests()
    {
        _cmsKitTestData = GetRequiredService<CmsKitTestData>();
        __userReactionRepository = GetRequiredService<IUserReactionRepository>();
    }

    [Fact]
    public async Task FindAsync()
    {
        var reaction = await __userReactionRepository.FindAsync(
            _cmsKitTestData.User1Id,
            _cmsKitTestData.EntityType1,
            _cmsKitTestData.EntityId1,
            StandardReactions.Confused
        );

        reaction.ShouldNotBeNull();
        reaction.CreatorId.ShouldBe(_cmsKitTestData.User1Id);
        reaction.EntityId.ShouldBe(_cmsKitTestData.EntityId1);
        reaction.EntityType.ShouldBe(_cmsKitTestData.EntityType1);
        reaction.ReactionName.ShouldBe(StandardReactions.Confused);
    }

    [Fact]
    public async Task GetListForUserAsync()
    {
        var reactions = await __userReactionRepository.GetListForUserAsync(
            _cmsKitTestData.User1Id,
            _cmsKitTestData.EntityType1,
            _cmsKitTestData.EntityId1
        );

        reactions.Count.ShouldBe(2);
        reactions.Count(x => x.ReactionName == StandardReactions.Confused).ShouldBe(1);
        reactions.Count(x => x.ReactionName == StandardReactions.ThumbsUp).ShouldBe(1);
    }

    [Fact]
    public async Task GetSummariesAsync()
    {
        var summaries = await __userReactionRepository.GetSummariesAsync(
            _cmsKitTestData.EntityType1,
            _cmsKitTestData.EntityId1
        );

        summaries.Count.ShouldBe(2);
        summaries.First(x => x.ReactionName == StandardReactions.Confused).Count.ShouldBe(1);
        summaries.First(x => x.ReactionName == StandardReactions.ThumbsUp).Count.ShouldBe(2);

    }
}
