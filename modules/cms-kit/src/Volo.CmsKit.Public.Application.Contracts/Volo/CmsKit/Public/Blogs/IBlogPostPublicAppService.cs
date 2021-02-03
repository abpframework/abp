using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.Blogs
{
    public interface IBlogPostPublicAppService
    {
        Task<PagedResultDto<BlogPostPublicDto>> GetListAsync(string blogUrlSlug, PagedAndSortedResultRequestDto input);

        Task<BlogPostPublicDto> GetAsync(string blogUrlSlug, string blogPostUrlSlug);
    }
}
