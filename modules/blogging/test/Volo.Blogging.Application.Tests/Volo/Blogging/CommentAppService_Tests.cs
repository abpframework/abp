using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Comments.Dtos;
using Volo.Blogging.Posts;
using Xunit;

namespace Volo.Blogging
{
    public class CommentAppService_Tests : BloggingApplicationTestBase
    {
        private readonly ICommentAppService _commentAppService;
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IBlogRepository _blogRepository;

        public CommentAppService_Tests()
        {
            _commentAppService = GetRequiredService<ICommentAppService>();
            _commentRepository = GetRequiredService<ICommentRepository>();
            _postRepository = GetRequiredService<IPostRepository>();
            _blogRepository = GetRequiredService<IBlogRepository>();
        }

        [Fact]
        public async Task Should_Get_List_Of_Comments()
        {
            var blog = (await _blogRepository.GetListAsync()).FirstOrDefault();
            var post = (await _postRepository.GetListAsync()).FirstOrDefault();
            var comment1 = await _commentRepository.InsertAsync(new Comment(Guid.NewGuid(), post.Id, null, "qweasd"));
            var comment2 = await _commentRepository.InsertAsync(new Comment(Guid.NewGuid(), post.Id, null, "qweasd"));

            var comments =
                await _commentAppService.GetHierarchicalListOfPostAsync(
                    new GetCommentListOfPostAsync() { PostId = post.Id });

            comments.Count.ShouldBe(2);
        }
    }
}
