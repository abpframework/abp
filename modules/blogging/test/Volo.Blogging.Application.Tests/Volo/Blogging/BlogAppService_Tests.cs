using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;
using Volo.Blogging.Comments;
using Volo.Blogging.Comments.Dtos;
using Volo.Blogging.Posts;
using Xunit;

namespace Volo.Blogging
{
    public class BlogAppService_Tests : BloggingApplicationTestBase
    {
        private readonly IBlogAppService _blogAppService;
        private readonly IBlogRepository _blogRepository;

        public BlogAppService_Tests()
        {
            _blogAppService = GetRequiredService<IBlogAppService>();
            _blogRepository = GetRequiredService<IBlogRepository>();
        }

        [Fact]
        public async Task Should_Get_List_Of_Blogs()
        {
            var blogs = await _blogAppService.GetListAsync();

            blogs.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Should_Get_Blog_By_Shortname()
        {
            var targetBlog = (await _blogAppService.GetListAsync()).Items.First();

            var blog = await _blogAppService.GetByShortNameAsync(targetBlog.ShortName);

            blog.ShouldNotBeNull();
            blog.Name.ShouldBe(targetBlog.Name);
        }

        [Fact]
        public async Task Should_Create_A_Blog()
        {
            var name = "test name";
            var shortName = "test shortName";
            var description = "test description";

            var blogDto = await _blogAppService.Create(new CreateBlogDto() { Name = name, ShortName = name, Description = description });

            UsingDbContext(context =>
            {
                var blog = context.Blogs.FirstOrDefault(q => q.Id == blogDto.Id);
                blog.ShouldNotBeNull();
                blog.Name.ShouldBe(blogDto.Name);
                blog.ShortName.ShouldBe(blogDto.ShortName);
                blog.Description.ShouldBe(blogDto.Description);
            });
        }

        [Fact]
        public async Task Should_Update_A_Blog()
        {
            var newDescription = "new description";

            var oldBlog = (await _blogRepository.GetListAsync()).FirstOrDefault(); ;

            await _blogAppService.Update(oldBlog.Id, new UpdateBlogDto()
            { Description = newDescription, Name = oldBlog.Name, ShortName = oldBlog.ShortName });

            UsingDbContext(context =>
            {
                var blog = context.Blogs.FirstOrDefault(q => q.Id == oldBlog.Id);
                blog.Description.ShouldBe(newDescription);
            });
        }

        [Fact]
        public async Task Should_Delete_A_Blog()
        {
            var blog = (await _blogRepository.GetListAsync()).First();

            await _blogAppService.Delete(blog.Id);
        }
    }
}
