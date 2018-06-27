using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var title = "title";
            var content = "content";

            var newPost = await _postAppService.CreateAsync(new CreatePostDto()
            {
                BlogId = blogId,
                Title = title,
                Content = content
            });

            UsingDbContext(context =>
            {
                var post = context.Posts.FirstOrDefault(q => q.Title == title);
                post.ShouldNotBeNull();
                post.Title.ShouldBe(title);
                post.Content.ShouldBe(content);
                post.BlogId.ShouldBe(blogId);
            });
        }

        [Fact]
        public async Task Should_Create_And_Update_A_Post()
        {
            var blogId = (await _blogRepository.GetListAsync()).First().Id;
            var title = "title";
            var newTitle = "newtitle";
            var content = "content";

            var newPost = await _postAppService.CreateAsync(new CreatePostDto()
            {
                BlogId = blogId,
                Title = title,
                Content = content
            });

            await _postAppService.UpdateAsync(newPost.Id, new UpdatePostDto()
            {
                BlogId = blogId,
                Title = newTitle,
                Content = content
            });


            UsingDbContext(context =>
            {
                var post = context.Posts.FirstOrDefault(q => q.Id == newPost.Id);
                post.ShouldNotBeNull();
                post.Title.ShouldBe(newTitle);
                post.Content.ShouldBe(content);
                post.BlogId.ShouldBe(blogId);
            });
        }
    }
}
