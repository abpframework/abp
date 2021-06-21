using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.CmsKit.Reactions
{
    public class ReactionManager_Tests : CmsKitDomainTestBase
    {
        private readonly CmsKitTestData _cmsKitTestData;
        private readonly ReactionManager _reactionManager;

        public ReactionManager_Tests()
        {
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
            _reactionManager = GetRequiredService<ReactionManager>();
        }

        [Fact]
        public async Task GetReactionsAsync()
        {
            var reactions = await _reactionManager.GetReactionsAsync(_cmsKitTestData.EntityType1);

            reactions.Count.ShouldBe(12);

            var reactionsByEntityType = await _reactionManager.GetReactionsAsync(_cmsKitTestData.EntityType1);

            reactionsByEntityType.Count.ShouldBe(12);
        }

        [Fact]
        public async Task GetSummariesAsync()
        {
            var summary = await _reactionManager.GetSummariesAsync(
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1
                );

            summary.Single(s=>s.Reaction.Name == StandardReactions.ThumbsUp).Count.ShouldBe(2);
            summary.Single(s=>s.Reaction.Name == StandardReactions.Confused).Count.ShouldBe(1);
            summary.Single(s=>s.Reaction.Name == StandardReactions.Rocket).Count.ShouldBe(0);
        }

        [Fact]
        public async Task CreateAsync()
        {
            var reaction = await _reactionManager.GetOrCreateAsync(
                _cmsKitTestData.User2Id,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId2,
                StandardReactions.Eyes
                );

            reaction.CreatorId.ShouldBe(_cmsKitTestData.User2Id);
            reaction.EntityType.ShouldBe(_cmsKitTestData.EntityType1);
            reaction.EntityId.ShouldBe(_cmsKitTestData.EntityId2);
            reaction.ReactionName.ShouldBe(StandardReactions.Eyes);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            await _reactionManager.DeleteAsync(
                _cmsKitTestData.User1Id,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                StandardReactions.Confused
                );

            UsingDbContext(context =>
            {
                var reaction = context.Set<UserReaction>().FirstOrDefault(x =>
                    x.CreatorId == _cmsKitTestData.User1Id &&
                    x.ReactionName == StandardReactions.Confused &&
                    x.EntityId == _cmsKitTestData.EntityId1 &&
                    x.EntityType == _cmsKitTestData.EntityType1);

                reaction.ShouldBeNull();
            });
        }
    }
}
