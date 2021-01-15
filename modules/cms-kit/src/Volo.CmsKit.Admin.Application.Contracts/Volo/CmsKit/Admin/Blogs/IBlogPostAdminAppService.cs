using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Admin.Blogs
{
    public interface IBlogPostAdminAppService 
        : ICrudAppService<
            BlogPostDto, 
            Guid, 
            PagedAndSortedResultRequestDto,
            CreateUpdateBlogPostDto>
    {
        Task<BlogPostDto> GetByUrlSlugAsync(string urlSlug);
    }
}
