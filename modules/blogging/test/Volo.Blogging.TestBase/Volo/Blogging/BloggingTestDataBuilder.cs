using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Blogging.Blogs;

namespace Volo.Blogging
{
    public class BloggingTestDataBuilder : ITransientDependency
    {
        private readonly BloggingTestData _testData;
        private readonly IBlogRepository _blogRepository;

        public BloggingTestDataBuilder(
            BloggingTestData testData,
            IBlogRepository blogRepository)
        {
            _testData = testData;
            _blogRepository = blogRepository;
        }

        public void Build()
        {
            AsyncHelper.RunSync(BuildAsync);
        }

        public async Task BuildAsync()
        {
            await _blogRepository.InsertAsync(new Blog(_testData.Blog1Id, "The First Blog", "blog-1"));
        }
    }
}
