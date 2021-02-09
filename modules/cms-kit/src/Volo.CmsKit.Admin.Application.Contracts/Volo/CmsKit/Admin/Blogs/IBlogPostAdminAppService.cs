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
            CreateUpdateBlogPostDto>
    {
        Task<BlogPostDto> GetBySlugAsync(string blogSlug, string slug);

        Task SetCoverImageAsync(Guid id, RemoteStreamContent streamContent);

        Task<RemoteStreamContent> GetCoverImageAsync(Guid id);
    }
}
