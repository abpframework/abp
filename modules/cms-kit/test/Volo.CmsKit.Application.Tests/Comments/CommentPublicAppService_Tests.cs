using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Shouldly;
using Volo.Abp.Users;
using Volo.CmsKit.Public.Comments;
using Xunit;

namespace Volo.CmsKit.Comments
{
    public class CommentPublicAppService_Tests : CmsKitApplicationTestBase
    {
        private readonly ICommentPublicAppService _commentAppService;
        private ICurrentUser _currentUser;
        private readonly CmsKitTestData _cmsKitTestData;

        public CommentPublicAppService_Tests()
        {
            _commentAppService = GetRequiredService<ICommentPublicAppService>();
            _cmsKitTestData = GetRequiredService<CmsKitTestData>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            _currentUser = Substitute.For<ICurrentUser>();
            services.AddSingleton(_currentUser);
        }

        [Fact]
        public async Task GetAllForEntityAsync()
        {
            var list = await _commentAppService.GetListAsync(_cmsKitTestData.EntityType1, _cmsKitTestData.EntityId1);

            list.Items.Count.ShouldBe(2);
            list.Items.First().Replies.Count.ShouldBe(2);
        }

        [Fact]
        public async Task CreateAsync()
        {
            _currentUser.Id.Returns(_cmsKitTestData.User2Id);

            var newComment = await _commentAppService.CreateAsync(
                _cmsKitTestData.EntityType1,
                _cmsKitTestData.EntityId1,
                new CreateCommentInput
                {
                    RepliedCommentId = null,
                    Text = "newComment"
                }
            );

            UsingDbContext(context =>
            {
                var comments = context.Set<Comment>().Where(x =>
                    x.EntityId == _cmsKitTestData.EntityId1 && x.EntityType == _cmsKitTestData.EntityType1).ToList();

                comments
                    .Any(c=>c.Id == newComment.Id && c.CreatorId == newComment.CreatorId && c.Text == "newComment")
                    .ShouldBeTrue();
            });
        }

        [Fact]
        public async Task UpdateAsync()
        {
            _currentUser.Id.Returns(_cmsKitTestData.User1Id);

            await _commentAppService.UpdateAsync(_cmsKitTestData.CommentWithChildId, new UpdateCommentInput
            {
                Text = "I'm Updated"
            });

            UsingDbContext(context =>
            {
                var comment = context.Set<Comment>().Single(x =>
                    x.Id == _cmsKitTestData.CommentWithChildId);

                comment.Text.ShouldBe("I'm Updated");
            });
        }

        [Fact]
        public async Task DeleteAsync()
        {
            _currentUser.Id.Returns(_cmsKitTestData.User1Id);

            await _commentAppService.DeleteAsync(_cmsKitTestData.CommentWithChildId);

            UsingDbContext(context =>
            {
                var comment = context.Set<Comment>().FirstOrDefault(x =>
                    x.Id == _cmsKitTestData.CommentWithChildId);

                comment.ShouldBeNull();
            });
        }
    }
}
