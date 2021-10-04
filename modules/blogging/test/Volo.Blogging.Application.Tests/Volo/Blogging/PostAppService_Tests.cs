using System;
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
        private readonly IPostRepository _postRepository;

        public PostAppService_Tests()
        {
            _postAppService = GetRequiredService<IPostAppService>();
            _blogRepository = GetRequiredService<IBlogRepository>();
            _postRepository = GetRequiredService<IPostRepository>();
        }

        [Fact]
        public async Task Should_Get_List_Of_Posts()
        {
            var blogId = (await _blogRepository.GetListAsync()).First().Id;
            var posts = await _postAppService.GetListByBlogIdAndTagNameAsync(blogId, null);
            posts.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Get_For_Reading()
        {
            var blogId = (await _blogRepository.GetListAsync()).First().Id;
            var post = (await _postRepository.GetListAsync()).First(p=>p.BlogId == blogId);
            var postToRead = await _postAppService.GetForReadingAsync(new GetPostInput() {BlogId = blogId, Url = post.Url});

            postToRead.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Create_A_Post()
        {
            var blogId = (await _blogRepository.GetListAsync()).First().Id;
            var title = "test title";
            var content = "test content";
            var coverImage = "new.jpg";

            var newPostDto = await _postAppService.CreateAsync(new CreatePostDto()
            {
                BlogId = blogId,
                Title = title,
                Content = content,
                CoverImage = coverImage,
                Url = title.Replace(" ", "-")
            });

            newPostDto.Id.ShouldNotBe(Guid.Empty);

            UsingDbContext(context =>
            {
                var post = context.Posts.FirstOrDefault(q => q.Title == title);
                post.ShouldNotBeNull();
                post.Id.ShouldBe(newPostDto.Id);
                post.Title.ShouldBe(newPostDto.Title);
                post.Content.ShouldBe(newPostDto.Content);
                post.CoverImage.ShouldBe(newPostDto.CoverImage);
                post.BlogId.ShouldBe(blogId);
                post.Url.ShouldBe(newPostDto.Url);
            });
        }

        [Fact]
        public async Task Should_Update_A_Post()
        {
            var blogId = (await _blogRepository.GetListAsync()).First().Id;
            var newTitle = "new title";
            var newContent = "content";

            var oldPost = (await _postRepository.GetListAsync()).FirstOrDefault(); ;

            await _postAppService.UpdateAsync(oldPost.Id, new UpdatePostDto()
            {
                BlogId = blogId,
                Title = newTitle,
                CoverImage = oldPost.CoverImage,
                Content = newContent,
                Url = newTitle.Replace(" ", "-")
            });

            UsingDbContext(context =>
            {
                var post = context.Posts.FirstOrDefault(q => q.Id == oldPost.Id);
                post.Title.ShouldBe(newTitle);
                post.Content.ShouldBe(newContent);
            });
        }

        [Fact]
        public async Task Should_Delete_A_Post()
        {
            var post = (await _postRepository.GetListAsync()).First();

            await _postAppService.DeleteAsync(post.Id);
        }
    }
}
