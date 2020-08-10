using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Users;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit
{
    public class CmsKitDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICmsUserRepository _cmsUserRepository;
        private readonly CmsKitTestData _cmsKitTestData;
        private readonly IUserReactionRepository _userReactionRepository;
        private readonly ICommentRepository _commentRepository;

        public CmsKitDataSeedContributor(
            IGuidGenerator guidGenerator,
            ICmsUserRepository cmsUserRepository,
            CmsKitTestData cmsKitTestData,
            IUserReactionRepository userReactionRepository,
            ICommentRepository commentRepository)
        {
            _guidGenerator = guidGenerator;
            _cmsUserRepository = cmsUserRepository;
            _cmsKitTestData = cmsKitTestData;
            _userReactionRepository = userReactionRepository;
            _commentRepository = commentRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await _cmsUserRepository.InsertAsync(new CmsUser(new UserData(_cmsKitTestData.User1Id, "user1","user1@volo.com", "user","1")));
            await _cmsUserRepository.InsertAsync(new CmsUser(new UserData(_cmsKitTestData.User2Id, "user2","user2@volo.com", "user","2")));

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
    }
}
