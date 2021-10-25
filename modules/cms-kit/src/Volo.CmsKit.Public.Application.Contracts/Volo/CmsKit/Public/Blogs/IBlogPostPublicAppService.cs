using JetBrains.Annotations;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.CmsKit.Public.Blogs
{
    public interface IBlogPostPublicAppService : IApplicationService
    {
        Task<PagedResultDto<BlogPostPublicDto>> GetListAsync([NotNull] string blogSlug, PagedAndSortedResultRequestDto input);

        Task<BlogPostPublicDto> GetAsync([NotNull] string blogSlug, [NotNull] string blogPostSlug);
    }
}
