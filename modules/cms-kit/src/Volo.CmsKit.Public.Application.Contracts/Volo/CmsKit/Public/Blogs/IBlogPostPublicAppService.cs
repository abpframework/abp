using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace Volo.CmsKit.Public.Blogs
{
    public interface IBlogPostPublicAppService
    {
        Task<PagedResultDto<BlogPostPublicDto>> GetListAsync(string blogSlug, PagedAndSortedResultRequestDto input);

        Task<BlogPostPublicDto> GetAsync(string blogSlug, string blogPostSlug);

        Task<RemoteStreamContent> GetCoverImageAsync(Guid id);
    }
}
