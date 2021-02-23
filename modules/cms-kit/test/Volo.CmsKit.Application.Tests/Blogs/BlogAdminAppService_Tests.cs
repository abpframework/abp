using System.Threading.Tasks;
using Shouldly;
using Volo.CmsKit.Admin.Blogs;
using Xunit;

namespace Volo.CmsKit.Blogs
{
    public class BlogAdminAppService_Tests : CmsKitApplicationTestBase
    {
        private readonly IBlogRepository blogRepository;
        private readonly IBlogAdminAppService blogAdminAppService;
        private readonly CmsKitTestData cmsKitTestData;
        
        public BlogAdminAppService_Tests()
        {
            blogRepository = GetRequiredService<IBlogRepository>();
            blogAdminAppService = GetRequiredService<IBlogAdminAppService>();
            cmsKitTestData = GetRequiredService<CmsKitTestData>();
        }
        
        [Fact]
        public async Task DeleteAsync_ShouldThrowError_WhileItHasPosts()
        {
            await Should.ThrowAsync<BlogCannotBeDeletedException>(async () =>
                await blogAdminAppService.DeleteAsync(cmsKitTestData.Blog_Id));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete_WhenNoPosts()
        {
            var blog = await blogRepository.FindAsync(cmsKitTestData.Blog_2_Id);

            blog.ShouldNotBeNull();
            
            await Should.NotThrowAsync(async () => await blogAdminAppService.DeleteAsync(cmsKitTestData.Blog_2_Id));

            blog = await blogRepository.FindAsync(cmsKitTestData.Blog_2_Id);
            
            blog.ShouldBeNull();
        }
    }
}
