using System;
using Shouldly;
using Volo.CmsKit.Admin.Blogs;

namespace Volo.CmsKit.Blogs
{
    public class BlogPostAdminAppService_Tests : CmsKitApplicationTestBase
    {
        private readonly IBlogPostAdminAppService blogPostAdminAppService;

        public BlogPostAdminAppService_Tests()
        {
            blogPostAdminAppService = GetRequiredService<IBlogPostAdminAppService>();
        }
    }
}
