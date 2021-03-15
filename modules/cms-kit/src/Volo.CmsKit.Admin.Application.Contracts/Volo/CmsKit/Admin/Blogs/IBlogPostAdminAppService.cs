using System;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Blogs
{
    public interface IBlogPostAdminAppService
        : ICrudAppService<
            BlogPostDto,
            Guid,
            BlogPostGetListInput,
            CreateBlogPostDto,
            UpdateBlogPostDto>
    {
    }
}
