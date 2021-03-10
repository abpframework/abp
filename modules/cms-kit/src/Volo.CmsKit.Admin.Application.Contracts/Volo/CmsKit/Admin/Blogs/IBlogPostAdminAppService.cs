using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Volo.CmsKit.Admin.Blogs
{
    public interface IBlogPostAdminAppService
        : ICrudAppService<
            BlogPostDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateBlogPostDto,
            UpdateBlogPostDto>
    {
    }
}
