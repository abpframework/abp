using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Xunit;

namespace Volo.Blogging
{
    public class CommentAppService_Tests : BloggingApplicationTestBase
    {
        private readonly ICommentAppService _commentAppService;
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public CommentAppService_Tests()
        {
            _commentAppService = GetRequiredService<ICommentAppService>(); ;
            _commentRepository = GetRequiredService<ICommentRepository>(); ;
            _postRepository = GetRequiredService<IPostRepository>(); ;
        }

        [Fact]
        public async Task Should_Get_List_Of_Comments()
        {
           var post = await _postRepository.InsertAsync(new Post(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "asd", "asd"));
            var comment1 = await _commentRepository.InsertAsync(new Comment(Guid.NewGuid(), post.Id, Guid.Empty, "qweasd"));
            var comment2 = await _commentRepository.InsertAsync(new Comment(Guid.NewGuid(), post.Id, comment1.Id, "qweasd"));

            var comments = await _commentRepository.GetListOfPostAsync(post.Id);

            comments.Count.ShouldBe(2);

        }
    }
}
