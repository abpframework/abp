using System;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.CmsKit.Blogs
{
    public class BlogManager_Test : CmsKitDomainTestBase
    {
        protected IBlogRepository BlogRepository { get; }
        protected BlogManager BlogManager { get; }
        protected CmsKitTestData TestData { get; }

        public BlogManager_Test()
        {
            BlogRepository = GetRequiredService<IBlogRepository>();
            BlogManager = GetRequiredService<BlogManager>();
            TestData = GetRequiredService<CmsKitTestData>();
        }

        [Fact]
        public async Task BlogCreate_ShouldThrowException_WithExistSlug()
        {
            await Should.ThrowAsync<BlogSlugAlreadyExistException>(
                async () =>
                await BlogManager.CreateAsync("test-name", TestData.BlogSlug)
                );
        }
        
        [Fact]
        public async Task BlogCreate_ShouldWorkProperly()
        {
            var blog = await BlogManager.CreateAsync("test-name", "test-slug");

            blog.ShouldNotBeNull();
            blog.Id.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task BlogUpdate_ShouldWork()
        {
            var blog = await BlogRepository.GetAsync(TestData.Blog_Id);
            
            await BlogManager.UpdateAsync(blog, "New name", "new-slug");

            blog.Name.ShouldBe("New name");
            blog.Slug.ShouldBe("new-slug");
        }
    }
}