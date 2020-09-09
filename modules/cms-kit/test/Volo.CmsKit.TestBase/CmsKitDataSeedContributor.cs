using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Users;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit
{
    public class CmsKitDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICmsUserRepository _cmsUserRepository;
        private readonly CmsKitTestData _cmsKitTestData;
        private readonly ICommentRepository _commentRepository;
        private readonly ReactionManager _reactionManager;
        private readonly IRatingRepository _ratingRepository;

        public CmsKitDataSeedContributor(
            IGuidGenerator guidGenerator,
            ICmsUserRepository cmsUserRepository,
            CmsKitTestData cmsKitTestData,
            ICommentRepository commentRepository,
            ReactionManager reactionManager,
            IRatingRepository ratingRepository)
        {
            _guidGenerator = guidGenerator;
            _cmsUserRepository = cmsUserRepository;
            _cmsKitTestData = cmsKitTestData;
            _commentRepository = commentRepository;
            _reactionManager = reactionManager;
            _ratingRepository = ratingRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedUsersAsync();

            await SeedCommentsAsync();

            await SeedReactionsAsync();

            await SeedRatingsAsync();
        }

        private async Task SeedUsersAsync()
        {
            await _cmsUserRepository.InsertAsync(new CmsUser(new UserData(_cmsKitTestData.User1Id, "user1",
                "user1@volo.com",
                "user", "1")));
            await _cmsUserRepository.InsertAsync(new CmsUser(new UserData(_cmsKitTestData.User2Id, "user2",
                "user2@volo.com",
                "user", "2")));
        }

        private async Task SeedCommentsAsync()
        {
            var comment1 = await _commentRepository.InsertAsync(new Comment(_cmsKitTestData.CommentWithChildId,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                "comment",
                null,
                _cmsKitTestData.User1Id
            ));

            await _commentRepository.InsertAsync(new Comment(_guidGenerator.Create(),
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                "reply",
                comment1.Id,
                _cmsKitTestData.User2Id
            ));

            await _commentRepository.InsertAsync(new Comment(_guidGenerator.Create(),
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                "reply",
                comment1.Id,
                _cmsKitTestData.User1Id
            ));

            await _commentRepository.InsertAsync(new Comment(_guidGenerator.Create(),
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                "comment",
                null,
                _cmsKitTestData.User2Id
            ));

            await _commentRepository.InsertAsync(new Comment(_guidGenerator.Create(),
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId2,
                "comment",
                null,
                _cmsKitTestData.User2Id
            ));

            await _commentRepository.InsertAsync(new Comment(_guidGenerator.Create(),
                _cmsKitTestData.EntityType2,
                _cmsKitTestData.EntityId1,
                "comment",
                null,
                _cmsKitTestData.User2Id
            ));
        }

        private async Task SeedReactionsAsync()
        {
            await _reactionManager.CreateAsync(
                _cmsKitTestData.User1Id,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                StandardReactions.Confused);

            await _reactionManager.CreateAsync(
                _cmsKitTestData.User1Id,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                StandardReactions.ThumbsUp);

            await _reactionManager.CreateAsync(
                _cmsKitTestData.User1Id,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId2,
                StandardReactions.Heart);

            await _reactionManager.CreateAsync(
                _cmsKitTestData.User1Id,
                _cmsKitTestData.EntityType2,
                _cmsKitTestData.EntityId1,
                StandardReactions.Rocket);

            await _reactionManager.CreateAsync(
                _cmsKitTestData.User2Id,
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                StandardReactions.ThumbsUp);
        }

        private async Task SeedRatingsAsync()
        {
            await _ratingRepository.InsertAsync(new Rating(_guidGenerator.Create(),
                    _cmsKitTestData.EntityType1,
                    _cmsKitTestData.EntityId1,
                    4,
                    _cmsKitTestData.User1Id
                ));
            
            await _ratingRepository.InsertAsync(new Rating(_guidGenerator.Create(),
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                5,
                _cmsKitTestData.User1Id
            ));
            
            await _ratingRepository.InsertAsync(new Rating(_guidGenerator.Create(),
                _cmsKitTestData.EntityType2,
                _cmsKitTestData.EntityId2,
                5,
                _cmsKitTestData.User2Id
            ));
            
            await _ratingRepository.InsertAsync(new Rating(_guidGenerator.Create(),
                _cmsKitTestData.EntityType2,
                _cmsKitTestData.EntityId2,
                1,
                _cmsKitTestData.User2Id
            ));
        }
    }
}