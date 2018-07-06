using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Blogging.Blogs;
using Volo.Blogging.Posts;
using Xunit;

namespace Volo.Blogging
{
    public class PostAppService_Tests : BloggingApplicationTestBase
    {
        private readonly IPostAppService _postAppService;
        private readonly IBlogRepository _blogRepository;
        private readonly BloggingTestData _testData;

        public PostAppService_Tests()
        {
            _testData = GetRequiredService<BloggingTestData>();
            _postAppService = GetRequiredService<IPostAppService>();
            _blogRepository = GetRequiredService<IBlogRepository>();
        }

        [Fact]
        public async Task Should_Create_A_Post()
        {
            var blogId = (await _blogRepository.GetListAsync()).First().Id;
            var title = "test title";
            var content = "test content";

            var newPostDto = await _postAppService.CreateAsync(new CreatePostDto()
            {
                BlogId = blogId,
                Title = title,
                Content = content,
                Url = title.Replace(" ", "-")
            });

            newPostDto.Id.ShouldNotBeNull();

            UsingDbContext(context =>
            {
                var post = context.Posts.FirstOrDefault(q => q.Title == title);
                post.ShouldNotBeNull();
                post.Id.ShouldBe(newPostDto.Id);
                post.Title.ShouldBe(newPostDto.Title);
                post.Content.ShouldBe(newPostDto.Content);
                post.BlogId.ShouldBe(blogId);
                post.Url.ShouldBe(newPostDto.Url);
            });
        }

        [Fact]
        public async Task Should_Create_And_Update_A_Post()
        {
            var blogId = (await _blogRepository.GetListAsync()).First().Id;
            var title = "title";
            var newTitle = "new title";
            var content = "content";

            var newPost = await _postAppService.CreateAsync(new CreatePostDto()
            {
                BlogId = blogId,
                Title = title,
                Content = content,
                Url = title.Replace(" ", "-")
            });

            await _postAppService.UpdateAsync(newPost.Id, new UpdatePostDto()
            {
                BlogId = blogId,
                Title = newTitle,
                Content = content,
                Url = newTitle.Replace(" ", "-")
            });
            
            UsingDbContext(context =>
            {
                var post = context.Posts.FirstOrDefault(q => q.Id == newPost.Id);
                post.ShouldNotBeNull();
                post.Title.ShouldBe(newTitle);
                post.Content.ShouldBe(content);
                post.BlogId.ShouldBe(blogId);
                post.Url.ShouldBe(newTitle.Replace(" ", "-"));
            });
        }
    }
}
