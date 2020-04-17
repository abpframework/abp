using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Blogging.Posts
{
    public abstract class PostRepository_Tests<TStartupModule> : BloggingTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IPostRepository PostRepository { get; }
        protected BloggingTestData BloggingTestData { get; }

        protected PostRepository_Tests()
        {
            PostRepository = GetRequiredService<IPostRepository>();
            BloggingTestData = GetRequiredService<BloggingTestData>();
        }

        [Fact]
        public async Task GetListOfPostAsync()
        {
            var posts = await PostRepository.GetPostsByBlogId(BloggingTestData.Blog1Id);
            posts.ShouldNotBeNull();
            posts.Count.ShouldBe(2);
            posts.ShouldContain(x => x.Id == BloggingTestData.Blog1Post1Id);
            posts.ShouldContain(x => x.Id == BloggingTestData.Blog1Post2Id);
        }

        [Fact]
        public async Task GetPostByUrl()
        {
            var post = await PostRepository.GetPostByUrl(BloggingTestData.Blog1Id, "url");
            post.ShouldNotBeNull();
            post.Url.ShouldBe("url");
        }
    }
}
