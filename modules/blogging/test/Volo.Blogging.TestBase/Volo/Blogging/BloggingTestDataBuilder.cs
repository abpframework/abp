using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Blogging.Blogs;
using Volo.Blogging.Posts;

namespace Volo.Blogging
{
    public class BloggingTestDataBuilder : ITransientDependency
    {
        private readonly BloggingTestData _testData;
        private readonly IBlogRepository _blogRepository;
        private readonly IPostRepository _postRepository;

        public BloggingTestDataBuilder(
            BloggingTestData testData,
            IBlogRepository blogRepository,
            IPostRepository postRepository)
        {
            _testData = testData;
            _blogRepository = blogRepository;
            _postRepository = postRepository;
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
        }
    }
}
