using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Users;
using Volo.CmsKit.Public.Reactions;
using Xunit;

namespace Volo.CmsKit.Reactions
{
    public class ReactionPublicAppService_Tests : CmsKitApplicationTestBase
    {

        private readonly CmsKitTestData _cmsKitTestData;
        private readonly ReactionPublicAppService _reactionPublicAppService;
        private ICurrentUser _currentUser;

        public ReactionPublicAppService_Tests()
        {
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
            _reactionPublicAppService = GetRequiredService<ReactionPublicAppService>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _currentUser = Substitute.For<ICurrentUser>();
            services.AddSingleton(_currentUser);
        }

        [Fact]
        public async Task GetForSelectionAsync()
        {
            _currentUser.Id.Returns(_cmsKitTestData.User1Id);
            _currentUser.IsAuthenticated.Returns(true);

            var reactions = await _reactionPublicAppService.GetForSelectionAsync(
                _cmsKitTestData.EntityType2,
                _cmsKitTestData.EntityId1
            );

            reactions.Items.
                First(r=>r.Reaction.Name == StandardReactions.Rocket).IsSelectedByCurrentUser.ShouldBeTrue();

            reactions.Items.
                First(r=>r.Reaction.Name == StandardReactions.Rocket).Count.ShouldBe(1);

            reactions.Items.
                Where(r=>r.Reaction.Name != StandardReactions.Rocket).All(r=>!r.IsSelectedByCurrentUser)
                .ShouldBeTrue();
            reactions.Items.
                Where(r=>r.Reaction.Name != StandardReactions.Rocket).All(r=> r.Count == 0)
                .ShouldBeTrue();
        }

        [Fact]
        public async Task CreateAsync()
        {
            _currentUser.Id.Returns(_cmsKitTestData.User1Id);

            await _reactionPublicAppService.CreateAsync(
                _cmsKitTestData.EntityType2,
                _cmsKitTestData.EntityId2,
                StandardReactions.Eyes
            );

            UsingDbContext(context =>
            {
                var reaction = context.Set<UserReaction>().Where(x =>
                    x.CreatorId == _cmsKitTestData.User1Id &&
                    x.ReactionName == StandardReactions.Eyes &&
                    x.EntityId == _cmsKitTestData.EntityId2 &&
                    x.EntityType == _cmsKitTestData.EntityType2).ToList();

                reaction.Count.ShouldBe(1);
            });
        }

        [Fact]
        public async Task DeleteAsync()
        {
            _currentUser.Id.Returns(_cmsKitTestData.User1Id);

            await _reactionPublicAppService.DeleteAsync(
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
