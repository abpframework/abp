using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;

namespace Volo.Blogging
{
    public class BloggingTestDataBuilder : ITransientDependency
    {
        private readonly BloggingTestData _testData;
        private readonly IBlogRepository _blogRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ITagRepository _tagRepository;

        public BloggingTestDataBuilder(
            BloggingTestData testData,
            IBlogRepository blogRepository,
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            ITagRepository tagRepository)
        {
            _testData = testData;
            _blogRepository = blogRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
        }

        public void Build()
        {
            AsyncHelper.RunSync(BuildAsync);
        }

        public async Task BuildAsync()
        {
            await _blogRepository.InsertAsync(new Blog(_testData.Blog1Id, "The First Blog", "blog-1"));
            await _postRepository.InsertAsync(new Post(_testData.Blog1Post1Id, _testData.Blog1Id, Guid.Empty, "title", "coverImage", "url"));
            await _postRepository.InsertAsync(new Post(_testData.Blog1Post2Id, _testData.Blog1Id, Guid.Empty, "title", "coverImage", "url"));
            await _commentRepository.InsertAsync(new Comment(_testData.Blog1Post1Comment1Id,_testData.Blog1Post1Id,null,"text"));
            await _tagRepository.InsertAsync(new Tag(_testData.Blog1Id, _testData.Tag1Name));
        }
    }
}
