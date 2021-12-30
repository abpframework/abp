using System;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Blogs;

public interface IBlogPostAdminAppService
    : ICrudAppService<
        BlogPostDto,
        BlogPostListDto,
        Guid,
        BlogPostGetListInput,
        CreateBlogPostDto,
        UpdateBlogPostDto>
{
}
