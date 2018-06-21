using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Blogging.Blogs
{
    public abstract class BlogRepository_Tests<TStartupModule> : BloggingTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IBlogRepository BlogRepository { get; }

        protected BlogRepository_Tests()
        {
            BlogRepository = GetRequiredService<IBlogRepository>();
        }

        [Fact]
        public async Task FindByShortNameAsync_Temp()
        {
            var blog = await BlogRepository.FindByShortNameAsync("default");
            blog.ShouldBeNull();
        }
    }
}